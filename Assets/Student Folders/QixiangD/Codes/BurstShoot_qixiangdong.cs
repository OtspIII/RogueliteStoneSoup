using UnityEngine;
public class BurstShoot_qixiangdong : ActionScript
{
    private float spinSpeed = 180f;
    private float shootTimer = 0f;
    private float burstTimer = 0f;
    private float shootInterval = 0.1f;
    private float burstInterval = 2f;
    private int burstCount = 0;
    private int burstMax = 3;
    private bool isBursting = false;

    public BurstShoot_qixiangdong(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.BurstShoot_qixiangdong, who, true);
        Priority = 3;
        CanRotate = false;
        Duration = 999f;
    }

    public override void OnRun()
    {
        if (Who == null || Who.Thing == null) return;

        Who.Thing.transform.Rotate(0, 0, spinSpeed * Time.deltaTime);

        if (isBursting)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                burstCount++;
                ThingOption proj = GetHeld().Ask(EventTypes.GetProjectile).GetOption();
                if (proj != null)
                    Who.Thing.Shoot(proj);

                if (burstCount >= burstMax)
                {
                    isBursting = false;
                    burstCount = 0;
                    burstTimer = 0f;
                }
            }
        }
        else
        {
            burstTimer += Time.deltaTime;
            if (burstTimer >= burstInterval)
                isBursting = true;
        }

        Who.DesiredMove = Vector2.zero;
        Who.Thing.ActualMove = Vector2.zero;
    }
}
