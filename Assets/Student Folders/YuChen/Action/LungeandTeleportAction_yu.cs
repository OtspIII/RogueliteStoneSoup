using System.Collections.Generic;
using UnityEngine;


public class LungeandTeleportAction_yu : AttackAction
{
    public float TeleDistance = 4f;
    public float LungeSpeedMult = 1f;


    public LungeandTeleportAction_yu(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Lunge, who);
        Anim = "Lunge";
        MoveMult = 0;
        Knockback = 0;
    }

    public override void OnRun()
    {
        if (Phase == 0)
        {
            MoveMult = Who.AttackRange * LungeSpeedMult;
            Who.Thing.MoveForwards();
        }
        else
        {
            MoveMult = 0;
        }
    }
    public override void End()
    {
        base.End();
        TelePlayer();
    }

    void TelePlayer()
    {
        if (God.Session?.Player?.Thing == null || Who?.Thing == null)
            return;
        Transform Player = God.Session?.Player?.Thing.transform;

        Vector2 dir = Random.insideUnitCircle.normalized;

        Who.Thing.transform.position = Player.position + (Vector3)(dir * TeleDistance);

        Who.Thing.LookAt(God.Session.Player.Thing, 0f);
    }
}
