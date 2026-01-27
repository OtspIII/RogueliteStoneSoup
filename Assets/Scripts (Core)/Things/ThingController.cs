using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThingController : MonoBehaviour
{
    public string Name {get {return Info.Name;} set { Info.Name = value;}}
    [HideInInspector]
    public Rigidbody2D RB;
    public BodyController Body;
    public SpriteRenderer Icon;
    public TextMeshPro NameText;
    public Collider2D NoClip;
    
    public Vector3 StartSpot;
    public string DebugTxt;

    public ThingInfo Info;
    
    // public CharacterStats Stats;
    public ThingInfo CurrentWeapon {get {return Info.CurrentWeapon;} set { Info.CurrentWeapon = value;}}
    public BodyController WeaponBody;
    
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
        if(God.GM != null && !God.GM.Things.Contains(this)) God.GM.Things.Add(this);
        NameText.gameObject.SetActive(Input.GetKey(God.InfoKey));
        TakeEvent(EventTypes.Start);
    }

    public void Update()
    {
        if (Input.GetKeyDown(God.InfoKey)) NameText.gameObject.SetActive(true);
        if (Input.GetKeyUp(God.InfoKey)) NameText.gameObject.SetActive(false);
        if (Info.MidEvent)
        {
            Debug.Log("Thing Got Stuck Mid-Action: " + Name);
            Info.MidEvent = false;
        }
        TakeEvent(EventTypes.Update);
        
    }
    
    private void FixedUpdate()
    {
        if (Info.Knockback != Vector2.zero)
        {
            Info.Knockback *= 0.9f;
            if (Info.Knockback.magnitude < 0.1)
                Info.Knockback = Vector2.zero;
        }
    }
    
    void OnDestroy()
    {
        if(God.GM != null) God.GM.Things.Remove(this);
    }

    public void AddRB()
    {
        if (RB != null) return;
        RB = gameObject.AddComponent<Rigidbody2D>();
        RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        RB.gravityScale = 0;
    }
    
    public void TakeEvent(EventTypes e)
    {
        TakeEvent(new EventInfo(e));
    }

    public EventInfo Ask(EventTypes e, bool wpn = false)
    {
        return Info.Ask(e,wpn);
    }
    public EventInfo Ask(EventInfo e, bool wpn = false)
    {
        return Info.Ask(e,wpn);
    }
    
    public void TakeEvent(EventInfo e,bool instant=false,int safety=999)
    {
        // Debug.Log("TAKE EVENT A: " + e.Type);
        Info.TakeEvent(e,instant,safety);
    }
    
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
    
    public virtual void TakeKnockback(Vector3 from,float amt)
    {
        Vector2 dir = transform.position - from;
        Info.Knockback = dir.normalized * amt;
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

    public virtual float PlayAnim(string anim="",float speed=1)
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
    
    public virtual void DoAction(ActionScript a, EventInfo e=null)
    {
        if(e == null) e = God.E();
        e.Type = EventTypes.StartAction;
        e.Set(a);
        TakeEvent(e);
    }
    
    public virtual void DoAction(string a, EventInfo e=null)
    {
        DoAction(Enum.Parse<Actions>(a),e);
    }
    
    public virtual void DoAction(Actions a=Actions.None, EventInfo e=null)
    {
        if(e == null) e = God.E();
        e.Type = EventTypes.StartAction;
        e.Set(ActionInfo.Action,a);
        TakeEvent(e);
    }
    
    public virtual ActionScript GetAction(Actions a)
    {
        return Parser.Get(a,Info);
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
