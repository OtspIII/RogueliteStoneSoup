
using UnityEngine;
using System.Collections.Generic;

public class BarrierShieldAction_JuliusP : ActionScript
{
    // THIS SETS HOW FAR THE SHIELDS ARE//
    float Offset = 1.4f;

    // THIS CREATES A LIST TO KEEP TRACK OF ALL SHIELDS SPAWNED
    List<ThingInfo> spawnedShields = new List<ThingInfo>();

    // CONSTRUCTOR TAKES CHARACTER AND OPTIONAL EVENT
    public BarrierShieldAction_JuliusP(ThingInfo who, EventInfo e = null)
    {
        
        Setup(Actions.BarrierShield_JuliusP, who);

        // ALLOWS CHARACTER TO MOVE WHILE PERFORMING THIS ACTION
        HaltMomentum = false; 
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
    }

    // CAlLS EVERY FRAME SIMILAR TO UPDATE//
    public override void OnRun()
    {
      
        base.OnRun();

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
            // CALCULATES THE ANGLE IN RADIANS

            // IF I IS 0 -> ANGLE IS 0//

            // IF I IS 1 -> ANGLE IS 45//

            // IF I IS 2 -> ANGLE IS 90//

            // MAKES THE ANGLES 45 DEGREES APART WHEN I GETS INCREMENTED EACH TIME//
            float angle = i * angleStep * Mathf.Deg2Rad;

            // CALCULATES THE OFFSET USING COS FOR X, SIN FOR Y -> UNIT CIRCLE//
            //MATHF.COS(angle) → GIVES THE X-COORDINATE ON THE UNIT CIRCLE//
            //MATHF.SIN(angle) → GIVES THE Y-COORDINATES ON THE UNIT CIRCLE// 
            Vector3 Shieldoffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * Offset;

            // GET FINAL SPAWN POSITION RELATIVE TO CHARACTER//
            Vector3 spawnPos = Who.Thing.transform.position + Shieldoffset;

            // THIS LINE CREATES A NEW SHIELD OBJECT//
            ThingInfo shieldInfo = new ThingInfo(Shield);

            // SPAWNS A SHIELD AT THE POSITION//
            ThingController shieldController = shieldInfo.Spawn(spawnPos);

            // MAKES THE SHIELD ATTACHED TO THE THING//
            shieldController.transform.parent = Who.Thing.transform;

            // CREATES A NEW HEALTH EVENT//
            EventInfo hp = new EventInfo();

            // SETS THE HIELD HEALTH TO 2//
            hp.Set(NumInfo.Default, 2);

            // ADDS THE HEALTH TRAIT TO SHIELD//
            shieldInfo.AddTrait(Traits.Health, hp);

            // ADDS THE SHIELD TO THE LIST TO TRACK//
            spawnedShields.Add(shieldInfo);
        }
    }
}