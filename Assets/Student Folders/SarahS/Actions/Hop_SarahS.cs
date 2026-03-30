using NUnit.Framework.Internal.Execution;
using UnityEditor;
using UnityEngine;

public class Hop_SarahS : ActionScript
{
    private Vector2 hopStart;
    private Vector2 hopTarget;
    private float hopHeight = 0.5f;
    private float hopDistance = 2f;
    private float hopSpeed = 1f;

    public Hop_SarahS(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.HopSarahS, who);
        Type = Actions.HopSarahS;
        Priority = 2;
        MoveMult = 0;
        CanRotate = false;
    }

    public override void Begin()
    {
        base.Begin();
        ChangePhase(0);
        Duration = 0.5f;
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

        if (Phase == 1)
        {
            AnimateHop();
        }
    }

    public override void Complete()
    {
        if (Phase == 0)
        {
            ChangePhase(1);
            StartHop();
        }
        else if (Phase == 1)
        {
            if (Who.Target != null) Who.Thing.transform.position = hopTarget;
            CheckLandingKill();
            base.Complete();
        }
    }

    private void StartHop()
    {
        if (Who.Thing == null) return;
        hopStart = Who.Thing.transform.position;
        
        float actualDist = hopDistance;

        if (Who.Target != null && Who.Target.Thing != null)
        {
            Vector2 directionToPlayer = Who.Target.Thing.transform.position - Who.Thing.transform.position;
            actualDist = directionToPlayer.magnitude;
            if (actualDist > hopDistance)
            {
                hopTarget = hopStart + directionToPlayer.normalized * hopDistance;
                actualDist = hopDistance;
            }
            else
            {
                hopTarget = Who.Target.Thing.transform.position;
            }

            Who.Thing.LookAt(Who.Target);
        }
        else
        {
            Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            hopTarget = hopStart + randomDir * hopDistance;
        }
        
        Reset();
        Duration = actualDist / hopSpeed;
        if (Duration < 0.2f) Duration = 0.2f;
    }

    private void AnimateHop()
    {
        if (Who.Thing == null) return;
        float hopProgress = Timer / Duration;
        
        Vector2 currentPos = Vector2.Lerp(hopStart, hopTarget, hopProgress);
        float arc = hopHeight * 4f * hopProgress * (1f - hopProgress);
        Who.Thing.transform.position = new Vector3(currentPos.x, currentPos.y + arc, 0f);
    }
   
   private void CheckLandingKill()
   {
       if (Who.Thing == null || God.Player == null || God.Player.Thing == null) return;

       float distance = Vector2.Distance(Who.Thing.transform.position, God.Player.Thing.transform.position);

       if (distance < 1.5f)
       {
           God.Library.GetGnome("Blood").Spawn(God.Player.Thing.transform.position, 15);
           
           EventInfo killEvent = God.E(EventTypes.Death).Set(Who);
           
           God.Player.TakeEvent(killEvent, true); 
           
           if (!God.Session.Defeat) 
           {
               God.GM.SetUI("Health", "CRUSHED", 1);
               God.Player.Destruct(Who);
               God.Session.PlayerDeath(killEvent);
           }
       }
   }
    
    public override Actions NextAction()
    {
        return Actions.HopSarahS;
    }
}
