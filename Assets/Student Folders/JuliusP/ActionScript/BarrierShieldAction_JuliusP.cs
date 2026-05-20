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

public class Lv1BarrierShieldType2_JuliusP : ActionScript
{
    float Offset = 2.3f;

    List<ThingInfo> Lv2_spawnedShields = new List<ThingInfo>();
    List<ThingInfo> redShields = new List<ThingInfo>();

    private Rigidbody2D EnemyRb;
    float RotateSpeed = 120f;

    public Lv1BarrierShieldType2_JuliusP(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.BarrierShieldType2_JuliusP, who);
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

    
        for (int i = Lv2_spawnedShields.Count - 1; i >= 0; i--)
        {
            ThingInfo shield = Lv2_spawnedShields[i];

            if (shield == null || shield.Thing == null)
            {
                Lv2_spawnedShields.RemoveAt(i);
                redShields.Remove(shield);
            }
        }

        //CHECK FOR RED SHIELDS//
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

        // END THE ACTION WHEN ALL RED SHIELDS ARE GONE
        if (!anyRedAlive)
        {
            foreach (ThingInfo shield in Lv2_spawnedShields)
                if (shield != null)
                    shield.Destruct(shield);

            Lv2_spawnedShields.Clear();
            redShields.Clear();

            Complete();
            return;
        }

        // ORBIT SHIELDS
        int shieldCount = Lv2_spawnedShields.Count;

        if (shieldCount > 0)
        {
            float angleStep = 360f / shieldCount;

            for (int i = 0; i < shieldCount; i++)
            {
                ThingInfo shield = Lv2_spawnedShields[i];
                if (shield == null || shield.Thing == null)
                    continue;

                float angle = (i * angleStep + Time.time * RotateSpeed) * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

                shield.Thing.transform.position = Who.Thing.transform.position + offset;
            }
        }

     
    }

    public override void End()
    {
        base.End();

        int Level = God.Session.Level;
        
        if (Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);

        if (Who.Has(Traits.NoTimerStunNegation_JuliusP))
            Who.RemoveTrait(Traits.NoTimerStunNegation_JuliusP);

        foreach (ThingInfo shield in Lv2_spawnedShields)
            if (shield != null)
                shield.Destruct(shield);

        Lv2_spawnedShields.Clear();
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

        int numberOfShields = 4;
        float angleStep = 360f / numberOfShields;

        int randomIndexOne = Random.Range(0, numberOfShields);
        int randomIndexTwo;

        do
        {
            randomIndexTwo = Random.Range(0, numberOfShields);
        }
        while (randomIndexTwo == randomIndexOne);

        for (int i = 0; i < numberOfShields; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

            ThingInfo shieldInfo = Shield.Create();
            ThingController shieldController =
                shieldInfo.Spawn(Who.Thing.transform.position + offset);

            shieldController.transform.parent = null;

            if (i == randomIndexOne || i == randomIndexTwo)
            {
                redShields.Add(shieldInfo);

                SpriteRenderer sr = shieldController.GetComponentInChildren<SpriteRenderer>();
                if (sr != null) sr.color = Color.red;

                shieldInfo.AddTrait(Traits.ShieldTrait_JuliusP);
            }
            else
            {
                EventInfo hp = new EventInfo();
                hp.Set(NumInfo.Default, 2f);
                shieldInfo.AddTrait(Traits.Health, hp);
            }

            Lv2_spawnedShields.Add(shieldInfo);
        }
    }
}




public class Lv2_BarrierShield_JuliusP : ActionScript
{
    float Offset = 1.4f;

    List<ThingInfo> Lv2_spawnedShields = new List<ThingInfo>();
    List<ThingInfo> redShields = new List<ThingInfo>();

    private Rigidbody2D EnemyRb;
    float RotateSpeed = 120f;


    Level_JuliusP LJP;


    public Lv2_BarrierShield_JuliusP(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Lv2_BarrierShield_JuliusP, who);
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

     LJP = God.LB as Level_JuliusP;
            
    }

    public override void OnRun()
    {
        base.OnRun();

        if (Who?.Thing == null || God.Session.Player?.Thing == null)
            return;

    
        for (int i = Lv2_spawnedShields.Count - 1; i >= 0; i--)
        {
            ThingInfo shield = Lv2_spawnedShields[i];

            if (shield == null || shield.Thing == null)
            {
                Lv2_spawnedShields.RemoveAt(i);
                redShields.Remove(shield);
            }
        }

        //CHECK FOR RED SHIELDS//
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

        // END THE ACTION WHEN ALL RED SHIELDS ARE GONE
        if (!anyRedAlive)
        {
            foreach (ThingInfo shield in Lv2_spawnedShields)
                if (shield != null)
                    shield.Destruct(shield);

            Lv2_spawnedShields.Clear();
            redShields.Clear();

            End();
            return;
        }

        // ORBIT SHIELDS
        int shieldCount = Lv2_spawnedShields.Count;

        if (shieldCount > 0)
        {
            float angleStep = 360f / shieldCount;

            for (int i = 0; i < shieldCount; i++)
            {
                ThingInfo shield = Lv2_spawnedShields[i];
                if (shield == null || shield.Thing == null)
                    continue;

                float angle = (i * angleStep + Time.time * RotateSpeed) * Mathf.Deg2Rad;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

                shield.Thing.transform.position = Who.Thing.transform.position + offset;
            }
        }

        // CHASE PLAYER
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

        int Level = God.Session.Level;
        
        if (Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);

        if (Who.Has(Traits.NoTimerStunNegation_JuliusP))
            Who.RemoveTrait(Traits.NoTimerStunNegation_JuliusP);

        foreach (ThingInfo shield in Lv2_spawnedShields)
            if (shield != null)
                shield.Destruct(shield);

        Lv2_spawnedShields.Clear();
        redShields.Clear();

        Actions next = NextAction();
        if (next != null && Who.Thing != null)
            Who.Thing.DoAction(next);



     
        if(Level == 3)
        {
                    
         //LJP.Lv3FirstLevel2ShieldEnemyKilled = true;       
            

        }

      
        if(Level == 3 && LJP.Lv3FirstLevel2ShieldEnemyKilled && LJP.Lv3FirstRedLightKilled && LJP.Lv3RedLight3Killed)
        {
                    
           // LJP.Lv3FinalShieldEnemKilled = true;  
            

        }
   



        }
    

    void SpawnShields()
    {
        ThingOption Shield =
            Resources.Load<ThingOption>("JuliusP/Things With Actions/BarrierShield");

        if (Shield == null) return;

        int numberOfShields = 8;
        float angleStep = 360f / numberOfShields;

        int randomIndexOne = Random.Range(0, numberOfShields);
        int randomIndexTwo;

        do
        {
            randomIndexTwo = Random.Range(0, numberOfShields);
        }
        while (randomIndexTwo == randomIndexOne);

        for (int i = 0; i < numberOfShields; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

            ThingInfo shieldInfo = Shield.Create();
            ThingController shieldController =
                shieldInfo.Spawn(Who.Thing.transform.position + offset);

            shieldController.transform.parent = null;

            if (i == randomIndexOne || i == randomIndexTwo)
            {
                redShields.Add(shieldInfo);

                SpriteRenderer sr = shieldController.GetComponentInChildren<SpriteRenderer>();
                if (sr != null) sr.color = Color.red;

                shieldInfo.AddTrait(Traits.ShieldTrait_JuliusP);
            }
            else
            {
                EventInfo hp = new EventInfo();
                hp.Set(NumInfo.Default, 2f);
                shieldInfo.AddTrait(Traits.Health, hp);
            }

            Lv2_spawnedShields.Add(shieldInfo);
        }
    }
}





public class Lv4_BarrierShield_JuliusP : ActionScript
{
    float Offset = 1.4f;

    List<ThingInfo> Lv2_spawnedShields = new List<ThingInfo>();
    List<ThingInfo> redShields = new List<ThingInfo>();

    // THROWN SHIELDS
    class ThrownShieldData
    {
        public ThingInfo shield;
        public Vector2 dir;
    }

    List<ThrownShieldData> thrownShields = new List<ThrownShieldData>();

    private Rigidbody2D EnemyRb;
    float RotateSpeed = 120f;

    float throwTimer = 0f;
    float throwCooldown = 1.5f;
    float throwSpeed = 8f;

    Level_JuliusP LJP;

    public Lv4_BarrierShield_JuliusP(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Lv4_BarrierShield_JuliusP, who);
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

        LJP = God.LB as Level_JuliusP;

    }

    public override void OnRun()
    {
        base.OnRun();

        if (Who?.Thing == null || God.Session.Player?.Thing == null)
            return;

        throwTimer += Time.deltaTime;

        if (throwTimer >= throwCooldown)
        {
            throwTimer = 0f;
            ThrowShield();
        }

        for (int i = Lv2_spawnedShields.Count - 1; i >= 0; i--)
        {
            ThingInfo shield = Lv2_spawnedShields[i];

            if (shield == null || shield.Thing == null)
            {
                Lv2_spawnedShields.RemoveAt(i);
                redShields.Remove(shield);
            }
        }

        // CLEAN THROWN SHIELDS
        for (int i = thrownShields.Count - 1; i >= 0; i--)
        {
            ThrownShieldData data = thrownShields[i];

            if (data.shield == null || data.shield.Thing == null)
            {
                thrownShields.RemoveAt(i);
            }
        }

        //CHECK FOR RED SHIELDS//
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

        // END THE ACTION WHEN ALL RED SHIELDS ARE GONE
        if (!anyRedAlive)
        {
            foreach (ThingInfo shield in Lv2_spawnedShields)
            {
                if (shield != null)
                    shield.Destruct(shield);
            }

            foreach (ThrownShieldData data in thrownShields)
            {
                if (data.shield != null)
                    data.shield.Destruct(data.shield);
            }

            Lv2_spawnedShields.Clear();
            redShields.Clear();
            thrownShields.Clear();

            End();
            return;
        }

        // ORBIT SHIELDS
        int shieldCount = Lv2_spawnedShields.Count;

        if (shieldCount > 0)
        {
            float angleStep = 360f / shieldCount;

            for (int i = 0; i < shieldCount; i++)
            {
                ThingInfo shield = Lv2_spawnedShields[i];

                if (shield == null || shield.Thing == null)
                    continue;

                float angle =
                    (i * angleStep + Time.time * RotateSpeed) * Mathf.Deg2Rad;

                Vector3 offset =
                    new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

                shield.Thing.transform.position =
                    Who.Thing.transform.position + offset;
            }
        }

        // MOVE THROWN SHIELDS
        for (int i = 0; i < thrownShields.Count; i++)
        {
            ThrownShieldData data = thrownShields[i];

            if (data.shield == null || data.shield.Thing == null)
                continue;

            data.shield.Thing.transform.position +=
                (Vector3)(data.dir * throwSpeed * Time.deltaTime);

            // SPIN SHIELD
            data.shield.Thing.transform.Rotate(0, 0, 360f * Time.deltaTime);
        }

        // CHASE PLAYER
        ThingInfo targ = God.Session.Player;

        if (targ != null && targ.Thing != null)
        {
            Vector3 dir =
                (targ.Thing.transform.position -
                Who.Thing.transform.position).normalized;

            float speed = 1.8f;

            if (EnemyRb != null)
            {
                EnemyRb.MovePosition(
                    Who.Thing.transform.position +
                    dir * speed * Time.deltaTime);
            }
            else
            {
                Who.Thing.transform.position +=
                    dir * speed * Time.deltaTime;
            }

            Who.Thing.LookAt(targ);
        }
    }

    public override void End()
    {
        base.End();

        int Level = God.Session.Level;

        if (Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);

        if (Who.Has(Traits.NoTimerStunNegation_JuliusP))
            Who.RemoveTrait(Traits.NoTimerStunNegation_JuliusP);

        foreach (ThingInfo shield in Lv2_spawnedShields)
        {
            if (shield != null)
                shield.Destruct(shield);
        }

        foreach (ThrownShieldData data in thrownShields)
        {
            if (data.shield != null)
                data.shield.Destruct(data.shield);
        }

        Lv2_spawnedShields.Clear();
        redShields.Clear();
        thrownShields.Clear();

        Actions next = NextAction();

        if (next != null && Who.Thing != null)
            Who.Thing.DoAction(next);

        if (Level == 3)
        {
            //LJP.Lv3FirstLevel2ShieldEnemyKilled = true;       
        }

        if (Level == 3 &&
            LJP.Lv3FirstLevel2ShieldEnemyKilled &&
            LJP.Lv3FirstRedLightKilled &&
            LJP.Lv3RedLight3Killed)
        {
            // LJP.Lv3FinalShieldEnemKilled = true;  
        }
    }

    void ThrowShield()
    {
        if (Lv2_spawnedShields.Count <= 0)
            return;

        ThingInfo shield = Lv2_spawnedShields[0];

        if (shield == null || shield.Thing == null)
            return;

        Lv2_spawnedShields.RemoveAt(0);
        redShields.Remove(shield);

        Vector2 dir =
            (God.Session.Player.Thing.transform.position -
            shield.Thing.transform.position).normalized;

        ThrownShieldData data = new ThrownShieldData();

        data.shield = shield;
        data.dir = dir;

        thrownShields.Add(data);
    }

    void SpawnShields()
    {
        ThingOption Shield =
            Resources.Load<ThingOption>(
                "JuliusP/Things With Actions/BarrierShield");

        if (Shield == null)
            return;

        int numberOfShields = 8;
        float angleStep = 360f / numberOfShields;

        int randomIndexOne = Random.Range(0, numberOfShields);
        int randomIndexTwo;

        do
        {
            randomIndexTwo = Random.Range(0, numberOfShields);
        }
        while (randomIndexTwo == randomIndexOne);

        for (int i = 0; i < numberOfShields; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector3 offset =
                new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

            ThingInfo shieldInfo = Shield.Create();

            ThingController shieldController =
                shieldInfo.Spawn(
                    Who.Thing.transform.position + offset);

            shieldController.transform.parent = null;

            if (i == randomIndexOne || i == randomIndexTwo)
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

            Lv2_spawnedShields.Add(shieldInfo);
        }
    }
}