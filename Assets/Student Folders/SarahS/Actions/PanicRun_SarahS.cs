using UnityEngine;
using UnityEngine.Audio;

public class PanicRun_SarahS : ActionScript
{
    private float panicDuration = 3f;
    private Vector2 runDirection;

    public PanicRun_SarahS(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.PanicRunSarahS, who);
        Type = Actions.PanicRunSarahS;
        Priority = 4;
        MoveMult = 1.8f;
        CanRotate = true;
        Duration = panicDuration;
    }

    public override void Begin()
    {
        base.Begin();

        if (Who.CurrentHeld != null)
        {
            Who.DropHeld(false);
        }

        ThingInfo nearestThreat = null;
        float closestDist = 999f;

        foreach (ThingController thing in God.GM.Things)
        {
            if (thing.Info.Team == GameTeams.Enemy)
            {
                float dist = Who.Thing.Distance(thing);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    nearestThreat = thing.Info;
                }
            }
        }

        if (nearestThreat != null && nearestThreat.Thing != null)
        {
            runDirection = (Who.Thing.transform.position - nearestThreat.Thing.transform.position).normalized;
        }
        else
        {
            runDirection = Vector2.zero;
        }
    }

    public override void OnRun()
    {
        Who.DesiredMove = runDirection;
        if (runDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(runDirection.y, runDirection.x) * Mathf.Rad2Deg;
            Who.Thing.Body.transform.rotation = Quaternion.Euler(0,0,angle);
        }
    }

    public override Actions NextAction()
    {
        return Actions.Idle;
    }
}
