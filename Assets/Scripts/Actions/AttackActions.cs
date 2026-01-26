using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : UseAction
{
    public float Damage = 1;
    public float Knockback = 10;
    public List<ThingController> AlreadyHit = new List<ThingController>();

    public AttackAction(){ }
    
    public AttackAction(ThingController who,EventInfo e=null){}

    public virtual float GetDamage()
    {
        return Who.CurrentWeapon.Ask(EventTypes.GetDamage).GetFloat() * Damage;
    }

    public override void HitBegin(GameCollision col)
    {
        base.HitBegin(col);
        if (God.OneOf(col.HBMe.Type, HitboxTypes.Body, HitboxTypes.Friendly)) return;
        ThingController hit = col.Other;
        if (AlreadyHit.Contains(hit)) return;
        AlreadyHit.Add(hit);
        // Debug.Log("TAKE DAMAGE: " + hit.gameObject);
        hit.TakeEvent(new EventInfo(EventTypes.Damage).Set(NumInfo.Amount,GetDamage()).Set(Who?.Info));
        // hit.TakeDamage(GetDamage());
        hit.DoAction(Actions.Stun,God.E().Set(10.5f).Set(NumInfo.Priority,3));
        hit.TakeKnockback(Who.transform.position,Knockback);
    }
}

public class SwingAction : AttackAction
{
    public SwingAction(ThingController who,EventInfo e=null)
    {
        Setup(Actions.Swing,who);
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


public class ShootAction : AttackAction
{
    public ShootAction(ThingController who,EventInfo e=null)
    {
        Setup(Actions.Shoot,who);
        Duration = 0.2f;
        CanRotate = true;
        MoveMult = 0.5f;
        Anim = "Shoot";
    }

    public override void Begin()
    {
        base.Begin();
        ThingOption proj = GetWeapon().Ask(EventTypes.GetProjectile).GetOption();
        Who.Shoot(proj);
    }

    public override void OnRun()
    {
        base.OnRun();
        if (Who.Target != null)
        {
            Who.LookAt(Who.Target, 0.5f);
            Who.MoveTowards(Who.Target, Who.AttackRange);
        }
    }

}



public class LungeAction : AttackAction
{
    
    public LungeAction(ThingController who,EventInfo e=null)
    {
        Setup(Actions.Lunge,who);
        Anim = "Lunge";
        MoveMult = 0;
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