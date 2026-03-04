using Unity.Mathematics;
using UnityEngine;

public class CurveChaseAction_RaphaelC : ActionScript
{
    private Vector2 dir;
    public float speed = 5f;
    public float angle = 0f;
    public float radius;
    private float duration = 2f;
    public float timer = 0f;
    public CurveChaseAction_RaphaelC(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.CurveChase_RaphaelC,who,true);
        MoveMult = speed;
        CanRotate = false;
        Priority = 2;

        Setup(Actions.Shoot,who);
        Duration = 1.5f;
        Anim = "Shoot";
    }
    public override void Begin()
    {
        base.Begin();    
        var helditem = GetHeld();  
        ThingOption proj = GetHeld().Ask(EventTypes.GetProjectile).GetOption();
        ThingOption thingOption = GetHeld().Ask(EventTypes.GetDefaultAttack).GetOption();

        if (proj != null && thingOption != null)
        {
            Who.Thing.Shoot(proj);
        }

        else if (thingOption != null)
            return;
        
    }
    public override void OnRun()
    {
        base.OnRun();
        if (Who.Target == null)
        {
            Who.Thing.DoAction(Actions.Patrol);
            return;
        }

        Vector3 A = Who.Thing.transform.position;
        Vector3 B = Who.Target.Thing.transform.position;

        radius = Vector3.Distance(A,B)/2;
        Vector3 Center = (A + B)/2;

        float Cx = Center.x;
        float Cy = Center.y;

        timer += Time.deltaTime;
        float t = timer / duration;
        float angle = math.PI - (t* math.PI);

        dir = new Vector2(Cx + radius * math.cos(angle) - A.x, Cy + radius * math.sin(angle) - A.y).normalized;
        // x = Cx + r cos (theta)
        // y = Cy + r sin (theta)

        Who.Thing.ActualMove = MoveMult * dir;
        
        Who.Thing.LookAt(Who.Target,0.5f);
        
        if((Who.AttackRange <= 0.5f || Who.Thing.Distance(Who.Target) <= Who.AttackRange) && Who.Thing.IsFacing(Who.Target,5))
            Who.TakeEvent(God.E(EventTypes.StartAction).Set(ActionInfo.Action,Actions.DefaultAttack));
    } 
    public override void HandleMove(){
        if(Who.Thing == null)
        {
            return;
        }

        Who.DesiredMove = dir;
        Who.Thing.ActualMove = MoveMult * dir;
    }
}

public class Action2_RaphaelC : ActionScript
{
    public Action2_RaphaelC(ThingInfo who,EventInfo e = null)
    {
        Setup(Actions.Action2_RaphaelC,who,true);
        CanRotate = true;
        MoveMult = 0f;
        Duration = 1f;
        Anim = "Shoot";
    }
    public override void Begin()
    {
        base.Begin();
        ThingOption proj = GetHeld().Ask(EventTypes.GetProjectile).GetOption();
        Who.Thing.Shoot(proj);
    }
    public override void OnRun()
    {
        base.OnRun();
        if (Who.Target != null)
        {
            Who.Thing.LookAt(Who.Target, 0.5f);
            Who.Thing.MoveTowards(Who.Target, Who.AttackRange);
        }
    }


}

public class Action3_RaphaelC : ActionScript
{
    public Action3_RaphaelC(ThingInfo who,EventInfo e = null)
    {
        Setup(Actions.Action3_RaphaelC,who,true);
    }
}
