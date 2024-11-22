using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : ActionScript
{
    public IdleAction(ActorController who)
    {
        Type = Actions.Idle;
        Who = who;
        BeIdleAction();
    }
}

public class StunAction : ActionScript
{   
    public StunAction(ActorController who, float dur=1)
    {
        Type = Actions.Stun;
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
        Type = Actions.Chase;
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
        hit.DoAction(Actions.Stun,new Infos().Add(FloatI.Amt,0.5f).Add(FloatI.Priority,3));
        hit.TakeKnockback(Who.transform.position,Knockback);
    }
}

public class SwingAction : AttackAction
{
    public SwingAction(ActorController who)
    {
        Type = Actions.Swing;
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
        Type = Actions.Lunge;
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
        Type = Actions.Patrol;
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
                Mon.DoAction(Actions.Chase);
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
        Who.DebugTxt = Target.ToString();
    }

}