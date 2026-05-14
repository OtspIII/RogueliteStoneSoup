using System.Collections.Generic;
using UnityEngine;
public class Swingthenshoot_yu : AttackAction
{
    public Swingthenshoot_yu(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Swing, who);
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
        {
            MoveMult = 0;
        }
    }

    public override void End()
    {
        base.End();
        DoShoot();

    }
    void DoShoot()
    {
        if (Who == null || Who.Thing == null)
            return;

        ThingOption proj = GetHeld().Ask(EventTypes.GetProjectile).GetOption();
        Who.Thing.Shoot(proj);
    }
}
