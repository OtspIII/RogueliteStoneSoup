using UnityEngine;

public class ParryAction: ActionScript  
{
    public float knockback = 5;
    public ParryAction(ThingInfo who, EventInfo e)
    {
        Setup(Actions.ParryAction_ElioR,who);
        MoveMult = 0;
        Priority = 2;
        
    }

    public override void HitBegin(GameCollision collision)
    {
        ThingController Hit = collision.Other;
        Hit.TakeKnockback(Who.Thing.transform.position, knockback);
       // Who.TakeKnockback(Who.Thing.transform.position, knockback);
        
    }
    public override void End()
    {
        //make it so that my character is faster for a bit, around 3 seconds.
    }
}