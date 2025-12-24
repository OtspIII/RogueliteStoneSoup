using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ThingInfo
{
    public string Name = "";
    public ThingController Thing;
    public ThingOption Type;
    public ThingInfo ChildOf;
    
    public Dictionary<Traits, TraitInfo> Trait = new Dictionary<Traits, TraitInfo>();
    public Dictionary<EventTypes, List<Traits>> PreListen = new Dictionary<EventTypes, List<Traits>>();
    public Dictionary<EventTypes, List<Traits>> TakeListen = new Dictionary<EventTypes, List<Traits>>();
    public List<EventInfo> EventQueue = new List<EventInfo>();
    public bool MidEvent = false;
    
    public TraitInfo CurrentWeapon;

    public ThingInfo Target;
    public TraitInfo ActorTrait;
    public float AttackRange = 1.5f;
    public float VisionRange = 4;
    public float CurrentSpeed; //Set by traits so you don't need to recalc constantly
    public Vector2 DesiredMove;
    public Vector2 Knockback;

    public ThingInfo()
    {
    }
    
    public ThingInfo(ThingOption o=null)
    {
        Type = o;
    }
    
    public ThingController Spawn(SpawnPointController where)
    {
        return Spawn(where.transform.position, where.transform.rotation.eulerAngles.z);
    }
    
    public ThingController Spawn(Vector3 pos)
    {
        return Spawn(pos, 0);
    }
    
    public virtual ThingController Spawn(Vector3 pos, float rot)
    {
        Thing = GameObject.Instantiate(God.Library.ActorPrefab, pos, Quaternion.Euler(0, 0, rot));
        Thing.Info = this;
        Thing.gameObject.name = Name;
        // Debug.Log("SPAWN: " + Name);
        Thing.TakeEvent(EventTypes.Setup);
        Thing.Body = GameObject.Instantiate(Type.Body, Thing.transform);
        Thing.Body.Setup(Thing,Type.Art);
        if(Thing.Body.Anim != null)
            Thing.Body.Anim.Rebind();
        return Thing;
    }
    
    public TraitInfo AddTrait(Traits t,EventInfo i=null)
    {
        TraitInfo r = Get(t);
        if (r != null)
        {
            // Debug.Log("AT UP: " + t + " / " + Name);
            r.ReUp(i);
        }
        else
        {
            // Debug.Log("AT ADD: " + t + " / " + Name);
            r = new TraitInfo(t, this, i);
            Trait.Add(t,r);
            // if(init) 
            r.Init();
        }
        return r;
    }
    
    public TraitInfo Get(Traits t)
    {
        if (Trait.TryGetValue(t, out TraitInfo r)) return r;
        return null;
    }

    public bool Has(Traits t)
    {
        return Trait.ContainsKey(t);
    }
    
    
    public void AddListen(EventTypes e, Traits t,bool pre = false)
    {
        // Debug.Log("ADD LISTEN: " + e + " / " + Name);
        Dictionary<EventTypes, List<Traits>> d = pre ? PreListen : TakeListen;
        if(!d.ContainsKey(e)) d.Add(e,new List<Traits>());
        if(!d[e].Contains(t)) d[e].Add(t);
    }
    
    
    public void SetWeapon(HoldableOption w)
    {
        ThingInfo weap = w.Create();
        // ThingInfo weap = God.Library.GetWeapon(w);//ThingBuilder.Things[w]);
        TraitInfo i = weap.Get(Traits.Holdable);
        SetWeapon(weap);
        // CurrentWeapon = new TraitInfo(Traits.Holdable,null,weap.Traits[Traits.Holdable]);//God.Library.GetWeapon(stats.Weapon);
        // CurrentWeapon.Seed = weap;
    }

    public void SetWeapon(ThingInfo w)
    {
        TraitInfo i = w.Get(Traits.Holdable);
        // Debug.Log("SET WEAPON: " + Name + " / " + w.Name + " / " + i);
        CurrentWeapon = i;
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
        if(pre != null) foreach (Traits t in pre) Get(t).PreEvent(e);

        if (e.Abort) return;
        
        TakeListen.TryGetValue(e.Type, out List<Traits> take);
        // Debug.Log("TAKE EVENT C: " + e.Type + " / " + (take == null ? "X" : take.Count));
        if(take != null) foreach (Traits t in take) Get(t).TakeEvent(e);
        MidEvent = false;
        if (EventQueue.Count > 0)
        {
            EventInfo next = EventQueue[0];
            EventQueue.RemoveAt(0);
            TakeEvent(next,false,safety);
        }
    }
    
    public EventInfo Ask(EventTypes e)
    {
        EventInfo r = God.E(e);
        TakeEvent(r,true);
        return r;
    }
    
    public bool IsPlayer()
    {
        return Has(Traits.Player) || (ChildOf != null && ChildOf.Has(Traits.Player));
    }

    public void Destruct()
    {
        GameObject.Destroy(Thing.gameObject);
    }

}
