using System;
using System.Collections.Generic;
using UnityEngine;

public class ThingController : MonoBehaviour
{
    public string Name {get {return Info.Name;} set { Info.Name = value;}}
    [HideInInspector]
    public Rigidbody2D RB;
    public BodyController Body;
    public SpriteRenderer Icon;
    public Collider2D NoClip;
    
    public Vector3 StartSpot;
    public string DebugTxt;

    public ThingInfo Info;
    
    // public CharacterStats Stats;
    public TraitInfo CurrentWeapon {get {return Info.CurrentWeapon;} set { Info.CurrentWeapon = value;}}
    public BodyController WeaponBody;
    
    // public Dictionary<Traits, TraitInfo> Trait {get {return Info.Trait;} set { Info.Trait = value;}}
    // public Dictionary<EventTypes, List<Traits>> PreListen {get {return Info.PreListen;} set { Info.PreListen = value;}}
    // public Dictionary<EventTypes, List<Traits>> TakeListen {get {return Info.TakeListen;} set { Info.TakeListen = value;}}
    // public List<EventInfo> EventQueue {get {return Info.EventQueue;} set { Info.EventQueue = value;}}
    // public bool MidEvent {get {return Info.MidEvent;} set { Info.MidEvent = value;}}
    
    //May be moved to traits
    public ThingController Target {get {return Info.Target != null ? Info.Target.Thing : null;} set { Info.Target = (value == null ? null : value.Info);}}
    public TraitInfo ActorTrait {get {return Info.ActorTrait;} set { Info.ActorTrait = value;}}
    public float AttackRange {get {return Info.AttackRange;} set { Info.AttackRange = value;}}
    public float VisionRange  {get {return Info.VisionRange;} set { Info.VisionRange = value;}}
    public float CurrentSpeed {get {return Info.CurrentSpeed;} set { Info.CurrentSpeed = value;}}
    public Vector2 DesiredMove {get {return Info.DesiredMove;} set { Info.DesiredMove = value;}}
    public Vector2 Knockback {get {return Info.Knockback;} set { Info.Knockback = value;}}

    
    public void Awake()
    {
        OnAwake();
    }

    public virtual void OnAwake()
    {
        StartSpot = transform.position;
        RB = GetComponent<Rigidbody2D>();
    }
    
    public void Start()
    {
        TakeEvent(EventTypes.Start);
    }

    public virtual void OnStart()
    {
    }

    public void Update()
    {
        if (Info.MidEvent)
        {
            Debug.Log("Thing Got Stuck Mid-Action: " + Name);
            Info.MidEvent = false;
        }
        TakeEvent(EventTypes.Update);
        
    }

    
    private void FixedUpdate()
    {
        if (Knockback != Vector2.zero)
        {
            Knockback *= 0.9f;
            if (Knockback.magnitude < 0.1)
                Knockback = Vector2.zero;
        }
    }

    public void AddRB()
    {
        if (RB != null) return;
        RB = gameObject.AddComponent<Rigidbody2D>();
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        RB.gravityScale = 0;
    }
    
    
    // public TraitInfo AddTrait(Traits t,EventInfo i=null)
    // {
    //     return Info.AddTrait(t, i);
    // }
    //
    // public TraitInfo Get(Traits t)
    // {
    //     return Info.Get(t);
    // }

    

    public void TakeEvent(EventTypes e)
    {
        TakeEvent(new EventInfo(e));
    }

    public EventInfo Ask(EventTypes e)
    {
        return Info.Ask(e);
    }
    
    public void TakeEvent(EventInfo e,bool instant=false,int safety=999)
    {
        // Debug.Log("TAKE EVENT A: " + e.Type);
        Info.TakeEvent(e,instant,safety);
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
                    DesiredMove = transform.position - targ;
                else
                    DesiredMove = Vector2.zero;
                return;
            }
        }
        
        DesiredMove = targ - transform.position;
    }

    public ThingController Shoot(GameTags t =GameTags.Projectile)
    {
        ThingOption stat = God.Library.GetThing(t);
        return Shoot(stat);
    }

    public ThingController Shoot(ThingOption o)
    {
        ThingInfo i = o.Create();
        i.ChildOf = Info;
        i.Team = Info.Team;
        float rot = Body.Weapon.transform.rotation.eulerAngles.z - 90;
        ThingController r = i.Spawn(Body.Weapon.transform.position,rot);
        // rot.z -= 90; //Eventually add accuracy stat?
        // ProjectileController r = Instantiate(pref, Body.Weapon.transform.position, Quaternion.Euler(rot));
        // r.Setup(this,stat);
        return r;
    }

    public void MoveForwards()
    {
        DesiredMove = transform.right;
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
        float z = turnTime > 0 ? Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, rot_z, (180/turnTime) * Time.deltaTime) : rot_z;
        transform.rotation = Quaternion.Euler(0,0,z);
        return Mathf.Abs(Mathf.DeltaAngle(z, rot_z));
    }
    
    public virtual void TakeKnockback(Vector3 from,float amt)
    {
        Vector2 dir = transform.position - from;
        Knockback = dir.normalized * amt;
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
        float rot = transform.rotation.eulerAngles.z;
        return Mathf.Abs(Mathf.DeltaAngle(tdir, rot)) < thresh;
    }

    public virtual float PlayAnim(string anim="")
    {
        float r = 0;
        r = Math.Max(r,Body.PlayAnim(anim));
        // Debug.Log("PLAY ANIM: " + anim + " / " + this);
        if(Body.Weapon?.Anim != null) r = Math.Max(r,Body.Weapon.PlayAnim(anim));
        return r;
    }
    
    public void SetPhase(int n)
    {
        TakeEvent(God.E(EventTypes.SetPhase).Set(n));
    }
    
    public virtual void DoAction(ActionScript a, Infos i=null)
    {
        TakeEvent(God.E(EventTypes.StartAction).Set(a));
    }
    
    public virtual void DoAction(string a, Infos i=null)
    {
        DoAction(Enum.Parse<Actions>(a),i);
    }
    
    public virtual void DoAction(Actions a=Actions.None, Infos i=null)
    {
        TakeEvent(God.E(EventTypes.StartAction).Set(EnumInfo.Action,(int)a));
    }
    
    public virtual ActionScript DefaultAttackAction()
    {
        Actions act = CurrentWeapon.Get<Actions>(EnumInfo.DefaultAction);
        return GetAction(act);
    }

    public virtual ActionScript GetAction(Actions a)
    {
        return ActionParser.GetAction(a,this);
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     HitboxController hb = other.gameObject.GetComponent<HitboxController>();
    //     if (hb != null && hb.Who != null)
    //     {
    //         // Debug.Log("COLL: " + hb.Who);
    //         TakeEvent(God.E(EventTypes.OnTouch).Set(hb.Who));
    //         if(!Touching.Contains(hb.Who)) Touching.Add(hb.Who);
    //         //Only one because they'll call their own version
    //     }
    //     else
    //     {
    //         TakeEvent(God.E(EventTypes.OnTouchWall).Set(transform.position));
    //     }
    // }
    //
    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     HitboxController hb = other.gameObject.GetComponent<HitboxController>();
    //     if (hb != null)
    //     {
    //         Touching.Remove(hb.Who);
    //     }
    // }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     ThingController tc = other.gameObject.GetComponent<ThingController>();
    //     if (tc != null)
    //     {
    //         TakeEvent(God.E(EventTypes.OnTouch).Set(tc));
    //         if(!Touching.Contains(tc)) Touching.Add(tc);
    //         //Only one because they'll call their own version
    //     }
    //     else
    //     {
    //         TakeEvent(God.E(EventTypes.OnTouchWall).Set(other.GetContact(0).point));
    //     }
    // }
    //
    // private void OnCollisionExit2D(Collision2D other)
    // {
    //     ThingController tc = other.gameObject.GetComponent<ThingController>();
    //     if (tc != null)
    //     {
    //         Touching.Remove(tc);
    //     }
    // }

    public bool IsPlayer()
    {
        return Info.IsPlayer();
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
}
