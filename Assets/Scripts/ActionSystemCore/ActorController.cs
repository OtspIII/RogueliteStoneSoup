using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ActorController : MonoBehaviour
{
    public ActionScript CurrentAction;
    public Actions DefaultAction;
    public Vector2 DesiredMove;
    public Vector2 Knockback;
    [HideInInspector]
    public Rigidbody2D RB;
    public BodyController Body;
    public string DefaultAnim = "Idle";
    public float HP;
    public Vector3 StartSpot;
    public string DebugTxt;
    public CharacterStats Stats;
    public bool IsPlayer = false;
    
    public ActorController Target;
    public float AttackRange = 1.5f;
    public float VisionRange = 4;

    public void Awake()
    {
        StartSpot = transform.position;
        OnAwake();
    }

    public virtual void OnAwake()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    
    public void Start()
    {
        OnStart();
    }

    public virtual void OnStart()
    {
        if (!IsPlayer && Target == null) Target = God.Player;
        HP = Stats.HP;
        DoAction();
    }

    public void Update()
    {
        OnUpdate();
        if(IsPlayer) PlayerInputs();
        CurrentAction?.Run();
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

    public virtual void OnUpdate()
    {
        
    }

    public virtual void Imprint(CharacterStats stats,bool player=false)
    {
        Stats = stats;
        gameObject.name = stats.Name;
        // Speed = stats.Speed;
        HP = stats.HP;
        // MaxHP = stats.HP;
        if (player)
        {
            IsPlayer = true;
            God.Player = this;
            DefaultAction = Actions.Idle;
        }
        else
        {
            DefaultAction = Actions.Patrol;
        }

        Body = Instantiate(God.Library.BodyPrefab, transform);
        Body.Setup(this);
        Body.Anim.Rebind();
        // DefaultAction = Enum.Parse<Actions>(json.DefaultAction);
    }

    public virtual void DoAction(ActionScript a, Infos i=null)
    {
        float prio = i != null ? i.Get(FloatI.Priority, 1) : 1;
        if (CurrentAction != null && CurrentAction.Priority >= prio) return;
        if (a == null) Debug.Log("ERROR: NULL ACTION / " + this);
        CurrentAction = a;
        if (CurrentAction != null)
        {
            CurrentAction.Begin();
        }
    }
    
    public virtual void DoAction(Actions a=Actions.None, Infos i=null)
    {
        ActionScript act = ActionParser.GetAction(a == Actions.None ? DefaultAction : a,this);
        DoAction(act,i);
    }

    public void MoveTowards(ActorController targ,float thresh=0)
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

    public void MoveForwards()
    {
        DesiredMove = transform.right;
    }

    public float Distance(ActorController targ)
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

    public float LookAt(ActorController targ,float turnTime=0)
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

    public void SetPhase(int n)
    {
        if (CurrentAction == null) return;
        CurrentAction.ChangePhase(n); //the village vet
    }

    public virtual void TakeDamage(float amt)
    {
        if (Stats.HP <= 0) return;
        HP -= amt;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public virtual void TakeKnockback(Vector3 from,float amt)
    {
        Vector2 dir = transform.position - from;
        Knockback = dir.normalized * amt;
    }

    public virtual ActionScript DefaultAttackAction()
    {
        return GetAction(Actions.Swing);
    }

    public virtual ActionScript GetAction(Actions a)
    {
        return ActionParser.GetAction(a,this);
    }

    public bool IsFacing(ActorController targ,float thresh=45)
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
    
    public void PlayerInputs()
    {
        Vector2 vel = Vector2.zero;
        if (Input.GetKey(KeyCode.D)) vel.x = 1;
        if (Input.GetKey(KeyCode.A)) vel.x = -1;
        if (Input.GetKey(KeyCode.W)) vel.y = 1;
        if (Input.GetKey(KeyCode.S)) vel.y = -1;
        DesiredMove = vel;
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
            DoAction(Actions.Swing);
        
        if(CurrentAction.CanRotate) LookAt(God.Cam.Cam.ScreenToWorldPoint(Input.mousePosition),0.1f);
    }
}
