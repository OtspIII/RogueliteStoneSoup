using UnityEngine;
using System.Collections.Generic;

public class BarrierShieldAction_JuliusP : ActionScript
{
    // THIS SETS HOW FAR THE SHIELDS ARE//
    float Offset = 1.1f;

    // THIS CREATES A LIST TO KEEP TRACK OF ALL SHIELDS SPAWNED
    List<ThingInfo> spawnedShields = new List<ThingInfo>();

    private Rigidbody2D EnemyRb;

    ThingInfo redShield;
    float RotateSpeed = 120f;

    bool ended = false;
    bool locked = false;

    public BarrierShieldAction_JuliusP(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.BarrierShield_JuliusP, who, true);
        HaltMomentum = true;
        MoveMult = 0f;
    }

    public override void Begin()
    {
        if (ended || locked) return;

        base.Begin();

        if (!Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);

        SpawnShields();

        EnemyRb = Who.Thing.GetComponent<Rigidbody2D>();
        EnemyRb.simulated = true;
    }

    public override void OnRun()
    {
        base.OnRun();

        if (ended) return;

        // ONLY CHECK RED SHIELD (CRITICAL FIX)
        if (redShield == null || redShield.Get(Traits.Health) == null || redShield.Get(Traits.Health).GetN() <= 0)
        {
            foreach (ThingInfo shield in spawnedShields)
                if (shield != null) shield.Destruct(shield);

            spawnedShields.Clear();
            End();
            return;
        }

        // REMOVE DEAD NON-RED SHIELDS (NO END CONDITION HERE)
        for (int i = spawnedShields.Count - 1; i >= 0; i--)
        {
            ThingInfo shield = spawnedShields[i];

            if (shield == null || shield.Get(Traits.Health) == null || shield.Get(Traits.Health).GetN() <= 0)
            {
                spawnedShields.RemoveAt(i);
            }
        }

        int shieldCount = spawnedShields.Count;
        float angleStep = 360f / shieldCount;

        for (int i = 0; i < shieldCount; i++)
        {
            ThingInfo shield = spawnedShields[i];
            if (shield == null || shield.Thing == null) continue;

            float angle = (i * angleStep + Time.time * RotateSpeed) * Mathf.Deg2Rad;
            Vector3 Shieldoffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;
            shield.Thing.transform.position = Who.Thing.transform.position + Shieldoffset;
        }

        ThingInfo targ = God.Session.Player;

        if (targ != null)
            Who.Thing.LookAt(targ);
    }

    public override void End()
    {
        if (ended) return;

        base.End();

        ended = true;
        locked = true;

        if (Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);

        foreach (ThingInfo shield in spawnedShields)
        {
            if (shield != null)
                shield.Destruct(shield);
        }

        spawnedShields.Clear();
    }

    void SpawnShields()
    {
        ThingOption Shield = Resources.Load<ThingOption>("JuliusP/Things With Actions/BarrierShield");

        int numberOfShields = 9;

        float angleStep = 360f / numberOfShields;

        for (int i = 0; i < numberOfShields; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 Shieldoffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

            Vector3 spawnPos = Who.Thing.transform.position + Shieldoffset;

            ThingInfo shieldInfo = Shield.Create();

            ThingController shieldController = shieldInfo.Spawn(spawnPos);

            shieldController.transform.parent = null;

            EventInfo hp = new EventInfo();

           if (i == 0)
           {
             hp.Set(NumInfo.Default, 1);
             hp.Set(NumInfo.Max, 1);
             hp.Set(NumInfo.Min, 1);

             redShield = shieldInfo;

             SpriteRenderer sr = shieldController.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.color = Color.red;
            }
            else
            {
                hp.Set(NumInfo.Default, Mathf.Infinity);
            }

            shieldInfo.AddTrait(Traits.Health, hp);

            spawnedShields.Add(shieldInfo);
        }
    }
}
//FOR LEVEL2 VERSION OF THE ACTION//
public class Lv2_BarrierShield_JuliusP : ActionScript
{
    //SHIELD OFFSET DISTANCE//
    float Offset = 1.4f;

    // TRACK ALL SHIELDS//
    List<ThingInfo> Lv2_spawnedShields = new List<ThingInfo>();

    // TRCK THE RED SHIELDS//
    List<ThingInfo> redShields = new List<ThingInfo>();

    private Rigidbody2D EnemyRb;

    float RotateSpeed = 120f;

    public Lv2_BarrierShield_JuliusP(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Lv2_BarrierShield_JuliusP, who);
    }

    public override void Begin()
    {
        base.Begin();

        // GRANT DMG RESISTANCE TRAIT//
        if (!Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);
           

        // SPAWN SHIELDS
        SpawnShields();

        EnemyRb = Who.Thing?.GetComponent<Rigidbody2D>();
        if (EnemyRb != null)
            EnemyRb.simulated = true;
    }

    public override void OnRun()
    {
        base.OnRun();

        // SAFETY CHECK: IF WHO OR PLAYER IS NULL, DO NOTHING
        if (Who?.Thing == null || God.Session.Player?.Thing == null)
            return;

        // REMOVE DESTROYED SHIELDS FROM THE LISTS//
        for (int i = Lv2_spawnedShields.Count - 1; i >= 0; i--)
        {
            ThingInfo shield = Lv2_spawnedShields[i];
            if (shield == null || shield.Get(Traits.Health)?.GetN() <= 0)
            {
                Lv2_spawnedShields.RemoveAt(i);
                redShields.Remove(shield); 
            }
        }

        // END THE ACTION IF BOTH RED SHILEDS ARE DESTORYED//
        if (redShields.Count == 0)
        {
            foreach (ThingInfo shield in Lv2_spawnedShields)
                if (shield != null) shield.Destruct(shield);

            Lv2_spawnedShields.Clear();
            End();
            return;
        }

        // MAKE THE SHIELDS ORBIT AROUD THE THING//
        int shieldCount = Lv2_spawnedShields.Count;
        if (shieldCount > 0)
        {
            float angleStep = 360f / shieldCount;
            for (int i = 0; i < shieldCount; i++)
            {
                ThingInfo shield = Lv2_spawnedShields[i];
                if (shield == null || shield.Thing == null) continue;

                float angle = (i * angleStep + Time.time * RotateSpeed) * Mathf.Deg2Rad;
                Vector3 Shieldoffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;
                shield.Thing.transform.position = Who.Thing.transform.position + Shieldoffset;
            }
        }

        // MAKES THE THING CHASE THE PLAYER SLOWLY//
        ThingInfo targ = God.Session.Player;

        if (targ != null && targ.Thing != null)
        {
            Vector3 dir = (targ.Thing.transform.position - Who.Thing.transform.position).normalized;

            float speed = 13f;

            // CONSISTENT RIGIDBODY MOVEMENT
            if (EnemyRb != null)
            {
                Vector3 targetPos = Who.Thing.transform.position + dir * speed * Time.deltaTime;
                EnemyRb.MovePosition(targetPos);
            }
            else
            {
                Who.Thing.transform.position += dir * speed * Time.deltaTime;
            }

            // ALWAYS FACE THE PLAYER
            Who.Thing.LookAt(targ);
        }
    }

    public override void End()
    {
        base.End();

        // REMOVE DMG IMMUNITY TRAIT//
        if (Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);
            
          

        // DESTROY REMAINING SHIELDS//
        foreach (ThingInfo shield in Lv2_spawnedShields)
            if (shield != null)
                shield.Destruct(shield);

        Lv2_spawnedShields.Clear();
        redShields.Clear();

        // TRIGGERS NEXT ACTION//
        Actions next = NextAction();
        if (next != null && Who.Thing != null)
            Who.Thing.DoAction(next);
    }

    void SpawnShields()
    {
        //FIND THE SHILED THINGOPTION IN THE FOLDER IT'S IN//
        ThingOption Shield = Resources.Load<ThingOption>("JuliusP/Things With Actions/BarrierShield");

        // SAFETY CHECK: IF PREFAB NOT FOUND
        if (Shield == null) return;

        //GETS THE THINGINFO OF THE PLAYER//
        ThingInfo Player = God.Session.Player;

        // MAKE 8 SHIELDS SPAWN AROUND THE THING LIKE A PROTECTIVE BARRIER//
        int numberOfShields = 8;

        // THIS MAKES EACH SHIELD 45 DEGREES APART//
        float angleStep = 360f / numberOfShields;

        // PICKS TWO SHIELDS TO BE THE CRIT POINTS//
        int randomIndexOne = Random.Range(0, numberOfShields);
        int randomIndexTwo;
        do
        {   
            //THIS MAKES SURE THAT THE TWO SHIELDS AREN'T CHOSEN TWICE//
            randomIndexTwo = Random.Range(0, numberOfShields);
        } while (randomIndexTwo == randomIndexOne);

        for (int i = 0; i < numberOfShields; i++)
        {
            // CALCULATES THE ANGLE IN RADIANS:
            float angle = i * angleStep * Mathf.Deg2Rad;

            // CALCULATES THE OFFSET USING COS FOR X, SIN FOR Y -> UNIT CIRCLE//
            Vector3 Shieldoffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

            // GET FINAL SPAWN POSITION RELATIVE TO CHARACTER//
            Vector3 spawnPos = Who.Thing.transform.position + Shieldoffset;

            // THIS LINE CREATES A NEW SHIELD OBJECT//
            ThingInfo shieldInfo = Shield.Create();

            // SPAWNS A SHIELD AT THE POSITION//
            ThingController shieldController = shieldInfo.Spawn(spawnPos);

            // MAKES THE SHIELD ATTACHED TO THE THING//
            shieldController.transform.parent = null;

            // CREATES A NEW HEALTH EVENT//
            EventInfo hp = new EventInfo();

            //CHECKS WHEN i IS THE RANDOM INDEXES
            if (i == randomIndexOne || i == randomIndexTwo)
            {
                // SET THE VALUE TO BE 1 (CRIT POINTS)
                hp.Set(NumInfo.Default, 1); 

                redShields.Add(shieldInfo);

                //MAKE THE CRITS APPEAR RED (VITALS)
                SpriteRenderer sr = shieldController.GetComponentInChildren<SpriteRenderer>();
                if (sr != null) sr.color = Color.red;
            }
            else
            {
                // ALL SHIELDS ARE IMMUNE//
                hp.Set(NumInfo.Default, Mathf.Infinity);
            }

            //ADD THE HEALTH TRAIT TO THE SHIELDS//
            shieldInfo.AddTrait(Traits.Health, hp);
            Lv2_spawnedShields.Add(shieldInfo);
        }
    }
}