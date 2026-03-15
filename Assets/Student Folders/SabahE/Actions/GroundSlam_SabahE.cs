using UnityEngine;

public class GroundSlam_SabahE : ActionScript
{
    private float radius = 3f;
    private float damage = 10f;

    public GroundSlam_SabahE(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.GroundSlam_SabahE, who);

        MoveMult = 3f;
        HaltMomentum = true;
        Priority = 2;
        CanRotate = false;

        Duration = 0.4f;
    }

    public override void Begin()
    {
        base.Begin();

        if (Who == null || Who.Thing == null)
        {
            Complete();
            return;
        }

        Vector2 pos = Who.Thing.transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, radius);

        foreach (Collider2D col in hits)
        {
            ThingController tc = col.GetComponent<ThingController>();
            if (tc == null) continue;
            if (tc.Info == Who) continue;

            tc.Info.TakeEvent(
                God.E(EventTypes.Damage)
                .Set(damage)
                .Set(Who)
            );
        }
    }
}