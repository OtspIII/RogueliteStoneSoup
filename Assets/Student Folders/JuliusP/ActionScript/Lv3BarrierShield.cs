using UnityEngine;
using System.Collections.Generic;

public class Lv3BarrierShield : ActionScript
{
    float Offset = 1.4f;

    List<ThingInfo> Lv3_spawnedShields = new List<ThingInfo>();
    List<ThingInfo> redShields = new List<ThingInfo>();

    private Rigidbody2D EnemyRb;
    float RotateSpeed = 120f;




    public Lv3BarrierShield(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Lv3_BarrierShield_JuliusP, who);
    }

    public override void Begin()
    {
        base.Begin();




        if (!Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);

        if (!Who.Has(Traits.NoTimerStunNegation_JuliusP))
            Who.AddTrait(Traits.NoTimerStunNegation_JuliusP);

        SpawnShields();

        EnemyRb = Who.Thing?.GetComponent<Rigidbody2D>();
        if (EnemyRb != null)
            EnemyRb.simulated = true;
    }

    public override void OnRun()
    {
        base.OnRun();

        if (Who?.Thing == null || God.Session.Player?.Thing == null)
            return;



        // CLEAN UP DEAD SHIELDS
        for (int i = Lv3_spawnedShields.Count - 1; i >= 0; i--)
        {
            ThingInfo shield = Lv3_spawnedShields[i];

            if (shield == null || shield.Thing == null)
            {
                Lv3_spawnedShields.RemoveAt(i);
                redShields.Remove(shield);
            }
        }

        // CHECK RED SHIELDS ALIVE
        bool anyRedAlive = false;

        for (int i = 0; i < redShields.Count; i++)
        {
            ThingInfo shield = redShields[i];
            if (shield != null && shield.Thing != null)
            {
                anyRedAlive = true;
                break;
            }
        }

        // END WHEN ALL RED SHIELDS ARE GONE
        if (!anyRedAlive)
        {
            foreach (ThingInfo shield in Lv3_spawnedShields)
                if (shield != null)
                    shield.Destruct(shield);

            Lv3_spawnedShields.Clear();
            redShields.Clear();

            End();
            return;
        }

        // ORBIT (SAME AS LV2)
        int shieldCount = Lv3_spawnedShields.Count;

        if (shieldCount > 0)
        {
            float angleStep = 360f / shieldCount;

            for (int i = 0; i < shieldCount; i++)
            {
                ThingInfo shield = Lv3_spawnedShields[i];
                if (shield == null || shield.Thing == null)
                    continue;

                float angle = (i * angleStep + Time.time * RotateSpeed) * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

                shield.Thing.transform.position =
                    Who.Thing.transform.position + offset;
            }
        }

        // CHASE PLAYER (same Lv2 style)
        ThingInfo targ = God.Session.Player;

        if (targ != null && targ.Thing != null)
        {
            Vector3 dir = (targ.Thing.transform.position - Who.Thing.transform.position).normalized;
            float speed = 1.8f;

            if (EnemyRb != null)
                EnemyRb.MovePosition(Who.Thing.transform.position + dir * speed * Time.deltaTime);
            else
                Who.Thing.transform.position += dir * speed * Time.deltaTime;

            Who.Thing.LookAt(targ);
        }
    }

    public override void End()
    {
        base.End();

        if (Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);

        if (Who.Has(Traits.NoTimerStunNegation_JuliusP))
            Who.RemoveTrait(Traits.NoTimerStunNegation_JuliusP);

        foreach (ThingInfo shield in Lv3_spawnedShields)
            if (shield != null)
                shield.Destruct(shield);

        Lv3_spawnedShields.Clear();
        redShields.Clear();

        Actions next = NextAction();
        if (next != null && Who.Thing != null)
            Who.Thing.DoAction(next);
    }

    void SpawnShields()
    {
        ThingOption Shield =
            Resources.Load<ThingOption>("JuliusP/Things With Actions/BarrierShield");

        if (Shield == null) return;

        int numberOfShields = 8;

        float angleStep = 360f / numberOfShields;

       
        int redCount = 3;
        List<int> redIndexes = new List<int>();

        while (redIndexes.Count < redCount)
        {
            int r = Random.Range(0, numberOfShields);
            if (!redIndexes.Contains(r))
                redIndexes.Add(r);
        }

        for (int i = 0; i < numberOfShields; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

            ThingInfo shieldInfo = Shield.Create();
            ThingController shieldController =
                shieldInfo.Spawn(Who.Thing.transform.position + offset);

            shieldController.transform.parent = null;

            if (redIndexes.Contains(i))
            {
                redShields.Add(shieldInfo);

                SpriteRenderer sr =
                    shieldController.GetComponentInChildren<SpriteRenderer>();

                if (sr != null)
                    sr.color = Color.red;

                shieldInfo.AddTrait(Traits.ShieldTrait_JuliusP);
            }
            else
            {
                EventInfo hp = new EventInfo();
                hp.Set(NumInfo.Default, 2f);
                shieldInfo.AddTrait(Traits.Health, hp);
            }

            Lv3_spawnedShields.Add(shieldInfo);
        }
    }
}