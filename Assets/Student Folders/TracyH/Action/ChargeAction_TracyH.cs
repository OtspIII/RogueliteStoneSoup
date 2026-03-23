using UnityEngine;

public class ChargeAction_TracyH : ActionScript
{
    private Vector2 dir;
    public float speed = 10f;

    public ChargeAction_TracyH(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Charge_TracyH, who, true);
        MoveMult = speed;
        CanRotate = false;
        Priority = 2;
        Duration = 0.8f;
        Anim = "Charge";
    }

    public override void Begin()
    {
        base.Begin();

        if (Who == null || Who.Thing == null)
        {
            Complete();
            return;
        }

        if (Who.Target == null)
        {
            Complete();
            return;
        }

        dir = (Who.Target.Thing.transform.position - Who.Thing.transform.position).normalized;
    }

    public override void OnRun()
    {
        base.OnRun();

        if (Who == null || Who.Thing == null)
        {
            Complete();
            return;
        }

        if (Who.Target == null)
        {
            Who.Thing.DoAction(Actions.Patrol);
            return;
        }

        Who.Thing.ActualMove = MoveMult * dir;
    }

    public override void HandleMove()
    {
        if (Who.Thing == null)
        {
            return;
        }

        Who.DesiredMove = dir;
        Who.Thing.ActualMove = MoveMult * dir;
    }
}