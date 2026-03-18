using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BleakWatcher : ActionScript
{
    
    bool CanOnlyspawnOne = false;

    bool DetectedHit = false;


    



    public BleakWatcher(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.BleakWatcher_JuliusP, who, true);
        HaltMomentum = true;
        Duration = Mathf.Infinity;
    }

    public override void OnRun()
    {
        if (Input.GetKeyDown(KeyCode.B) && !CanOnlyspawnOne)
        {
            SpawnBleakWatcherTurret();
            CanOnlyspawnOne = true;
      
        }
    }

public override void HitBegin(GameCollision col)
{
    Debug.Log("Hi!!!!");


    
}
    
    void SpawnBleakWatcherTurret()
    {
    //LOAD THE TURRET FORM THE FOLDER//

    ThingOption Turret = Resources.Load<ThingOption>("JuliusP/ThingOptions/Bleak Turret");


    ThingInfo Turretinfo = new ThingInfo(Turret);

   

    //SPAWN POS OF TURRET//
    Vector3 SpawnPos = Who.Thing.transform.position + Who.Thing.transform.up * 1.5f;


    ThingController TurretController = Turretinfo.Spawn(SpawnPos);


    //ADD A RIGIDBODY TO IT FOR THROWING//
    TurretController.AddRB();


    //THROW FORCE//
    float ThrowForce = 3f;


    TurretController.TakeKnockback(Who.Thing, ThrowForce);


    // SHOTOT AFTER LANDING//
    God.C(TurretShootRoutine(TurretController));


    // TURRET LASTS FOR 10 SECONDS//
    God.C(DestroyTurret(Turretinfo, 9f));

}


IEnumerator TurretShootRoutine(ThingController turret)
{
    ThingOption projectile = Resources.Load<ThingOption>("JuliusP/ThingOptions/Crystals");

    while (turret != null) // keep shooting while turret exists
    {
        // Shoot 3 projectiles in a burst
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPos = turret.transform.position + turret.transform.up * 1f;

            ThingInfo projectileInfo = new ThingInfo(projectile);
            ThingController projController = projectileInfo.Spawn(spawnPos);

            //SLOWS ON HIT//
            projectileInfo.AddTrait(Traits.SlowOnhit_JuliusP);

            //PROJECTILE LASTS FOR 2 SECONDS//
            God.C(DestroyTurret(projectileInfo, 2f)); 
            
            
            // ADD RIGIDBODY TO STASIS PROJECTILE//
            projController.AddRB();
            

            yield return new WaitForSeconds(5f); // wait 2 seconds before next projectile
        }

        // Optional: pause before next burst
        yield return new WaitForSeconds(1f);
    }
}


//Coroutine to destroy a projectile safely

    // Safely destroys a ThingInfo after a duration
    IEnumerator DestroyTurret(ThingInfo thing, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (thing != null && thing.Thing != null)
        {
            thing.DestroyForm(); // destroys the GameObject and clears reference
            Debug.Log("Destroyed " + thing + " after " + duration + " seconds");

       
        }
    }
}



public class SlowingProjectileTrait : Trait
{
    public SlowingProjectileTrait()
    {
        Type = Traits.SlowOnhit_JuliusP;

        AddListen(EventTypes.OnTouch);
        AddListen(EventTypes.Update); 
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                // GET THE THINGINFO OF THE STASIS PROJECTILE//
                ThingInfo projectile = i.Who;

                // GET THINGCONTROLLER OF STATSIS CRYSTAL//
                ThingController projController = projectile.Thing;

                // DETECTION RADIUS//
                float range = 2f;

                // SPEED OF THE PROJECTILE//
                float speed = 5f;

                // SETS HOW FAST THE CYSTAL CAN TRACK OR TURN//
                float turnSpeed = 5f;

                // GET ALL THINGINFOS IN THE RADIUS OF THE STASIS PROJECTILE//
                var hits = God.GM.CollideCircle(projController.transform.position, range);

                // VARIABLE TO STORE THE CLOSEST TARGET FOUND
                ThingInfo closest = null;

                // VARIABLE TO STORE THE DISTANCE TO THE CLOSEST TARGET (STARTS REALLY HIGH)
                float closestDist = Mathf.Infinity;

                // LOOP THROUGH EVERY THING FOUND IN THE COLLISION RADIUS
                foreach (var t in hits)
                {
                    // IGNORE THE STASIS CRYSTAL//
                    if (t == projectile) continue;

                    // IGNORE TEAMMATES / ALLIES
                    if (t.Team == projectile.Team) continue;

                    // CALCULATE DISTANCE FROM THE PROJECTILE TO THIS THING
                    float dist = Vector2.Distance(projController.transform.position,t.Thing.transform.position);

                    // IF THIS DISTANCE IS CLOSER THAN ANYTHING FOUND BEFORE, SAVE IT
                    if (dist < closestDist)
                    {
                        //THIS IS THE CLOSEST TARGET
                        closest = t; 

                        //THIS SAVES THE CLOSEST DISTANCE//     
                        closestDist = dist; 
                    }
    
                 }

                   //IF THE CLOSEST EXISTS AND IS A THING//
                    if (closest != null)
                    {
                         // CALCULATE THE NORMALIZED DIRECTION VECTOR FROM PROJECTILE TO TARGET
                        Vector2 dir = (closest.Thing.transform.position - projController.transform.position).normalized;

                         // SMOOTHLY ADJUST THE PROJECTILE'S VELOCITY TOWARD THE TARGET
                        // THIS LERP CAUSES THE PROJECTILE TO CURVE TOWARD THE TARGET
                         projController.ActualMove = Vector2.Lerp(projController.ActualMove, dir * speed,Time.deltaTime * turnSpeed);

                        // ROTATE THE PROJECTILE TO FACE THE DIRECTION IT IS MOVING
                        projController.transform.up = projController.ActualMove.normalized;
                    }

                     break; 
            }

            case EventTypes.OnTouch:
            {
                GameCollision col = e.Collision;
                ThingInfo target = col.Other.Info;

                if (target != null && !target.Has(Traits.Slowed_JuliusP))
                {
                    target.AddTrait(Traits.Slowed_JuliusP);
                    Debug.Log("Slowed applied to: " + target);
                }

                i.Who.Destruct(i.Who);
                break;
            }
        }
    }
}