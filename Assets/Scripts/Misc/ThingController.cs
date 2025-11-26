using System;
using System.Collections.Generic;
using UnityEngine;

public class ThingController : MonoBehaviour
{
    public string Name;
    [HideInInspector]
    public Rigidbody2D RB;
    public BodyController Body;
    public string DefaultAnim = "Idle";
    
    public Vector3 StartSpot;
    public string DebugTxt;
    // public CharacterStats Stats;
    public TraitInfo CurrentWeapon;

    public TraitInfo ActorTrait;
    public Dictionary<Traits, TraitInfo> Trait = new Dictionary<Traits, TraitInfo>();
    public Dictionary<EventTypes, List<Traits>> PreListen = new Dictionary<EventTypes, List<Traits>>();
    public Dictionary<EventTypes, List<Traits>> TakeListen = new Dictionary<EventTypes, List<Traits>>();
    public List<EventInfo> EventQueue = new List<EventInfo>();
    public bool MidEvent = false;
    
    //May be moved to traits
    public ThingController Target;
    public float AttackRange = 1.5f;
    public float VisionRange = 4;
    public float CurrentSpeed; //Set by traits so you don't need to recalc constantly
    public Vector2 DesiredMove;
    public Vector2 Knockback;
    
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
        OnStart();
    }

    public virtual void OnStart()
    {
    }

    public void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate()
    {
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
    
    public void SetWeapon(string w)
    {
        ThingSeed weap = ThingBuilder.Things[w];
        CurrentWeapon = new TraitInfo(Traits.Weapon,null,weap.Traits[Traits.Weapon]);//God.Library.GetWeapon(stats.Weapon);
        CurrentWeapon.Seed = weap;
    }
    
    public TraitInfo AddTrait(Traits t,EventInfo i=null)
    {
        TraitInfo r = Get(t);
        if (r != null)
        {
            r.ReUp(i);
        }
        else
        {
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

    public void AddListen(EventTypes e, Traits t,bool pre = false)
    {
        Dictionary<EventTypes, List<Traits>> d = pre ? PreListen : TakeListen;
        if(!d.ContainsKey(e)) d.Add(e,new List<Traits>());
        if(!d[e].Contains(t)) d[e].Add(t);
    }

    public void TakeEvent(EventTypes e)
    {
        TakeEvent(new EventInfo(e));
    }

    public EventInfo Ask(EventTypes e)
    {
        EventInfo r = God.E(e);
        TakeEvent(r,true);
        return r;
    }
    
    public void TakeEvent(EventInfo e,bool instant=false,int safety=999)
    {
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
        if(take != null) foreach (Traits t in take) Get(t).TakeEvent(e);
        MidEvent = false;
        if (EventQueue.Count > 0)
        {
            EventInfo next = EventQueue[0];
            EventQueue.RemoveAt(0);
            TakeEvent(next,false,safety);
        }
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

    public ProjectileController Shoot(string p)
    {
        ProjStats stat = God.Library.GetProjectile(p);
        return Shoot(stat);
    }

    public ProjectileController Shoot(ProjStats stat)
    {
        ProjectileController pref = God.Library.GetProjectilePrefab();
        Vector3 rot = Body.Weapon.transform.rotation.eulerAngles;
        rot.z -= 90; //Eventually add accuracy stat?
        ProjectileController r = Instantiate(pref, Body.Weapon.transform.position, Quaternion.Euler(rot));
        r.Setup(this,stat);
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

    public virtual void PlayAnim(string anim)
    {
        Body.Anim.Play(anim);
        // if(Body.Weapon?.Anim != null) Body.Weapon.Anim.Play(anim);
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
        return GetAction(Body.Weapon.DefaultAttack);
    }

    public virtual ActionScript GetAction(Actions a)
    {
        return ActionParser.GetAction(a,this);
    }
    
}
