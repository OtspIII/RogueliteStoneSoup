using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//This attribute makes it visible in the Inspector and makes it easier to pack into a save file
[System.Serializable]
public class ThingInfo //The class that handles all the core info for all non-wall objects in the game
{
    public string Name = "";       //The thing's name
    public ThingController Thing;  //A link to their game object's script
    public ThingOption Type;       //A link to the Option that spawned them
    public ThingInfo ChildOf;      //If they 'belong' to something. Ie-a projectile is 'owned' by the thing that shot it
    public GameTeams Team;         //Used to prevent friendly fire between things on the same team
    public ThingInfo CurrentHeld;  //The item being currently held, if any
    
    //These variables handle the Trait subsystem within a Thing
    //Connects enums to the trait's state variables
    public Dictionary<Traits, TraitInfo> Trait = new Dictionary<Traits, TraitInfo>(); 
    //Traits can register the types of events they listen for. PreListen runs first, then TakeListen
    public Dictionary<EventTypes, List<Traits>> PreListen = new Dictionary<EventTypes, List<Traits>>();
    public Dictionary<EventTypes, List<Traits>> TakeListen = new Dictionary<EventTypes, List<Traits>>();
    //Sometimes Events cause more events. Those get added to a queue, rather than being resolved instantly by default
    public List<EventInfo> EventQueue = new List<EventInfo>();
    //A variable that marks if we're in the middle of resolving an event (ie-if a new event should go in the queue or not)
    [HideInInspector]public bool MidEvent = false;
    

    //These variables are just used for internal bookkeeping
    [HideInInspector]public bool Setup = false;     //Have I run setup already? Prevents setup from running twice
    [HideInInspector]public bool Destroyed = false; //Have I been marked for destruction? So I don't trigger death effects multiple times
    public ThingInfo Target;         //Is there a thing I'm targeting? Record it here
    public TraitInfo ActorTrait;     //A link to my ActorTrait, if any. For efficiency
    public float AttackRange = 1.5f; //My range from my target before I use my held item. So I don't need to ask my traits constantly
    public float VisionRange = 4;    //My detection range before I see the player
    public float CurrentSpeed;       //Set by traits so you don't need to recalc constantly
    public Vector2 DesiredMove;      //What direction do I want to be moving in? Used by Actions
    
    public ThingInfo(ThingOption o=null)
    {
        //A constructor. Just records the Option that spawned us, if any
        Type = o;
    }
    
    //Spawns the GameObject of the Thing at a point chosen and sets it up
    public ThingController Spawn(SpawnPointController where)
    { return Spawn(where.transform.position, where.transform.rotation.eulerAngles.z); }
    public ThingController Spawn(Vector3 pos)
    { return Spawn(pos, 0); }
    public ThingController Spawn(Vector3 pos, float rot)
    {
        //Instantiate our GameObject and set a few of its variables to mirror ours
        Thing = GameObject.Instantiate(God.Library.ActorPrefab, pos, Quaternion.Euler(0, 0, rot));
        Thing.Info = this;
        //If this is the first time we've spawned, set up all our listeners
        if (!Setup)
        {
            foreach (EventTypes e in PreListen.Keys) SortListen(e,true);
            foreach (EventTypes e in TakeListen.Keys) SortListen(e,false);
            //Tell our Traits that we just came into existence.
            Thing.TakeEvent(EventTypes.Setup);
        }
        Thing.gameObject.name = Name;
        Thing.NameText.text = GetName(true);
        //Spawn and set up our body and tell everything what team it's on
        Thing.Body = GameObject.Instantiate(Type.GetBody(), Thing.transform);
        Thing.Body.Setup(Thing,Type);
        Thing.SetTeam(Team);
        //Tell our traits we just spawned a GameObject!
        TakeEvent(EventTypes.OnSpawn);
        //Mark us as having been setup, so we don't do setup things twice
        Setup = true;
        return Thing; //Return the gameobject script we just made
    }
    
    //###############Info Functions###################
    //These functions let you ask some basic variable questions to the Thing
    
    ///Returns the 'owner' of this thing. This is usually itself, but projectiles are 'owned' by the character who shot them
    /// Imagine: if you died from touching this thing in a deathmatch shooter, who would get credit for the kill?
    public ThingInfo GetOwner(bool selfOk=true,bool ultimate = true) //if ultimate is false just return immediate parent
    {
        //Make a variable we'll eventually return as the answer
        //Our first guess is based on our parameters--is ourself a valid answer? If selfOk is true, then yes
        ThingInfo r = selfOk ? this : ChildOf;
        int safety = 99; //Make a safety for the while loop
        //For as long as our current answer has a parent, keep setting the answer to that parent and then trying again
        while (safety > 0 && r.ChildOf != null)
        {
            safety--;
            r = r.ChildOf;
            if (!ultimate) return r; //If ultimate is set to false, just do this once, though
        }
        return r; //Return the answer
    }

    ///Returns the thing's name. Some traits might change a thing's name, so ask them for it
    public string GetName(bool raw=false)
    {
        if (raw) return Name;
        return Ask(God.E(EventTypes.ShownName).Set(Name)).GetString();
    }
    
    //###############Trait Functions###################
    //These functions relate to either setting up or using the Trait system
    
    ///Adds a new trait to the Thing
    public TraitInfo AddTrait(Traits t,EventInfo i=null,EventInfo e=null)
    {
        //If we add a trait mid-game it throws an event to let other traits know
        if (Setup)
        {
            //We can include an EventInfo that tells us more about how we got the trait
            //But if we didn't make a blank one
            if (e == null) e = God.E();
            e.Type = EventTypes.GainTrait;
            e.Set(t);
            //Pass that event along to all our other traits
            TakeEvent(e, true);
            //If any of those events yelled 'Abort', we're immune to the trait and shouldn't gain it
            if (e.Abort) return null;
        }
        //Get our info already assodiated with the trait, if we have any
        TraitInfo r = Get(t);
        //If we found some, it means we're re-upping an existing trait, not gaining a new one
        //Example: we got hit with an attack that sets us on fire, but we're already on fire
        if (r != null)
        {
            //Tell the trait to run its 'ReUp' code
            r.ReUp(i);
        }
        else //Otherwise, we're gaining a new trait
        {
            //Make new trait info and add it to us/set it up
            r = new TraitInfo(t, this, i);
            Trait.Add(t,r);
            r.Init();
        }
        return r; //Return the trait info
    }
    
    ///Removes an existing trait from a thing
    public bool RemoveTrait(Traits t,EventInfo e=null)
    {
        TraitInfo r = Get(t);  //Get our existing trait info, if any exists
        if (r == null) return false; //If we don't find any, we don't have the trait so we can abort
        //If this is happening mid-game, we shoot an event to our traits about it
        if (Setup)
        {
            if (e == null) e = God.E();
            e.Type = EventTypes.LoseTrait;
            e.Set(t);
            TakeEvent(e, true);
            if (e.Abort) return false; //If any of them object, we don't lose the trait
        }
        r.Remove(e); //Tell the trait to remove itself
        return true;
    }
    
    ///Returns the trait info of a selected trait
    public TraitInfo Get(Traits t)
    {
        if (Trait.TryGetValue(t, out TraitInfo r)) return r;
        return null;
    }

    ///Returns true if a Thing has a trait, false otherwise
    public bool Has(Traits t)
    {
        return Trait.ContainsKey(t);
    }
    
    ///Adds a listener for a certain type of event, so that when you get the event you tell a specific trait about it
    ///There are two types of listeners: pre and normal. Pre runs first, and is for making changes to the event itself
    public void AddListen(EventTypes e, Traits t,bool pre = false)
    {
        //Which dictionary are we adding this to? Pre or normal listen?
        Dictionary<EventTypes, List<Traits>> d = pre ? PreListen : TakeListen;
        if(!d.ContainsKey(e)) d.Add(e,new List<Traits>()); //If the dictionary doesn't already have this event, add it
        if(!d[e].Contains(t)) d[e].Add(t);                 //If the event doesn't have this trait, add it
        //Listens have priority. If this happened mid-game, recalculate the order traits hear about the events in
        if(Setup) SortListen(e,pre);
    }

    ///Removes the listeners for an event for a trait
    public void RemoveListen(EventTypes e, Traits t, bool pre = false)
    {
        Dictionary<EventTypes, List<Traits>> d = pre ? PreListen : TakeListen;
        if (!d.ContainsKey(e)) return;
        List<Traits> l = d[e];
        l.Remove(t);
    }

    ///Traits can set what order they get events in
    /// Ie-Invincible should handle TakeDamage before Health, so it can negate the damage
    /// This function makes a list of them in order
    public void SortListen(EventTypes e, bool pre = false)
    {
        //#######BOOKMARK########
        List<Traits> l = pre ? PreListen[e] : TakeListen[e];
        if (l.Count <= 1) return;
        Dictionary<Traits, float> prio = new Dictionary<Traits, float>();
        foreach (Traits t in l)
        {
            Trait tr = Parser.Get(t);
            float pr = pre ? tr.PreListen[e] : tr.TakeListen[e];
            prio.Add(t,pr);
        }
        l.Sort((a, b) => { return prio[a] > prio[b] ? 1 : -1; });
    }
    
    public void TakeEvent(EventTypes e)
    {
        TakeEvent(new EventInfo(e));
    }
    public void TakeEvent(EventInfo e,bool instant=false,int safety=999)
    {
        // Debug.Log("TAKE EVENT B: " + e.Type + " / " + PreListen.Keys.Count + " / " + TakeListen.Keys.Count + " / " + Name);
        safety--;
        if (safety <= 0)
        {
            Debug.Log("INFINITE EVENT LOOP: " + e);
            return;
        }
        if (MidEvent && !instant)
        {
            EventQueue.Add(e);
            return;
        }
        MidEvent = true;
        PreListen.TryGetValue(e.Type, out List<Traits> pre);
        if(pre != null) {
            foreach (Traits t in pre)
            {
                Get(t).PreEvent(e);
                if (e.Abort) break;
            }
        }

        if (e.Abort) return;
        
        TakeListen.TryGetValue(e.Type, out List<Traits> take);
        // Debug.Log("TAKE EVENT C: " + e.Type + " / " + (take == null ? "X" : take.Count));
        if(take != null)
            foreach (Traits t in take)
            {
                Get(t).TakeEvent(e);
                if(e.Abort) break;
            }
        MidEvent = false;
        if (EventQueue.Count > 0)
        {
            EventInfo next = EventQueue[0];
            EventQueue.RemoveAt(0);
            TakeEvent(next,false,safety);
        }
    }

    public EventInfo Ask(EventTypes e, bool wpn = false)
    {
        EventInfo r = God.E(e);
        return Ask(r, wpn);
    }
    
    public EventInfo Ask(EventInfo e,bool wpn=false,int safety=99)
    {
        safety--;
        if (safety <= 0)
        {
            Debug.Log("INFINITE ASK LOOP: " + e);
            return e;
        }
        TakeEvent(e,true);
        if (wpn && CurrentHeld != null)
        {
            CurrentHeld.Ask(e, wpn, safety);
        }
        return e;
    }

    
    public void SetHeld(ItemOption w,bool dropOld=false)
    {
        if (w == null) return; 
        ThingInfo weap = w.Create();
        // ThingInfo weap = God.Library.GetWeapon(w);//ThingBuilder.Things[w]);
        TraitInfo i = weap.Get(Traits.Tool);
        SetHeld(weap);
        // CurrentWeapon = new TraitInfo(Traits.Holdable,null,weap.Traits[Traits.Holdable]);//God.Library.GetWeapon(stats.Weapon);
        // CurrentWeapon.Seed = weap;
    }

    public void SetHeld(int n)
    {
        if (God.Session.PlayerInventory.Count == 0) return;
        n = Mathf.Clamp(n,0,God.Session.PlayerInventory.Count - 1);
        SetHeld(God.Session.PlayerInventory[n]);
    }
    public void SetHeld(ThingInfo w)
    {
        w.Team = Team;
        TraitInfo i = w.Get(Traits.Tool);
        // Debug.Log("SET WEAPON: " + Name + " / " + w.Name + " / " + i);
        CurrentHeld = w;
        if (Thing != null)
        {
            Thing.Body.SetHeld(w);
        }
        w.TakeEvent(God.E(EventTypes.OnHoldStart).Set(this));
}
    
    public void DropHeld(bool destroy=false)
    {
        ThingInfo w = CurrentHeld;
        if (w == null) return;
        God.Session.RemoveInventory(w);
        CurrentHeld = null;
        w.Team = w.Type.Team;
        
        if (Thing != null)
        {
            Thing.DropHeld();
            if (!destroy)
            {
                w.Spawn(Thing.transform.position);
                w.TakeEvent(God.E(EventTypes.OnDrop).Set(this));
                TakeEvent(God.E(EventTypes.DidDrop).Set(w));
            }
        }
    }
    
    public void DestroyForm()
    {
        if (Thing == null) return;
        GameObject.Destroy(Thing.gameObject);
        Thing = null;
    }
    
    public void Destruct(ThingInfo source=null)
    {
        if (Destroyed) return;
        Destroyed = true;
        TakeEvent(God.E(EventTypes.OnDestroy).Set(source).Set(Thing.transform.position));
        DestroyForm();
    }

    public override string ToString()
    {
        string tr = "";
        foreach (Traits t in Trait.Keys)
        {
            if (tr != "") tr += ",";
            tr += t.ToString();
        }
        return Name + "("+ActorTrait?.ActScript?.ToString()+"/"+CurrentHeld?.Name+"/"+tr+")";
    }


    

    
}


///Dictates what objects collide. Things on Players & Enemies teams don't do friendly fire or block movement 
public enum GameTeams 
{
    None=0,
    Neutral=1,
    Player=2,
    Enemy=3,
}