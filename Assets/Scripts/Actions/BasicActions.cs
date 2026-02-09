using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAction : ActionScript
{
    public IdleAction(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.Idle,who,true);
    }
}

public class StunAction : ActionScript
{   
    public StunAction(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.Stun,who);
        Duration = e != null ? e.GetFloat(NumInfo.Default,0.5f) : 0.5f;
    }

    public override IEnumerator Script()
    {
        float speed = 360 / Duration;
        float rot = 0;
        while (rot < 360)
        {
            if (Who.Thing == null) yield break;
            rot += speed * Time.deltaTime;
            Who.Thing.Body.transform.rotation = Quaternion.Euler(0,0,rot);
            yield return null;
        }
        End();
    }

    public override void End()
    {
        base.End();
        if (Who == God.Player)
        {
            Who.Thing.LookAt(God.MouseLoc());
        }
        else
            Who.Thing.Body.transform.rotation = Quaternion.Euler(0,0,0);
    }
}

//A generic interact action. Doesn't do anything, just plays a generic 'use' animation
//For things like drinking potions. Also attack inherits from this
public class UseAction : ActionScript
{
    public UseAction(){ }
    
    public UseAction(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.Use,who);
        Anim = "Use";
        Duration = e != null ? e.GetFloat(NumInfo.Default,0.5f) : 0.5f;
    }

    public override void End()
    {
        base.End();
        Who.TakeEvent(God.E(EventTypes.UseHeldEnd));
    }

    public override void Complete()
    {
        base.Complete();
        Who.TakeEvent(God.E(EventTypes.UseHeldComplete));
    }

    public override void Abort(ActionScript newAct)
    {
        base.Abort(newAct);
        Who.TakeEvent(God.E(EventTypes.UseHeldAbort).Set(newAct.Type));
    }
}