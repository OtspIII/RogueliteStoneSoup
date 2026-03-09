using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseAction : ActionScript
{
    public ChaseAction(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.Chase,who,true);
    }

    public override void OnRun()
    {
        base.OnRun();
        Who.Thing.MoveTowards(Who.Target,Who.AttackRange);
        Who.Thing.LookAt(Who.Target,0.5f);
        
        if((Who.AttackRange <= 0.5f || Who.Thing.Distance(Who.Target) <= Who.AttackRange) && Who.Thing.IsFacing(Who.Target,5))
            Who.TakeEvent(God.E(EventTypes.StartAction).Set(ActionInfo.Action,Actions.DefaultAttack));
            // Who.DoAction(Who.DefaultAttackAction());
    }
}

public class PatrolAction : ActionScript
{
    public Vector3 Target;
    
    public PatrolAction(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.Patrol,who,true);
    }

    public override void Begin()
    {
        base.Begin();
        NewTarget();
    }

    public override void OnRun()
    {
        base.OnRun();
        if (Who.Target != null && Who.Thing.SeenThings.Contains(Who.Target))
        {
            Vector2 dir = Who.Target.Thing.transform.position - Who.Thing.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(Who.Thing.transform.position,
                dir, Who.VisionRange, LayerMask.GetMask("Wall"));
            if (hit.collider == null)
            {
                Who.Thing.DoAction(Actions.Chase);
                return;
            }
        }
        if (Who.Thing.Distance(Who.Target) < 0.2f)
        {
            NewTarget();
        }
        float turn = Who.Thing.LookAt(Target,0.5f);
        if (turn < 5)
        {
            RaycastHit2D hit = Physics2D.Raycast(Who.Thing.transform.position, Who.Thing.Body.transform.right,
                1, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                NewTarget();
                return;
            }
            Who.Thing.MoveTowards(Target,0);
        }
        
    }

    void NewTarget()
    {
        Target = Who.Thing.StartSpot + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        // Who.DebugTxt = Target.ToString();
    }

}