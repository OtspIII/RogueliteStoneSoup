using UnityEngine;

public class Dash_SabahE : ActionScript
{
    private Vector2 dashDirection;
    private float dashSpeed = 20f;

    public Dash_SabahE(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Dash_SabahE, who);

        MoveMult = 0f;
        HaltMomentum = true;
        Priority = 2;
        CanRotate = false;

        Duration = 0.15f;

        if (e != null)
            dashDirection = e.GetVector();
    }

    public override void Begin()
    {
        base.Begin();

        if (dashDirection == Vector2.zero)
        {
            Complete();
            return;
        }

        dashDirection = dashDirection.normalized;
    }

    public override void HandleMove()
    {
        if (Who.Thing == null)
            return;

        Who.Thing.ActualMove = dashDirection * dashSpeed;
    }

    public override void End()
    {
        Who.DesiredMove = Vector2.zero;
        base.End();
    }
}