using UnityEngine;
using System.Collections.Generic;

public class Lv3BarrierShield : ActionScript
{
    // SET BASE ORBIT DISTANCE FOR SHIELDS
    float Offset = 1.4f;

    // LIST TO TRACK ALL SPAWNED SHIELDS
    List<ThingInfo> Lv3_spawnedShields = new List<ThingInfo>();

    // LIST TO TRACK ONLY RED (VULNERABLE) SHIELDS
    List<ThingInfo> redShields = new List<ThingInfo>();

    // REFERENCE TO THE ENEMY RIGIDBODY
    private Rigidbody2D EnemyRb;

    // ROTATION SPEED OF ORBITING SHIELDS
    float RotateSpeed = 120f;

    // CONSTRUCTOR FOR THE BARRIER SHIELD ACTION
    public Lv3BarrierShield(ThingInfo who, EventInfo e = null)
    {
        // SETUP THE ACTION WITH CORRECT PARAMETERS
        Setup(Actions.Lv3_BarrierShield_JuliusP, who);
    }

    // RUN WHEN ACTION STARTS
    public override void Begin()
    {
        base.Begin();

        // ADD IMMUNITY TRAIT TO THE ENEMY IF IT DOESN'T HAVE IT
        if (!Who.Has(Traits.IgnoreDamage_JuliusP))
        {
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);
        }

        // GET RIGIDBODY COMPONENT OF ENEMY FOR MOVEMENT
        EnemyRb = Who.Thing.GetComponent<Rigidbody2D>();
        if (EnemyRb != null)
        {
            EnemyRb.simulated = true;
        }

        // SPAWN THE SHIELDS AROUND THE ENEMY
        SpawnShields();
    }

    // RUN EVERY FRAME WHILE ACTION IS ACTIVE
    public override void OnRun()
    {
        base.OnRun();

        // REMOVE DESTROYED SHIELDS FROM TRACKING LISTS
        for (int i = Lv3_spawnedShields.Count - 1; i >= 0; i--)
        {
            ThingInfo shield = Lv3_spawnedShields[i];
            if (shield == null || shield.Get(Traits.Health)?.GetN() <= 0)
            {
                Lv3_spawnedShields.RemoveAt(i);
                redShields.Remove(shield);
            }
        }

        // GET PLAYER'S CURRENT WEAPON
        ThingInfo currentWeapon = God.Session.Player.CurrentHeld;

        // DYNAMICALLY UPDATE RED SHIELDS HEALTH
        foreach (ThingInfo shield in redShields)
        {
            if (shield != null)
            {

                //STORES THE HEALTH TRAIT IN THE EVENT//
                EventInfo healthEvent = shield.Get(Traits.Health);
                
                if (healthEvent != null)
                {
                    if (currentWeapon != null && currentWeapon.GetName(true).Contains("Lv1.Bow"))
                    {
                        // VULNERABLE TO BOW
                        healthEvent.Set(NumInfo.Default, 1f);
                    }
                    else
                    {
                        // IMMUNE TO OTHER WEAPONS LIKE SWORD DAMAGE//
                        healthEvent.Set(NumInfo.Default, Mathf.Infinity);
                    }
                }
            }
        }

        // COUNT ALIVE RED SHIELDS
        int redShieldRemaining = 0;
        foreach (ThingInfo shield in redShields)
        {
            if (shield != null && shield.Get(Traits.Health)?.GetN() > 0)
            {
               redShieldRemaining++;
            }
        }

        // IF BOTH RED SHIELDS ARE DESTROYED, DESTROY ALL SHIELDS AND END ACTION
        if (redShieldRemaining == 0)
        {
            foreach (ThingInfo shield in Lv3_spawnedShields)
            {
                if (shield != null)
                {
                    shield.Destruct(shield);
                }
            }

            Lv3_spawnedShields.Clear();
            redShields.Clear();
            End();
            return;
        }

        // GET NUMBER OF SHIELDS TO ORBIT
        int shieldCount = Lv3_spawnedShields.Count;

        // ORBIT AND PULSE SHIELDS AROUND ENEMY
        if (shieldCount > 0)
        {
            // CALCULATE ANGLE DISTANCE BETWEEN SHIELDS
            float angleStep = 360f / shieldCount;

            // THIS IS FOR THE PULSING EFFECT
            float pulse = Mathf.Sin(Time.time * 2f) * 0.5f + Offset;

            // LOOP THROUGH ALL SHIELDS AND SET THEIR POSITION
            for (int i = 0; i < shieldCount; i++)
            {
                ThingInfo shield = Lv3_spawnedShields[i];
                if (shield == null || shield.Thing == null)
                {
                    continue;
                }

                // CALCULATE CURRENT ANGLE FOR THIS SHIELD
                float angle = (i * angleStep + Time.time * RotateSpeed) * Mathf.Deg2Rad;

                // CALCULATE POSITION OFFSET USING COS AND SIN
                Vector3 ShieldOffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * pulse;

                // SET SHIELD POSITION RELATIVE TO ENEMY
                shield.Thing.transform.position = Who.Thing.transform.position + ShieldOffset;
            }
        }

        // MAKES THE THING CHASE THE PLAYER SLOWLY//
         ThingInfo targ = God.Session.Player;

        if (targ != null && targ.Thing != null)
        {
            Vector3 dir = (targ.Thing.transform.position - Who.Thing.transform.position).normalized;

            float speed = 13f;

           
            if (EnemyRb != null)
            {
             EnemyRb.MovePosition(Who.Thing.transform.position + dir * speed * Time.deltaTime);
            }
           
            else
            {
             Who.Thing.transform.position += dir * speed * Time.deltaTime;
            }

            
             Who.Thing.LookAt(targ);
        
        }
    }

    // RUN WHEN ACTION ENDS
    public override void End()
    {
        base.End();

        // REMOVES IMMUNITY TRAIT, IF THE THING HAS IT
        if (Who.Has(Traits.IgnoreDamage_JuliusP))
        {
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);
        }

        // DESTROY ALL REMAINING SHIELDS
        foreach (ThingInfo shield in Lv3_spawnedShields)
        {
            if (shield != null)
            {
                shield.Destruct(shield);
            }
        }

        // CLEAR SHIELD LISTS
        Lv3_spawnedShields.Clear();
        redShields.Clear();

        // START NEXT ACTION
        Actions next = NextAction();
        Who.Thing.DoAction(next);
    }

    // FUNCTION TO SPAWN SHIELDS
    void SpawnShields()
    {
        // LOAD THE SHIELD THINGOPTION IN THE FOLDER IT'S IN//
        ThingOption Shield = Resources.Load<ThingOption>("JuliusP/Things With Actions/BarrierShield");

        // GET THE PLAYER THINGINFO//
        ThingInfo Player = God.Session.Player;

        // THE NUMBER OF SHILEDS THAT SURROND THE THING//
        int numberOfShields = 8;

        // PICK TWO RANDOM SHIELDS TO BE RED
        int randomIndexOne = Random.Range(0, numberOfShields);
        int randomIndexTwo;
        int randomIndexThree;
        do
        {
            // MAKES SURE THE SAME SHIELD ISN'T CHOSEN
            randomIndexTwo = Random.Range(0, numberOfShields);
            randomIndexThree = Random.Range(0, numberOfShields);
        } while (randomIndexTwo == randomIndexOne || randomIndexThree == randomIndexTwo || randomIndexThree == randomIndexOne);


        //EACH SHIELD WILL BE PLACED 45 DEGREES APART
        float angleStep = 360f / numberOfShields;
        // SPAWN ALL SHIELDS
        for (int i = 0; i < numberOfShields; i++)
        {
            // CALCULATE THE OFFSET FOR THE SHIELDS//
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 ShieldOffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;
            Vector3 spawnPos = Who.Thing.transform.position + ShieldOffset;

            // CREATE THE SHIELD OBJECT
            ThingInfo shieldInfo = Shield.Create();
            ThingController shieldController = shieldInfo.Spawn(spawnPos);
            shieldController.transform.parent = null;

            // CREATE HEALTH EVENT
            EventInfo hp = new EventInfo();

            // CHECK IF THIS SHIELD IS RED
            if (i == randomIndexOne || i == randomIndexTwo || i == randomIndexThree)
            {

                //SET HEALTH TO BE 1 ONLY FOR THE RED SHIELDS//
                hp.Set(NumInfo.Default, 1f); 
                redShields.Add(shieldInfo);

                // SET COLOR TO RED FOR THE ONES CHOSEN TO BE THE RED SHIELD//
                SpriteRenderer sr = shieldController.GetComponentInChildren<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = Color.red;
                }
            }
            else
            {
                // NORMAL SHIELDS ARE IMMUNE AND TAKE NO DAMAGE//
                hp.Set(NumInfo.Default, Mathf.Infinity);
            }

            // APPLY HEALTH TRAIT
            shieldInfo.AddTrait(Traits.Health, hp);

            // ADD THE SHIELDS TO SPAWNED SHIELDS LIST
            Lv3_spawnedShields.Add(shieldInfo);
        }
    }
}