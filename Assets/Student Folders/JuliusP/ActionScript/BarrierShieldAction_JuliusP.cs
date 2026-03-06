using UnityEngine;
using System.Collections.Generic;

public class BarrierShieldAction_JuliusP : ActionScript
{
    // THIS SETS HOW FAR THE SHIELDS ARE//
    float Offset = 1.4f;

    // THIS CREATES A LIST TO KEEP TRACK OF ALL SHIELDS SPAWNED
    List<ThingInfo> spawnedShields = new List<ThingInfo>();

    private Rigidbody2D EnemyRb;

    ThingInfo redShield;
    float RotateSpeed = 120f;

    // CONSTRUCTOR TAKES CHARACTER AND OPTIONAL EVENT
    public BarrierShieldAction_JuliusP(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.BarrierShield_JuliusP, who);

    
     
    }

    // CALLED WHEN THE ACTION STARTS
    public override void Begin()
    {
        base.Begin();

        // CHECK IF CHARACTER DOES NOT HAVE DAMAGE IMMUNITY
        if (!Who.Has(Traits.IgnoreDamage_JuliusP))
            // ADDS THW DAMAGE IMMUNITY TRAIT
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);

        // MAKES THE SHIELDS SPAWN AT THE BEGINNING OF THE ACTION//
        SpawnShields();

        EnemyRb = Who.Thing.GetComponent<Rigidbody2D>();

        EnemyRb.simulated = true;


    }

    // CAlLS EVERY FRAME SIMILAR TO UPDATE//
    public override void OnRun()
    {
        base.OnRun();

        // IF THE RED SHIELD IS DESTROYED, DESTROY ALL SHIELDS
        if (redShield == null || redShield.Get(Traits.Health) == null || redShield.Get(Traits.Health).GetN() <= 0)
        {
            foreach (ThingInfo shield in spawnedShields)
                if (shield != null) shield.Destruct(shield);

            spawnedShields.Clear();
            End();
            return;
        }

        // LOOP THROUGH ALL SHIELDS BACKWARDS
        for (int i = spawnedShields.Count - 1; i >= 0; i--)
        {
            // GET CURRENT SHIELD
            ThingInfo shield = spawnedShields[i];

            // CHECK IF SHIELD IS DESTROYED OR DEAD
            if (shield == null || shield.Get(Traits.Health) == null || shield.Get(Traits.Health).GetN() <= 0)
            {
                // REMOVE SHIELD FROM LIST
                spawnedShields.RemoveAt(i);
            }
        }

        // CHECK IF ALL SHIELDS HAVE BEEN DESTROYED
        bool allShieldsGone = spawnedShields.Count == 0;

        // IF NO SHIELDS ARE LEFT
        if (allShieldsGone)
        {
            //ENDS THE ACTION//
            End();
            return;
        }

        // ORBIT SHIELDS AROUND CHARACTER
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


         // MOVE TOWARDS PLAYER SLOWLY
         ThingInfo targ = God.Session.Player;
        if (targ != null)
        {
         // Calculate direction toward the player
         Vector3 dir = (targ.Thing.transform.position - Who.Thing.transform.position).normalized;

        // Set speed (slow movement)
         float speed = 13f; // adjust this for slower/faster movement

        // Move the enemy using Rigidbody2D if present
         if (EnemyRb != null)
         {
         EnemyRb.MovePosition(Who.Thing.transform.position + dir * speed * Time.deltaTime);
         }
        
         else
         {
         // fallback: move via transform
         Who.Thing.transform.position += dir * speed * Time.deltaTime;
         }

         // Make the enemy face the player
         Who.Thing.LookAt(targ);
}

        
    }

    // FUNCTION TO END THE ACTION//
    public override void End()
    {
        base.End();

        // CHECK IF THE THING HAS THE IMMUNE DAMAGE TRAIT//
        if (Who.Has(Traits.IgnoreDamage_JuliusP))
            // REMOVE THE DAMAGE IMMUNITY TRAIT TO ALLOW FOR DAMAGE
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);

        // LOOP THROUGH ANY LEFTOVER SHIELDS
        foreach (ThingInfo shield in spawnedShields)
        {
            // CHECK IF SHIELD EXISTS
            if (shield != null)
                // DESTROY SHIELD
                shield.Destruct(shield);
        }

        // CLEAR THE SHIELDS LIST
        spawnedShields.Clear();

        // GET THE NEXT ACTION (DEFAULT OR CHASE)
        Actions next = NextAction();

        // STARTS THE NEXT ACTION
        Who.Thing.DoAction(next);
    }

    // FUNCTION TO SPAWN SHIELDS AROUND CHARACTER
    void SpawnShields()
    {
        // FINDS AND LOADS THE BARRIER SHIELD IN THE FOLDER IT'S IN//
        ThingOption Shield = Resources.Load<ThingOption>("JuliusP/Things With Actions/BarrierShield");

        // MAKE 8 SHIELDS SPAWN AROUND THE THING LIKE A PROTECTIVE BARRIER//
        int numberOfShields = 8;

        // THIS MAKES EACH SHIELD 45 DEGREES APART//
        float angleStep = 360f / numberOfShields;

        // LOOP THROUGH EACH SHIELD
        for (int i = 0; i < numberOfShields; i++)
        {

            // CALCULATES THE ANGLE IN RADIANS:

            // IF I IS 0 -> ANGLE IS 0//

            // IF I IS 1 -> ANGLE IS 45//

            // IF I IS 2 -> ANGLE IS 90//
            float angle = i * angleStep * Mathf.Deg2Rad;

            // CALCULATES THE OFFSET USING COS FOR X, SIN FOR Y -> UNIT CIRCLE//
            Vector3 Shieldoffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

            // GET FINAL SPAWN POSITION RELATIVE TO CHARACTER//
            Vector3 spawnPos = Who.Thing.transform.position + Shieldoffset;

            // THIS LINE CREATES A NEW SHIELD OBJECT//
            ThingInfo shieldInfo = new ThingInfo(Shield);

            // SPAWNS A SHIELD AT THE POSITION//
            ThingController shieldController = shieldInfo.Spawn(spawnPos);

            // MAKES THE SHIELD ATTACHED TO THE THING//
            shieldController.transform.parent = null;

            // CREATES A NEW HEALTH EVENT//
            EventInfo hp = new EventInfo();

            if (i == 0)
            {
                // SETS THE HIELD HEALTH TO 1//
                hp.Set(NumInfo.Default, 1);
                redShield = shieldInfo;

                SpriteRenderer sr = shieldController.GetComponentInChildren<SpriteRenderer>();
                if (sr != null) sr.color = Color.red;
            }
            else
            {
                // SETS THE HIELD HEALTH TO 20//
                hp.Set(NumInfo.Default, Mathf.Infinity);
            }

            // ADDS THE HEALTH TRAIT TO SHIELD//
            shieldInfo.AddTrait(Traits.Health, hp);

            // ADDS THE SHIELD TO THE LIST TO TRACK//
            spawnedShields.Add(shieldInfo);
        }
    }
}

//FOR LEVEL2 VERSION OF THE ACTION//

public class Lv2_BarrierShield_JuliusP : ActionScript
{
    // Shield orbit distance
    float Offset = 1.4f;

    // Track all shields spawned
    List<ThingInfo> Lv2_spawnedShields = new List<ThingInfo>();

    // Track red shields
    List<ThingInfo> redShields = new List<ThingInfo>();

    private Rigidbody2D EnemyRb;

    float RotateSpeed = 120f;

    // Constructor
    public Lv2_BarrierShield_JuliusP(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Lv2_BarrierShield_JuliusP, who);
    }

    public override void Begin()
    {
        base.Begin();

        // Give damage immunity if not already present
        if (!Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);

        // Spawn shields at the start
        SpawnShields();

        EnemyRb = Who.Thing.GetComponent<Rigidbody2D>();
        EnemyRb.simulated = true;
        
    }

    public override void OnRun()
    {
        base.OnRun();

        // Remove destroyed shields from the lists
        for (int i = Lv2_spawnedShields.Count - 1; i >= 0; i--)
        {
            ThingInfo shield = Lv2_spawnedShields[i];
            if (shield == null || shield.Get(Traits.Health)?.GetN() <= 0)
            {
                Lv2_spawnedShields.RemoveAt(i);
                redShields.Remove(shield); // Also remove from red shield list if it was one
            }
        }

        // End action if BOTH red shields are destroyed
        if (redShields.Count == 0)
        {
            foreach (ThingInfo shield in Lv2_spawnedShields)
                if (shield != null) shield.Destruct(shield);

            Lv2_spawnedShields.Clear();
            End();
            return;
        }

        // Orbit remaining shields around character
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

        // Move slowly towards player
        ThingInfo targ = God.Session.Player;
        if (targ != null)
        {
            Vector3 dir = (targ.Thing.transform.position - Who.Thing.transform.position).normalized;
            float speed = 13f;

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

        // Remove damage immunity
        if (Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);

        // Destroy any leftover shields
        foreach (ThingInfo shield in Lv2_spawnedShields)
            if (shield != null)
                shield.Destruct(shield);

        Lv2_spawnedShields.Clear();
        redShields.Clear();

        // Trigger next action
        Actions next = NextAction();
        Who.Thing.DoAction(next);
    }

    
    
    
 void SpawnShields()
{
    ThingOption Shield = Resources.Load<ThingOption>("JuliusP/Things With Actions/BarrierShield");
    ThingInfo Player = God.Session.Player;
    ThingInfo heldItem = Player.CurrentHeld;

    string heldName = "";
    if (heldItem != null)
        heldName = heldItem.GetName(true); // raw name

    int numberOfShields = 8;
    float angleStep = 360f / numberOfShields;

    int randomIndexOne = Random.Range(0, numberOfShields);
    int randomIndexTwo;
    do
    {
        randomIndexTwo = Random.Range(0, numberOfShields);
    } while (randomIndexTwo == randomIndexOne);

    for (int i = 0; i < numberOfShields; i++)
    {
        float angle = i * angleStep * Mathf.Deg2Rad;
        Vector3 Shieldoffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;
        Vector3 spawnPos = Who.Thing.transform.position + Shieldoffset;

        ThingInfo shieldInfo = new ThingInfo(Shield);
        ThingController shieldController = shieldInfo.Spawn(spawnPos);
        shieldController.transform.parent = null;

        EventInfo hp = new EventInfo();

        if (i == randomIndexOne || i == randomIndexTwo)
        {
            float redShieldHealth = 1f; // default weak

            if (!string.IsNullOrEmpty(heldName))
            {
                if (heldName.Contains("Sword"))
                    redShieldHealth = Mathf.Infinity; // immune to sword
                else if (heldName.Contains("Bow"))
                    redShieldHealth = 1f; // vulnerable to bow
            }

            hp.Set(NumInfo.Default, redShieldHealth);
            redShields.Add(shieldInfo);

            SpriteRenderer sr = shieldController.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.color = Color.red;
        }
        else
        {
            hp.Set(NumInfo.Default, Mathf.Infinity);
        }

        shieldInfo.AddTrait(Traits.Health, hp);
        Lv2_spawnedShields.Add(shieldInfo);
    }
}
}