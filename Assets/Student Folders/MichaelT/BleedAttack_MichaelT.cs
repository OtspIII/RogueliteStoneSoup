using UnityEngine;
using System.Collections; 
using System.Collections.Generic;

public class BleedAttackAction_MichaelT : AttackAction
{
    public BleedAttackAction_MichaelT(ThingInfo who, EventInfo e = null) : base(who.Thing, e)
    {
        Setup(Actions.BleedAttack_MichaelT, who); 
       
        Anim = "Swing";
        MoveMult = 0.5f;
        Knockback = 10f;
    }
    public override void HitBegin(GameCollision col)
    {
        ThingController hit = col.Other; 

        base.HitBegin(col);

        if (AlreadyHit.Contains(hit))
        {
            hit.TakeEvent(God.E(EventTypes.GainTrait).Set(Traits.Bleed_MichaelT));
        }
    }
}
