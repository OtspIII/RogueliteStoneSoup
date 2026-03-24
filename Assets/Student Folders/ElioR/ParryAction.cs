using UnityEngine;

public class ParryAction: ActionScript  
{
    public float knockback = 5;
    
    public ParryAction(ThingInfo who, EventInfo e)
    {
        Setup(Actions.ParryAction_ElioR,who);
        MoveMult = 0;
        Priority = 1;
        Duration = 2;
        
    }

    public override void Begin()
    {
        Debug.Log("PA BEGIN");
        Who.AddTrait(Traits.Barrier);
        // Who.TakeEvent(God.E(EventTypes.GainTrait).Set(Traits.Barrier));
    }
    public override void HitBegin(GameCollision collision)
    {
        //Debug.Log("Bruh");
        ThingController Hit = collision.Other;
        Hit.TakeKnockback(Who.Thing.transform.position, knockback);
        Who.Thing.TakeKnockback(Hit, knockback);
        End();
    }

    
    public override void End()
    {
        Debug.Log("PA END");
        base.End();
        Who.RemoveTrait(Traits.Barrier);
        // Who.TakeEvent(God.E(EventTypes.LoseTrait).Set(Traits.Barrier));
        //make it so that my character is faster for a bit, around 3 seconds.
    }
}