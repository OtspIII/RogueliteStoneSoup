using UnityEngine;

public class Charging_qixiangdong : ActionScript
{
    private float chargeSpeed = 8f;
    private Vector2 chargeDirection;
    private ThingInfo target;

    
    public Charging_qixiangdong(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Charging_qixiangdong, who);

        MoveMult = 0f;
        HaltMomentum = true;
        Priority = 2;
        CanRotate = false;

        Duration = 0.8f;

        if (e != null)
        {
            target = e.Get(ThingEInfo.Default);
        }
    }

    public override void Begin()
    {
        base.Begin();

        if (target == null)
        {
            target = Who.Target;
        }

        if (target == null || target.Thing == null)
        {
            Complete();
            return;
        }

        Vector2 myPos = Who.Thing.transform.position;
        Vector2 targetPos = target.Thing.transform.position;

        chargeDirection = (targetPos - myPos).normalized;
    }

    public override void HandleMove()
    {
        if (Who.Thing == null || Who.Thing.RB == null)
            return;


        Who.Thing.ActualMove = chargeDirection * chargeSpeed;
    }
    public override void OnRun()
    {
        base.OnRun();

        if (Who.Target == null)
        {
            return;
        }
        Who.Thing.LookAt(Who.Target, 0.5f);

        if (Who.Thing.Distance(Who.Target) <= Who.AttackRange)
        {
            Who.DoAction(Actions.DefaultAttack);
        }


    }

    public override void End()
    {
        Who.DesiredMove = Vector2.zero;
        base.End();
    }
}
