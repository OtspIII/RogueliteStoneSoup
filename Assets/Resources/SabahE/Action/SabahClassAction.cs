using Unity.VisualScripting;
using UnityEngine;

public class SabahClassAction : ActionScript
{ 

  
    public SabahClassAction(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.SabahClassAction,who);
        MoveMult = 1;
    }

    public override void OnRun()
    {
       ThingInfo targ = Who.Target;
        if (targ != null) return;
        Vector2 dir = (Vector2)(Who.Thing.transform.position - Who.Thing.transform.position);
        dir.Normalize();
        Who.DesiredMove = dir;

    }

}
