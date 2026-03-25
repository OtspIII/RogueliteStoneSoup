using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAction_TracyH : AttackAction
{
    public ChargeAction_TracyH() { }

    public ChargeAction_TracyH(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Charge_TracyH, who);
        Anim = "Charge";
        MoveMult = 0;
        Knockback = 10;
        Duration = 0.4f;
        CanRotate = true;
    }

    public override void Begin()
    {
        base.Begin();

        if (Who.Target != null)
            Who.Thing.LookAt(Who.Target, 1f);
    }

    public override void OnRun()
    {
        base.OnRun();

        if (Who.Target != null)
            Who.Thing.LookAt(Who.Target, 1f);

        if (Phase == 0)
        {
            MoveMult = Who.AttackRange;
            Who.Thing.MoveForwards();
        }
        else
            MoveMult = 0;
    }

    public override void HitBegin(GameCollision col)
    {
        if (Who.Thing == null) return;
        if (God.OneOf(col.HBMe.Type, HitboxTypes.Body, HitboxTypes.Friendly)) return;

        ThingController hit = col.Other;
        if (hit == null) return;
        if (AlreadyHit.Contains(hit)) return;

        AlreadyHit.Add(hit);

        EventInfo hpEvent = God.E(EventTypes.ShownHP);
        hit.TakeEvent(hpEvent);
        float hp = hpEvent.GetFloat();

        float halfDamage = hp * 0.5f;

        hit.TakeEvent(
            God.E(EventTypes.Damage)
                .Set(NumInfo.Default, halfDamage)
                .Set(Who)
        );

        hit.DoAction(Actions.Stun, God.E().Set(0.5f).Set(NumInfo.Priority, 3));
        hit.TakeKnockback(Who.Thing.transform.position, Knockback);
    }
}