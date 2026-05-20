using UnityEngine;

public class Lv3Homing : Trait
{
    public Lv3Homing()
    {
        Type = Traits.Lv3Homing_JuliusP;

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

                // SAFETY CHECK: make sure projectile exists
                if (projController == null || projController.transform == null) return;

                // DETECTION RADIUS//
                float range = 2f;

                // SPEED OF THE PROJECTILE//
                float speed = 5f;

                // SETS HOW FAST THE CYSTAL CAN TRACK OR TURN//
                float turnSpeed = 24f;

                // GET ALL THINGINFOS IN THE RADIUS OF THE STASIS PROJECTILE//
                var hits = God.GM.CollideCircle(projController.transform.position, range);

                // VARIABLE TO STORE THE CLOSEST TARGET FOUND
                ThingInfo closest = null;

                // VARIABLE TO STORE THE DISTANCE TO THE CLOSEST TARGET (STARTS REALLY HIGH)
                float closestDist = Mathf.Infinity;

                // LOOP THROUGH EVERY THING FOUND IN THE COLLISION RADIUS
                foreach (var t in hits)
                {
                    // SAFETY CHECK: skip null or destroyed objects
                    if (t == null || t.Thing == null) continue;

                    // IGNORE THE STASIS CRYSTAL//
                    if (t == projectile) continue;

                    // IGNORE TEAMMATES / ALLIES
                    if (t.Team == projectile.Team) continue;

                    //IGNORE NEUTRAL TEAM//
                    if (t.Team == GameTeams.Neutral) continue;

                    // ONLY TARGET ENEMIES (must have health / be valid combat target)
                    if (t.Has(Traits.Health) == false) continue;

                    // CALCULATE DISTANCE FROM THE PROJECTILE TO THIS THING
                    float dist = Vector2.Distance(projController.transform.position, t.Thing.transform.position);

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
                if (closest != null && closest.Thing != null)
                {
                    // CALCULATE THE NORMALIZED DIRECTION VECTOR FROM PROJECTILE TO TARGET
                    Vector2 dir = (closest.Thing.transform.position - projController.transform.position).normalized;

                    // SMOOTHLY ADJUST THE PROJECTILE'S VELOCITY TOWARD THE TARGET
                    projController.ActualMove = Vector2.Lerp(projController.ActualMove, dir * speed, Time.deltaTime * turnSpeed);

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
                    // target.AddTrait(Traits.Slowed_JuliusP);
                    //Debug.Log("Slowed applied to: " + target);
                }


                i.Who.Destruct(i.Who);
                break;
            }
        }
    }
}

//CAN BE USED FOR ARROW OR PROJECTILE LIFETIME//


public class SelfDestructV2 : Trait
{
    public SelfDestructV2()
    {
        Type = Traits.SelfDestruct2_JuliusP;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                if (i.Who == null || i.Who.Thing == null) return;

                // GET TIMER (NO Has CHECK)
                float time = i.Get(NumInfo.Default);

                // IF NOT INITIALIZED, SET IT
                if (time <= 0f)
                {
                    time = 2.1f;
                }

                // COUNTDOWN
                time -= Time.deltaTime;
                i.Set(NumInfo.Default, time);

                // DESTROY WHEN DONE
                if (time <= 0f)
                {
                    i.Who.Destruct(i.Who);
                    return;
                }

                break;
            }
        }
    }
}