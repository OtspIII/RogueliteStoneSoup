using UnityEngine;

public class RobAction_JaidenB : ActionScript
{
    public RobAction_JaidenB(ThingInfo who, EventInfo e = null) 
    {
       Setup(Actions.RobAction_JaidenB, who, true);
       Anim = "Swing";
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
    // RemoveInventory(ThingInfo) no idea what I'm going to do with this but I want to use this variable
    // Go to the attackaction and make it where you lose a point if hit with the hitend function, dont be afraid to copy code.
}
