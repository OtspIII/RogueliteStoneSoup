using UnityEngine;

public class AttackContinuous : ActionScript
{
    public AttackContinuous()
    {
        //meant for turret. will just attack automatically
    }
    public override void OnRun()
    {
        base.OnRun();
        Who.TakeEvent(God.E(EventTypes.StartAction).Set(ActionInfo.Action, Actions.DefaultAttack));

    }
}
