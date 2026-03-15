using UnityEngine;

public class RestAction : ActionScript
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }
}
