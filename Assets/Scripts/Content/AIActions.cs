using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChaseAction : ActionScript
{
    public ChaseAction(ThingController who,EventInfo e=null)
    {
        Setup(Actions.Chase,who,true);
    }

    public override void OnRun()
    {
        base.OnRun();
        Who.MoveTowards(Who.Target,Who.AttackRange);
        Who.LookAt(Who.Target,0.5f);
        
        if((Who.AttackRange <= 0.5f || Who.Distance(Who.Target) <= Who.AttackRange) && Who.IsFacing(Who.Target,5))
            Who.TakeEvent(God.E(EventTypes.StartAction).SetEnum(EnumInfo.Action,(int)Actions.DefaultAttack));
            // Who.DoAction(Who.DefaultAttackAction());
    }
}

public class PatrolAction : ActionScript
{
    public Vector3 Target;
    
    public PatrolAction(ThingController who,EventInfo e=null)
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
        if (Who.Target != null && Vector2.Distance(Who.Target.transform.position,
                Who.transform.position) < Who.VisionRange)
        {
            Vector2 dir = Who.Target.transform.position - Who.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(Who.transform.position,
                dir, Who.VisionRange, LayerMask.GetMask("Wall"));
            if (hit.collider == null)
            {
                Who.DoAction(Actions.Chase);
                return;
            }
        }
        if (Vector2.Distance(Target, Who.transform.position) < 0.2f)
        {
            NewTarget();
        }
        float turn = Who.LookAt(Target,0.5f);
        if (turn < 5)
        {
            RaycastHit2D hit = Physics2D.Raycast(Who.transform.position, Who.Body.transform.right,
                1, LayerMask.GetMask("Wall"));
            if (hit.collider != null)
            {
                NewTarget();
                return;
            }
            Who.MoveTowards(Target,0);
        }
        
    }

    void NewTarget()
    {
        Target = Who.StartSpot + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        // Who.DebugTxt = Target.ToString();
    }

}