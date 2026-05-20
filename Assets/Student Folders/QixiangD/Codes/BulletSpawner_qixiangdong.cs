using System.Collections;
using UnityEngine;

public class BulletSpawner_qixiangdong : ActionScript
{
    public float WaveInterval ;
    public int BulletsPerWave ;
    public float BulletSpeed ;

    public BulletSpawner_qixiangdong(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.BulletSpawner_qixiangdong, who);
        WaveInterval = e != null ? e.GetFloat(NumInfo.Default, 2.5f) : 2.5f;
        BulletsPerWave = e != null ? e.GetInt(NumInfo.Phase, 8) : 8;
        BulletSpeed = e != null ? e.GetFloat(NumInfo.Speed, 4f) : 4f;
    }

    public override IEnumerator Script()
    {
        Debug.Log("BulletSpawner Script started");
        while(true)
        {
            float t = 0f;
            while (t < WaveInterval)
            {
                if (Who.Thing == null) yield break;
                t += Time.deltaTime;
                yield return null;
            }
            if (Who.Thing == null) yield break;
            FireWave();
        }
        

    }
    void FireWave()
    {
        Vector2 origin = (Vector2)Who.Thing.transform.position;
        float angleStep = 360f / BulletsPerWave;

        float offset = (Time.time * 30f) % 360f;
        Debug.Log("FireWave called - spawning " + BulletsPerWave + " bullets");
        ThingOption bulletOpt = God.Library.GetThing("BulletHellBullet");

        if (bulletOpt == null)
        {
            Debug.LogWarning("BulletSpawner: no thing tagged BulletHellBullet");
            return;
        }

        for (int b = 0; b < BulletsPerWave; b++)
        {
            float angle = (offset + b * angleStep) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            ThingInfo bullet = bulletOpt.Create();
            bullet.Spawn(origin);

            bullet.TakeEvent(God.E(EventTypes.Knockback).Set(direction * BulletSpeed).Set(Who));

        }
    }
    
}
