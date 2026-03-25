using UnityEngine;

public class SpinAction_Yuchen : ActionScript
{
   

    public SpinAction_Yuchen(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.spinAction_Yu, who);
        Anim = "spin+";
    }
    public override void OnRun()
    {
        base.OnRun();
        if (Phase < 1)
        {
            Who.Thing.MoveForwards();
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
