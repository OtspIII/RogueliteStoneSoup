using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : ActionScript
{
    public IdleAction(ThingController who,EventInfo e=null)
    {
        Setup(Actions.Idle,who,true);
    }
}

public class StunAction : ActionScript
{   
    public StunAction(ThingController who,EventInfo e=null)
    {
        Setup(Actions.Stun,who);
        Duration = e != null ? e.GetFloat(NumInfo.Amount,1) : 1;
    }

    public override IEnumerator Script()
    {
        float speed = 360 / Duration;
        float rot = 0;
        while (rot < 360)
        {
            rot += speed * Time.deltaTime;
            Who.Body.transform.rotation = Quaternion.Euler(0,0,rot);
            yield return null;
        }
        End();
    }

    public override void End()
    {
        base.End();
        Who.Body.transform.rotation = Quaternion.Euler(0,0,0);
    }
}