using UnityEngine;

public class DoubleShootV2 : ActionScript
{
    public DoubleShootV2(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Shoot, who);

        Duration = 0.02f;
        CanRotate = true;
        MoveMult = 0.5f;
        Anim = "Shoot";
    }

    public override void Begin()
    {
        base.Begin();

        TripleShot();
    }

    public override void OnRun()
    {
        base.OnRun();

        if (Who.Target != null)
        {
            // LOOK AT TARGET
            Who.Thing.LookAt(Who.Target, 0.5f);

            // MOVE TOWARD TARGET
            Who.Thing.MoveTowards(Who.Target, Who.AttackRange);
        }
    }

    private void TripleShot()
    {
        // GET PROJECTILE FROM HELD WEAPON
        ThingOption proj = GetHeld().Ask(EventTypes.GetProjectile).GetOption();

        // SAFETY CHECK
        if (proj == null) return;

        // SAVE ORIGINAL ROTATION
        Quaternion originalRot = Who.Thing.transform.rotation;

        // LEFT PROJECTILE (45 DEGREES)
        Who.Thing.transform.rotation = originalRot * Quaternion.Euler(0, 0, 3);
        Who.Thing.Shoot(proj);

        // CENTER PROJECTILE
        Who.Thing.transform.rotation = originalRot;
        Who.Thing.Shoot(proj);

        // RIGHT PROJECTILE (-45 DEGREES)
        Who.Thing.transform.rotation = originalRot * Quaternion.Euler(0, 0, -3);
        Who.Thing.Shoot(proj);

        // RESET ROTATION BACK TO NORMAL
        Who.Thing.transform.rotation = originalRot;
    }

    public override void End()
    {
        base.End();
    }
}
