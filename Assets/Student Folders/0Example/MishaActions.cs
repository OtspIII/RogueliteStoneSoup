
using System.Collections;
using UnityEngine;

public class TestAction1_Misha : ActionScript
{
    public TestAction1_Misha(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.MishaTestAct1,who,true);
    }

    public override void OnRun()
    {
        ThingInfo targ = Who.Target;
        if (targ == null) return;
        Who.Thing.MoveTowards(targ);
        Who.Thing.LookAt(targ);
        //if they are close
        if (Who.Thing.Distance(targ) < 1)
        {
            Who.TakeEvent(God.E(EventTypes.StartAction).Set(ActionInfo.Action,Actions.DefaultAttack));
        }
        //then they attack
    }

}

public class TestAction2_Misha : ActionScript
{
    public TestAction2_Misha(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.MishaTestAct1,who);
    }
}