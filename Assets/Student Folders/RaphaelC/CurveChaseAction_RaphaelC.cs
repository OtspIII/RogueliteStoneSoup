using UnityEngine;

public class CurveChaseAction_RaphaelC : ActionScript
{
    public CurveChaseAction_RaphaelC(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.CurveChase_RaphaelC,who,true);
        //anim = "CurveMoce";
    }
    public override void OnRun()
    {
        base.OnRun();
        //who.Thing.MoveTowards(Who.Target,Who.AttackRange);
        Who.Thing.LookAt(Who.Target,0.5f);
        
        if((Who.AttackRange <= 0.5f || Who.Thing.Distance(Who.Target) <= Who.AttackRange) && Who.Thing.IsFacing(Who.Target,5))
            Who.TakeEvent(God.E(EventTypes.StartAction).Set(ActionInfo.Action,Actions.DefaultAttack));
    }
}
