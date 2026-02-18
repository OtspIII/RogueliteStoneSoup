using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ThingController : MonoBehaviour
{
    //If you ask me my name, I'll give you the name from my info
    public string Name {get {return Info.Name;} set { Info.Name = value;}}
    [HideInInspector] public Rigidbody2D RB; //I spawn my own RB, so hide this from the inspector
    public BodyController Body;   //My body, which I spawn as a separate prefab
    public BodyController HeldBody; //My currently equipped item's body
    public SpriteRenderer Icon;   //I have a little sprite that can be used for context action icons. 'Hit E to pick me up' type stuff
    public TextMeshPro NameText;  //I have a textmesh that displays my name. You can see it by holding shift.
    public Collider2D NoClip;     //I have a tiny collider that only collides with walls. To make sure I don't escape the stage.
    public ThingInfo Info;        //My Info. The core of who I am. Most of the code that controls me lives here.
    public ThingInfo CurrentHeld {get {return Info.CurrentHeld;} set { Info.CurrentHeld = value;}}  //My currently equipped item's info
    [Header("Movement Bookkeeping")]
    public Vector2 ActualMove;
    public Vector2 Knockback;
    public List<ThingInfo> CanSee; //A list of all the Things you can currently see
    public List<RoomScript> CurrentRooms;
    [Header("Debug Info")]
    public Vector3 StartSpot;     //I write down where I first spawned, just so I know
    public string DebugTxt;       //This just exists as a secondary debug.log. Set it and check its status in the inspector
    
    public void Awake()
    {
        StartSpot = transform.position;    //Record our start spot
        RB = GetComponent<Rigidbody2D>();  //If we have a rigidbody, note it
    }

    public void Start()
    {
        //When I spawn, add me to the list of Things that exist that's on the GameManager
        if(God.GM != null && !God.GM.Things.Contains(this)) God.GM.Things.Add(this);
        //When I spawn, display my name if the 'show names' button is currently held
        NameText.gameObject.SetActive(Input.GetKey(God.InfoKey));
    }

    public void Update()
    {
        //If the player hits the 'show name' key, show my name
        if (Input.GetKeyDown(God.InfoKey)) NameText.gameObject.SetActive(true);
        //If the player lets go of the 'show name' key, hide my name
        if (Input.GetKeyUp(God.InfoKey)) NameText.gameObject.SetActive(false);
        //If a new frame starts and we're in the middle of resolving an Event, that means the event bugged out and didn't finish
        if (Info.MidEvent)
        {
            God.LogError("Thing Got Stuck Mid-Action: " + Name);
            Info.MidEvent = false;
        }
        //Tell all my traits to run their 'every frame' code
        TakeEvent(EventTypes.Update);
    }
    
    private void FixedUpdate()
    {
        if (RB != null)
        {
            //If I'm suffering knockback, resolve it.
            if (Knockback != Vector2.zero)
            {
                //My knockback gets weaker every frame, and once it's weak enough it sets fully to 0
                Knockback *= 0.9f;
                if (Knockback.magnitude < 0.1)
                    Knockback = Vector2.zero;
            }

            //Actual assignment of velocity to the rigidbody
            //ActualMove is calculated by the thing's Action
            RB.linearVelocity = ActualMove + Knockback;
        }
        TakeEvent(EventTypes.FixedUpdate);
    }
    
    //When I get destroyed, tell the GM to take me off the list of everything that exists
    void OnDestroy()
    {
        if(God.GM != null) God.GM.Things.Remove(this);
    }

    ///Adds a rigidbody to the Thing. Called by some Traits during setup.
    public void AddRB()
    {
        //If I already have a RB, don't add a second one
        if (RB != null) return;
        RB = gameObject.AddComponent<Rigidbody2D>();
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        RB.gravityScale = 0;
    }

    ///Creates a new hitbox of a certain type.
    public HitboxController AddHitbox(HitboxTypes t, float size = 1)
    {
        HitboxController r = Instantiate(God.Library.Hitbox, transform);
        r.gameObject.name = t + " Hitbox";
        r.transform.localPosition = Vector3.zero;
        ((CircleCollider2D)r.Coll).radius = size;
        r.Setup(t,this);
        return r;
    }

    //#################Event Shortcuts###################
    //Most of the real code for these lives on ThingInfo, but I added a function here that just calls that for QoL purposes
    //If you want to know what they do, look at their ThingInfo equivalents
    
    public EventInfo Ask(EventTypes e, bool wpn = false) { return Info.Ask(e,wpn); }
    public EventInfo Ask(EventInfo e, bool wpn = false) { return Info.Ask(e,wpn); }
    public void TakeEvent(EventTypes e,bool instant=false,int safety=999) { Info.TakeEvent(new EventInfo(e)); }
    public void TakeEvent(EventInfo e,bool instant=false,int safety=999) { Info.TakeEvent(e,instant,safety); }
    
    //#################Useful Functions###################
    //There's a ton of stuff that a creature might want to do that I wrote functions to make easier
    //You can have your actions/etc just call these instead of coding them directly
    
    //Set your DesiredMove to be moving towards a target
    //You can feed in a ThingController, ThingInfo, GameObject, or just Vector3 and it'll handle it
    public void MoveTowards(ThingInfo targ,float thresh=0)
    {
        if (targ == null) return;
        MoveTowards(targ.Thing,thresh);
    }
    public void MoveTowards(ThingController targ,float thresh=0)
    {
        if (targ == null) return;
        MoveTowards(targ.transform.position,thresh);
    }
    public void MoveTowards(GameObject targ,float thresh=0)
    {
        if (targ == null) return;
        MoveTowards(targ.transform.position,thresh);
    }
    public void MoveTowards(Vector3 targ,float thresh=0)
    {
        if (thresh > 0)
        {
            float d = Distance(targ);
            if (d < thresh)
            {
                
                if (d < thresh - 1)
                    Info.DesiredMove = transform.position - targ;
                else
                    Info.DesiredMove = Vector2.zero;
                return;
            }
        }
        Info.DesiredMove = targ - transform.position;
    }

    //Sets your DesiredMove to just be going in the direction you're facing
    public void MoveForwards()
    {
        Info.DesiredMove = Body.transform.right;
    }

    //Measures how far you are from another thing
    //Can be fed a ThingInfo, ThingController, GameObject, or Vector3
    public float Distance(ThingInfo targ)
    {
        if (targ == null) return 999;
        return Distance(targ.Thing);
    }
    public float Distance(ThingController targ)
    {
        if (targ == null) return 999;
        return Distance(targ.transform.position);
    }
    public float Distance(GameObject targ)
    {
        if (targ == null) return 999;
        return Distance(targ.transform.position);
    }
    public float Distance(Vector3 targ)
    {
        return Vector3.Distance(targ, transform.position);
    }

    //Rotates you towards looking at another thing.
    //If turnTime is set, the bigger it is the slower you turn--it's how long you would take to do a 180 in seconds
    //Can be fed a ThingInfo, ThingController, GameObject, or Vector3
    public float LookAt(ThingInfo targ,float turnTime=0)
    {
        if (targ == null) return 0;
        return LookAt(targ.Thing,turnTime);
    }
    public float LookAt(ThingController targ,float turnTime=0)
    {
        if (targ == null) return 0;
        return LookAt(targ.transform.position,turnTime);
    }
    public float LookAt(GameObject targ,float turnTime=0)
    {
        if (targ == null) return 0;
        return LookAt(targ.transform.position,turnTime);
    }
    public float LookAt(Vector3 targ,float turnTime=0)
    {
        Vector3 diff = targ - transform.position;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        float z = turnTime > 0 ? Mathf.MoveTowardsAngle(Body.transform.rotation.eulerAngles.z, rot_z, (180/turnTime) * Time.deltaTime) : rot_z;
        Body.transform.rotation = Quaternion.Euler(0,0,z);
        return Mathf.Abs(Mathf.DeltaAngle(z, rot_z));
    }
    
    //Sets your knockback, making you fly away from a specified point with a speed of 'amt'
    //Can be fed a ThingInfo, ThingController, GameObject, or Vector3
    public void TakeKnockback(ThingInfo from,float amt)
    {
        if (from?.Thing == null) return;
        TakeKnockback(from.Thing.transform.position,amt);
    }
    public void TakeKnockback(ThingController from,float amt)
    {
        if (from == null) return;
        TakeKnockback(from.transform.position,amt);
    }
    public void TakeKnockback(GameObject from,float amt)
    {
        if (from == null) return;
        TakeKnockback(from.transform.position,amt);
    }
    public void TakeKnockback(Vector3 from,float amt)
    {
        Vector2 dir = transform.position - from;
        Knockback = dir.normalized * amt;
    }

    //Returns true if you are facing the point specified, false otherwise. Thresh is how much leeway you have
    //Can be fed a ThingInfo, ThingController, GameObject, or Vector3
    public bool IsFacing(ThingInfo targ,float thresh=45)
    {
        if (targ == null) return false;
        return IsFacing(targ.Thing,thresh);
    }
    public bool IsFacing(ThingController targ,float thresh=45)
    {
        if (targ == null) return false;
        return IsFacing(targ.transform.position,thresh);
    }
    public bool IsFacing(GameObject targ,float thresh=45)
    {
        if (targ == null) return false;
        return IsFacing(targ.transform.position,thresh);
    }
    public bool IsFacing(Vector3 targ, float thresh=45)
    {
        Vector3 diff = targ - transform.position;
        float tdir = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        float rot = Body.transform.rotation.eulerAngles.z;
        return Mathf.Abs(Mathf.DeltaAngle(tdir, rot)) < thresh;
    }
    
    //Spawns a projectile (or any Option, technically) just in front of the character and set it as a child of them
    //Can feed it either an Option directly or a Spawn Request
    public ThingInfo Shoot(SpawnRequest sr)
    {
        ThingOption o = sr.FindThing();
        return Shoot(o);
    }
    public ThingInfo Shoot(ThingOption o)
    {
        ThingInfo i = o.Create();
        i.ChildOf = Info;
        i.Team = Info.Team;
        float rot = Body.Held.transform.rotation.eulerAngles.z - 90;
        i.Spawn(Body.Held.transform.position,rot);
        return i;
    }

    //#################Doing Physical Things###################
    //Most code gets run on ThingInfo, but code that's specifically related to physics/animations/etc goes here instead
    
    ///Plays an animation on both body and weapon and returns how long the longer of those animation goes.
    /// Speed sets how fast the animation plays out, and modifies the return time. 
    public float PlayAnim(string anim="",float speed=1)
    {
        float r = 0;
        r = Math.Max(r,Body.PlayAnim(anim,speed));
        if(Body.Held?.Anim != null) r = Math.Max(r,Body.Held.PlayAnim(anim,speed));
        return r;
    }

    //Sets the Thing to start a new action by sending a message to its ActorTrait.
    //Can be called as an ActionScript, string, or enum
    public void DoAction(ActionScript a, EventInfo e=null)
    {
        if(e == null) e = God.E();
        e.Type = EventTypes.StartAction;
        e.Set(a);
        TakeEvent(e);
    }
    public void DoAction(string a, EventInfo e=null)
    {
        DoAction(Enum.Parse<Actions>(a),e);
    }
    public void DoAction(Actions a=Actions.None, EventInfo e=null)
    {
        if(e == null) e = God.E();
        e.Type = EventTypes.StartAction;
        e.Set(ActionInfo.Action,a);
        TakeEvent(e);
    }
    
    ///Set the team of my bodies, and by extension all their hitboxes
    public void SetTeam(GameTeams team)
    {
        //Tell my body to update the team of all their hitboxes
        Body.SetTeam(team);
        //And if I have a weapon, tell them to do the same
        if(HeldBody != null) HeldBody.SetTeam(team);
    }

    ///This is basically OnCollisionEnter. Gets called by HitboxController when hitboxes touch each other
    ///GameCollision is basically Collision2D but automatically tells you the ThingInfos/Hitboxes of everyone involved
    public void TouchStart(GameCollision col)
    {
        TakeEvent(God.E(EventTypes.OnTouch).Set(col));
    }
    
    ///This is basically OnCollisionExit. Gets called by HitboxController when hitboxes untouch each other
    ///GameCollision is basically Collision2D but automatically tells you the ThingInfos/Hitboxes of everyone involved
    public void TouchEnd(GameCollision col)
    {
        TakeEvent(God.E(EventTypes.OnTouchEnd).Set(col));
    }

    ///Walls don't have the ThingController script, so we have a different collision script for them. Where is where we touched them (maybe)
    public void TouchWall(Vector2 where)
    {
        TakeEvent(God.E(EventTypes.OnTouchWall).Set(where));
    }
    
    ///Gives a list of all the Things we're currently touching. Filter can return only certain types of things
    public List<ThingController> GetTouching(HitboxTypes filter = HitboxTypes.None)
    {
        return Body.GetTouching(filter);
    }
    
    ///Called when an object enters a Vision Hitbox owned by the character
    public void SeeBegin(ThingInfo who)
    {
        if (CanSee.Contains(who)) return;
        CanSee.Add(who);
        TakeEvent(God.E(EventTypes.OnSee).Set(who));
    }
    
    ///Called when an object leaves a Vision Hitbox owned by the character
    public void SeeEnd(ThingInfo who)
    {
        if (!CanSee.Contains(who)) return;
        CanSee.Remove(who);
        TakeEvent(God.E(EventTypes.OnSeeEnd).Set(who));
    }

    ///Destroys the body of the item we're currently holding. The code for actually being dropped is elsewhere
    public void DropHeld()
    {
        if (HeldBody == null) return;
        Destroy(HeldBody.gameObject);
        HeldBody = null;
    }

    public void EnterRoom(RoomScript rm)
    {
        if (!CurrentRooms.Contains(rm))
            CurrentRooms.Add(rm);
        if (!rm.Contents.Contains(this))
        {
            rm.Contents.Add(this);
            rm.SendEvent(God.E(EventTypes.EnterRoom).Set(Info).Set(rm).Set(transform.position));
        }
    }
    
    public void ExitRoom(RoomScript rm)
    {
        CurrentRooms.Remove(rm);
        if (rm.Contents.Contains(this))
        {
            rm.Contents.Remove(this);
            rm.SendEvent(God.E(EventTypes.ExitRoom).Set(Info).Set(rm).Set(transform.position));
        }
    }
}
