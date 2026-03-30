using UnityEngine;

public class RiseFromDead_SarahS : ActionScript
{
    private bool hasRisen = false;
    private float detectionRange = 1f;

    public RiseFromDead_SarahS(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.RiseFromDeadSarahS, who);
        Type = Actions.RiseFromDeadSarahS;
        Priority = 1;
        MoveMult = 0;
        CanRotate = false;
    }

    public override void OnRun()
    {
        if (hasRisen)
        {
            return;
        }

        foreach (ThingController thing in God.GM.Things)
        {
            if (thing.Info.Team == GameTeams.Player && thing.Info.Thing != null)
            {
                float dist = Who.Thing.Distance(thing);
                if (dist < detectionRange)
                {
                    hasRisen = true;
                    Duration = 0.5f;
                    
                    Who.Thing.MoveTowards(thing, 0);
                    Who.Thing.TakeKnockback(thing, -15f);

                    if (dist < 1.5f)
                    {
                        thing.Info.TakeEvent(God.E(EventTypes.Damage).Set(1f).Set(Who).Set(StrInfo.DType, "Ambush"));
                    }

                    break;
                }
            }
        }
    }

    public override Actions NextAction()
    {
        if (hasRisen)
        {
            return Who.Ask(God.E(EventTypes.GetDefaultAttack)).GetAction();
        }
        return Actions.RiseFromDeadSarahS;
    }

    public override void Reset()
    {
        base.Reset();
        hasRisen = false;
    }
}
