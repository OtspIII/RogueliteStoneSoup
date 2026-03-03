using UnityEngine;

public class Charging_qixiangdong : ActionScript
{
    private float chargeSpeed = 8f;
    private Vector2 chargeDirection;
    private ThingInfo target;
    public ThingController C;

    public Charging_qixiangdong(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Charging_qixiangdong, who);

        MoveMult = 0f;
        HaltMomentum = true;
        Priority = 2;
        CanRotate = false;
        // Duration

        if(e != null)
        {
            target = e.Get(ThingEInfo.Default);
        }
    }

    public override void Begin()
    {

        if (target == null)
        {
            End();
            return;
        }

    }

}
