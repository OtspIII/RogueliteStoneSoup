using UnityEngine;

public class SpinShoot_qixiangdong : ActionScript
{
    private float spinSpeed = 180f;
    private float shootTimer = 0f;
    private float shootInterval = 0.6f;

    public SpinShoot_qixiangdong(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.SpinShoot_qixiangdong, who, true);

        Priority = 3;
        CanRotate = false;
        Duration = 999f;
    }

    public override void OnRun()
    {
        if (Who == null || Who.Thing == null)
            return;

        Who.Thing.transform.Rotate(
            0,
            0,
            spinSpeed * Time.deltaTime
        );

        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;

            ThingOption proj =
                GetHeld()
                .Ask(EventTypes.GetProjectile)
                .GetOption();

            if (proj != null)
            {
                Who.Thing.Shoot(proj);
            }
        }

        Who.DesiredMove = Vector2.zero;
        Who.Thing.ActualMove = Vector2.zero;
    }
}