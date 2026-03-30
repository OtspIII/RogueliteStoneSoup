using UnityEngine;

public class Stalk_SarahS : ActionScript
{
    public Stalk_SarahS(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.StalkSarahS,who);
        Type = Actions.StalkSarahS;
        Priority = 2;
        MoveMult = 0.4f;
        CanRotate = true;
    }

    public override void OnRun()
    {
        if (Who.Target == null)
        {
            foreach (ThingController thing in God.GM.Things)
            {
                if (thing.Info.Team == GameTeams.Player)
                {
                    Who.SetTarget(thing.Info);
                    break;
                }
            }
        }

        if (Who.Target != null && Who.Target.Thing != null)
        {
            Who.Thing.MoveTowards(Who.Target);
            Who.Thing.LookAt(Who.Target, 0.5f);

            float huntTime = Timer;
            MoveMult = Mathf.Min(0.4f + (huntTime * 0.05f), 1.2f);

            if (Who.Thing.Distance(Who.Target) < Who.AttackRange)
            {
                Who.DoAction(Actions.DefaultAttack);
            }
        }
        else
        {
            Who.DesiredMove = Vector2.zero;
        }
    }

    public override void HitBegin(GameCollision col)
    {
        
    }

    public override Actions NextAction()
    {
        return Actions.StalkSarahS;
    }
}
