using UnityEngine;

public class RestAction : ActionScript
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float healInterval = 1;
    public RestAction(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.RestAction_AdamD, who);
        MoveMult = 0;
        HaltMomentum = true;
        Priority = 0;
        //heals hp, and if has a mana bar, refills that too
    }
    public override void OnRun()
    {
        base.OnRun();
        healInterval-= Time.deltaTime;
        if (healInterval < 0) 
        {
            Who.TakeEvent(God.E(EventTypes.Damage).Set(-1).Set(Who)); //should heal once it is active
            healInterval = 1;
        }

    }
    public override void Begin()
    {
        base.Begin();
        if (Who.Team == GameTeams.Enemy)
        {
            Who.TakeEvent(God.E(EventTypes.GainTrait).Set(Traits.Barrier)); //makes them unable to take dmg nor act
            //helps give player time to prepare while enemy isn't doing anything
        }
    }
    public override void End()
    {
        base.End();
        if (Who.Team == GameTeams.Enemy)
        {
            Who.TakeEvent(God.E(EventTypes.LoseTrait).Set(Traits.Barrier)); //removes this
        }
    }
}
