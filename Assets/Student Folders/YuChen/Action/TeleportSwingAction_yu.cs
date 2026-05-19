using System.Collections.Generic;
using UnityEngine;


public class TeleportSwingAction_Yu : AttackAction
{
    public float TeleDistance = 1f;


    public TeleportSwingAction_Yu(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Swing, who);
        Anim = "Swing";
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

        Who.Thing.LookAt(God.Session.Player.Thing,0f);
    }
}
