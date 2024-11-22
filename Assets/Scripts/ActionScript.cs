using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScript
{
    public ActorController Who;
    public float MoveMult = 0;
    public bool HaltMomentum = true;
    public Coroutine Coro;
    public float Priority = 1;
    public string Anim="";
    public float Duration;
    public float Timer;
    public bool CanRotate = false;
    public int Phase;

    protected void BeIdleAction()
    {
        Priority = 0;
        MoveMult = 1;
        CanRotate = true;
    }
    
    public void Run()
    {
        OnRun();
        HandleMove();
        if (Duration > 0)
        {
            Timer+=Time.deltaTime;
            if(Timer >= Duration)
                End();
        }
    }

    public virtual void OnRun()
    {
        
    }

    public virtual void Reset()
    {
        Timer = 0;
    }
    
    public virtual void HandleMove()
    {
        if (Who.RB == null) return;
        Who.RB.velocity = (MoveMult * Who.Speed * Who.DesiredMove.normalized) + Who.Knockback;
    }

    public virtual void Begin()
    {
        Reset();
        if(HaltMomentum && Who.RB != null) Who.RB.velocity = Vector2.zero;
        Coro = Who.StartCoroutine(Script());
        if (Anim != "")
        {
            Who.Anim.Play(Anim);
            foreach(AnimationClip c in Who.Anim.runtimeAnimatorController.animationClips)
                if (c.name == Anim)
                    Duration = c.length * Who.Anim.speed;
            
        }
        else
            Who.Anim.Play(Who.DefaultAnim);
        ChangePhase(0);
    }
    
    public virtual void End()
    {
        Who.CurrentAction = null;
        Who.DoAction(NextAction());
        if (Coro != null)
        {
            Who.StopCoroutine(Coro);
            Coro = null;
        }
    }

    public virtual IEnumerator Script()
    {
        yield return null;
    }

    public virtual ActionScript NextAction()
    {
        return Who.DefaultAction;
    }

    public virtual void ChangePhase(int newPhase)
    {
        Phase = newPhase;
    }

    public virtual void HitBegin(ActorController hit, HurtboxController box)
    {
        
    }
    
    public virtual void HitEnd(ActorController hit, HurtboxController box)
    {
        
    }

    public float Perc()
    {
        if (Duration <= 0) return 0;
        return Timer / Duration;
    }
}

public class IdleAction : ActionScript
{
    public IdleAction(ActorController who)
    {
        Who = who;
        BeIdleAction();
    }
}

public class SpinAction : ActionScript
{   
    public SpinAction(ActorController who, float dur=1)
    {
        Who = who;
        Duration = dur;
    }

    public override IEnumerator Script()
    {
        float speed = 360 / Duration;
        float rot = 0;
        while (rot < 360)
        {
            rot += speed * Time.deltaTime;
            Who.transform.rotation = Quaternion.Euler(0,0,rot);
            yield return null;
        }
        End();
    }

    public override void End()
    {
        base.End();
        Who.transform.rotation = Quaternion.Euler(0,0,0);
    }
}


public class ChaseAction : ActionScript
{
    public MonsterController Mon;
    
    public ChaseAction(ActorController who)
    {
        Who = who;
        Mon = (MonsterController)Who;
        BeIdleAction();
    }

    public override void OnRun()
    {
        base.OnRun();
        Mon.MoveTowards(Mon.Target,Mon.AttackRange);
        Mon.LookAt(Mon.Target,0.5f);
        
        if(Mon.Distance(Mon.Target) <= Mon.AttackRange && Mon.IsFacing(Mon.Target,5))
            Mon.DoAction(Mon.DefaultAttackAction());
    }

}

public class AttackAction : ActionScript
{
    public float Damage = 1;
    public float Knockback = 10;
    public List<ActorController> AlreadyHit = new List<ActorController>();

    public AttackAction(){ }
    
    public AttackAction(ActorController who, string anim)
    {
        Who = who;
        Anim = anim;
    }

    public override void HitBegin(ActorController hit, HurtboxController box)
    {
        base.HitBegin(hit, box);
        if (AlreadyHit.Contains(hit)) return;
        AlreadyHit.Add(hit);
        hit.TakeDamage(Damage);
        hit.DoAction(new SpinAction(hit,0.5f),3);
        hit.TakeKnockback(Who.transform.position,Knockback);
    }
}

public class SwingAction : AttackAction
{
    public SwingAction(ActorController who)
    {
        Who = who;
        Anim = "Swing";
    }

    public override void OnRun()
    {
        base.OnRun();
        if (Phase < 1)
        {
            Who.MoveForwards();
        }
    }

    public override void ChangePhase(int newPhase)
    {
        base.ChangePhase(newPhase);
        if (Phase == 0)
        {
            MoveMult = 1;
        }
        else
            MoveMult = 0;
    }
}


public class LungeAction : AttackAction
{
    public float Power;
    public MonsterController Mon;
    
    public LungeAction(ActorController who, float pow=10)
    {
        Who = who;
        Anim = "Lunge";
        MoveMult = 0;
        Power = pow;
        Knockback = 0;
        Mon = (MonsterController)Who;
    }

    public override void OnRun()
    {
        base.OnRun();
        if (Phase == 0)
        {
            MoveMult = Mon.AttackRange;
            Who.MoveForwards();
        }
        else
            MoveMult = 0;
    }
}

public class PatrolAction : ActionScript
{
    public MonsterController Mon;
    public Vector3 Target;
    
    public PatrolAction(ActorController who)
    {
        Who = who;
        Mon = (MonsterController)Who;
        BeIdleAction();
    }

    public override void Begin()
    {
        base.Begin();
        NewTarget();
    }

    public override void OnRun()
    {
        base.OnRun();
        if (Mon.Target != null && Vector2.Distance(Mon.Target.transform.position,
                Who.transform.position) < Mon.VisionRange)
        {
            Vector2 dir = Mon.Target.transform.position - Mon.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(Mon.transform.position,
                dir, Mon.VisionRange, LayerMask.GetMask("Wall"));
            if (hit.collider == null)
            {
                Mon.DoAction(new ChaseAction(Mon));
                return;
            }
        }
        if (Vector2.Distance(Target, Who.transform.position) < 0.2f)
        {
            NewTarget();
        }
        float turn = Mon.LookAt(Target,0.5f);
        if (turn < 5)
        {
            RaycastHit2D hit = Physics2D.Raycast(Mon.transform.position, Mon.transform.right,
                1, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                NewTarget();
                return;
            }
            Mon.MoveTowards(Target,0);
        }
        
    }

    void NewTarget()
    {
        Target = Who.StartSpot + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        Who.Debug = Target.ToString();
    }

}