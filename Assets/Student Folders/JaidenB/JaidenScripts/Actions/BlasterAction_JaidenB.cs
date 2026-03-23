using UnityEngine;

public class BlasterAction_JaidenB : ActionScript
{
    int LaserStack = 10;

    public BlasterAction_JaidenB(ThingInfo who, EventInfo e = null) 
    {
       Setup(Actions.BlasterAction_JaidenB, who);
        Duration = 0.4f;
        CanRotate = true;
        MoveMult = 0.4f;
        Anim = "Shoot";
    }


    public override void Begin()
    {
        base.Begin();
        LaserStack -= 1;

        Debug.Log(LaserStack);
        if (LaserStack > 10) 
        {
            ThingOption proj = GetHeld().Ask(EventTypes.GetProjectile).GetOption();
            Who.Thing.Shoot(proj);
        }
        else if (LaserStack < 10)
        {
            ThingOption proj = GetHeld().Ask(EventTypes.GetProjectile).GetOption();
            Who.Thing.Shoot(proj);
        }
    }


    public override void HitBegin(GameCollision col)
    {

    }

    public override void OnRun()
    {
        base.OnRun();
        if (Who.Target != null)
        {
            Who.Thing.LookAt(Who.Target, 0.5f);
            Who.Thing.MoveTowards(Who.Target, Who.AttackRange);
        }
    }
}
