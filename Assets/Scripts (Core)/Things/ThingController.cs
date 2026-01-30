using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThingController : MonoBehaviour
{
    //If you ask me my name, I'll give you the name from my info
    public string Name {get {return Info.Name;} set { Info.Name = value;}}
    [HideInInspector] public Rigidbody2D RB; //I spawn my own RB, so hide this from the inspector
    public BodyController Body;   //My body, which I spawn as a separate prefab
    public BodyController WeaponBody; //My currently equipped item's body
    public SpriteRenderer Icon;   //I have a little sprite that can be used for context action icons. 'Hit E to pick me up' type stuff
    public TextMeshPro NameText;  //I have a textmesh that displays my name. You can see it by holding shift.
    public Collider2D NoClip;     //I have a tiny collider that only collides with walls. To make sure I don't escape the stage.
    public ThingInfo Info;        //My Info. The core of who I am. Most of the code that controls me lives here.
    public ThingInfo CurrentWeapon {get {return Info.CurrentWeapon;} set { Info.CurrentWeapon = value;}}  //My currently equipped item's info
    [Header("Movement Bookkeeping")]
    public Vector2 ActualMove;
    public Vector2 Knockback;
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
        //Tell my Traits that we're started
        TakeEvent(EventTypes.Start);
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
            Debug.LogError("Thing Got Stuck Mid-Action: " + Name);
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


    public ThingInfo Shoot(ThingOption o)
    {
        ThingInfo i = o.Create();
        i.ChildOf = Info;
        i.Team = Info.Team;
        float rot = Body.Weapon.transform.rotation.eulerAngles.z - 90;
        i.Spawn(Body.Weapon.transform.position,rot);
        return i;
    }

    public void MoveForwards()
    {
        Info.DesiredMove = Body.transform.right;
    }

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
    
    public void TakeKnockback(Vector3 from,float amt)
    {
        Vector2 dir = transform.position - from; //Should this be an event??
        Knockback = dir.normalized * amt;
    }

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

    public float PlayAnim(string anim="",float speed=1)
    {
        float r = 0;
        r = Math.Max(r,Body.PlayAnim(anim,speed));
        // Debug.Log("PLAY ANIM: " + anim + " / " + this);
        if(Body.Weapon?.Anim != null) r = Math.Max(r,Body.Weapon.PlayAnim(anim,speed));
        return r;
    }
    
    public void SetPhase(int n)
    {
        TakeEvent(God.E(EventTypes.SetPhase).Set(n));
    }
    
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
    
    public void SetTeam(GameTeams team)
    {
        Body.SetTeam(team);
        if(WeaponBody != null) WeaponBody.SetTeam(team);
    }

    public void TouchStart(GameCollision col)
    {
        TakeEvent(God.E(EventTypes.OnTouch).Set(col));
    }
    
    public void TouchEnd(GameCollision col)
    {
        TakeEvent(God.E(EventTypes.OnTouchEnd).Set(col));
    }

    public void TouchWall(Vector2 where)
    {
        TakeEvent(God.E(EventTypes.OnTouchWall).Set(where));
    }
    
    public List<ThingController> GetTouching(HitboxTypes filter = HitboxTypes.None)
    {
        return Body.GetTouching(filter);
    }

    public void DropHeld()
    {
        if (WeaponBody == null) return;
        Destroy(WeaponBody.gameObject);
        WeaponBody = null;
    }

    public ThingInfo GetOwner(bool selfOk = true, bool ultimate = true)
    {
        return Info.GetOwner(selfOk,ultimate);
    }
}
