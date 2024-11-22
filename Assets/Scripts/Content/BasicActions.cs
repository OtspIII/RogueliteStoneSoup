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
    public ChaseAction(ActorController who)
    {
        Type = Actions.Chase;
        Who = who;
        BeIdleAction();
    }

    public override void OnRun()
    {
        base.OnRun();
        Who.MoveTowards(Who.Target,Who.AttackRange);
        Who.LookAt(Who.Target,0.5f);
        
        if(Who.Distance(Who.Target) <= Who.AttackRange && Who.IsFacing(Who.Target,5))
            Who.DoAction(Who.DefaultAttackAction());
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

    public virtual float GetDamage()
    {
        return Who.CurrentWeapon.Damage * Damage;
    }

    public override void HitBegin(ActorController hit, HurtboxController box)
    {
        base.HitBegin(hit, box);
        if (AlreadyHit.Contains(hit)) return;
        AlreadyHit.Add(hit);
        hit.TakeDamage(GetDamage());
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
    
    public LungeAction(ActorController who, float pow=10)
    {
        Type = Actions.Lunge;
        Who = who;
        Anim = "Lunge";
        MoveMult = 0;
        Power = pow;
        Knockback = 0;
    }

    public override void OnRun()
    {
        base.OnRun();
        if (Phase == 0)
        {
            MoveMult = Who.AttackRange;
            Who.MoveForwards();
        }
        else
            MoveMult = 0;
    }
}

public class PatrolAction : ActionScript
{
    public Vector3 Target;
    
    public PatrolAction(ActorController who)
    {
        Type = Actions.Patrol;
        Who = who;
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
        if (Who.Target != null && Vector2.Distance(Who.Target.transform.position,
                Who.transform.position) < Who.VisionRange)
        {
            Vector2 dir = Who.Target.transform.position - Who.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(Who.transform.position,
                dir, Who.VisionRange, LayerMask.GetMask("Wall"));
            if (hit.collider == null)
            {
                Who.DoAction(Actions.Chase);
                return;
            }
        }
        if (Vector2.Distance(Target, Who.transform.position) < 0.2f)
        {
            NewTarget();
        }
        float turn = Who.LookAt(Target,0.5f);
        if (turn < 5)
        {
            RaycastHit2D hit = Physics2D.Raycast(Who.transform.position, Who.transform.right,
                1, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                NewTarget();
                return;
            }
            Who.MoveTowards(Target,0);
        }
        
    }

    void NewTarget()
    {
        Target = Who.StartSpot + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        Who.DebugTxt = Target.ToString();
    }

}