using System.Collections;
using UnityEngine;

public class SelfKillAction : ActionScript
{
    public SelfKillAction(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.SelfKill,who,true);
    }

    public override IEnumerator Script()
    {
        yield return null; // wait a frame before suicide
        
        Who.TakeEvent(God.E(EventTypes.Death).Set(Who));
    }
}