using UnityEngine;

public class Possession_SarahS : ActionScript
{
    private float channelTime = 1.5f;
    private float possessRange = 4f;
    private ThingInfo targetToPossess = null;

    public Possession_SarahS(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.PossessionSarahS, who);
        Type = Actions.PossessionSarahS;
        Priority = 3;
        MoveMult = 0;
        CanRotate = false;
        Duration = channelTime;
    }

    public override void Begin()
    {
        base.Begin();

        float closestDist = possessRange;
        foreach (ThingController thing in God.GM.Things)
        {
            if (thing.Info == Who || thing.Info.Team == GameTeams.Player) continue;

            float dist = Who.Thing.Distance(thing);
            if (dist < closestDist)
            {
                closestDist = dist;
                targetToPossess = thing.Info;
            }
        }

        if (targetToPossess != null)
        {
            Who.Thing.LookAt(targetToPossess);
        }
        else
        {
            Complete();
        }
    }

    public override void OnRun()
    {
        
    }

    public override void Complete()
    {
        if (targetToPossess != null && targetToPossess.Thing != null)
        {
            Vector3 oldPos = Who.Thing.transform.position;
            Vector3 newPos = targetToPossess.Thing.transform.position;

            foreach (Traits trait in Who.Trait.Keys)
            {
                TraitInfo traitInfo = Who.Trait[trait];
                EventInfo copyInfo = new EventInfo();
                foreach (NumInfo num in traitInfo.Numbers.Keys)
                    copyInfo.SetFloat(num, traitInfo.GetFloat(num));
            }
            
            targetToPossess.Team = Who.Team;
            targetToPossess.Thing.SetTeam(Who.Team);
            
            Who.Destruct();
        }
        base.Complete();
    }

    public override Actions NextAction()
    {
        return Actions.Idle;
    }
}
