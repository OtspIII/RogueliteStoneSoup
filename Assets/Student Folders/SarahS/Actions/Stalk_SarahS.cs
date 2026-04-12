using UnityEngine;

public class Stalk_SarahS : ActionScript
{
    private float huntTimer = 0f;
    public Stalk_SarahS(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.StalkSarahS,who, true);
        Type = Actions.StalkSarahS;
        MoveMult = 0.4f;
        CanRotate = true;
        Duration = 0f;
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

            huntTimer += Time.deltaTime;
            MoveMult = Mathf.Min(0.4f + (huntTimer * 0.05f), 1.2f);

            float distance = Who.Thing.Distance(Who.Target);
            if (distance <= Who.AttackRange)
            {
                if (Who.Thing.HeldBody != null)
                {
                    Who.Thing.HeldBody.gameObject.SetActive(true);
                }
                
                Who.DoAction(Actions.DefaultAttack);
            }
            else
            {
                if (Who.Thing.HeldBody != null)
                {
                    Who.Thing.HeldBody.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Who.DesiredMove = Vector2.zero;
        }
    }

    public override void Reset()
    {
        base.Reset();
        huntTimer = 0f;
    }

    public override Actions NextAction()
    {
        return Actions.StalkSarahS;
    }
}
