using System.Collections.Generic;
using UnityEngine;

//Note:
//Default = 0 : random radius teleport 
//Distance : teleport radius inside unit circle
//Default = 1 : random room teleport 
//Distance : scattered unit inside destination room
//Zone only 
//Speed : how often pad triggers
public class TeleportTrait : Trait
{
    public TeleportTrait()
    {
        Type = Traits.Teleport;
        AddListen(EventTypes.OnUse);
        AddListen(EventTypes.OnTouch);
        AddListen(EventTypes.OnInside);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //Held item teleport 
            case EventTypes.OnUse:
                {
                    ThingInfo t = e.GetThing();

                    //Use the helper function to calculate the teleport destination
                    Vector2 dest = GetTeleportDestination(i, t.Thing.transform.position);
                    //Move Thing to new position
                    t.Thing.transform.position = dest;
                    return;
                }

            //Zone touch teleport
            case EventTypes.OnTouch:
                {
                    //Check collision data
                    GameCollision col = e.Collision;

                    //Check for Things that touched
                    ThingInfo t = col.Other.Info;

                    //Use the teleport zone position as center
                    Vector2 center = i.Who.Thing.transform.position;
                    //Calculate the teleport destination
                    Vector2 dest = GetTeleportDestination(i, center);
                    //Move Thing to new position
                    t.Thing.transform.position = dest;
                    return;
                }

            //Teleport pad
            case EventTypes.OnInside:
                {
                    //Cooldown before trigger 
                    float timer = i.Get(NumInfo.Speed, 1);
                    //Find hitbox
                    HitboxController hb = e.GetHitbox();

                    //Find Thing location (aka teleporter) 
                    Vector2 center = i.Who.Thing.transform.position;

                    //Loop stuff inside hitbox
                    foreach (ThingController tc in hb.Touching.ToArray())
                    {
                        ThingInfo t = tc.Info;

                        //Calculate the teleport destination
                        Vector2 dest = GetTeleportDestination(i, center);
                        //Move Thing to new position
                        t.Thing.transform.position = dest;
                    }

                    //Reset hitbox timer 
                    hb.Timer = timer;
                    return;
                }

            default: return;
        }
    }

    //The destination calculator 
    Vector2 GetTeleportDestination(TraitInfo i, Vector2 center)
    {
        //Mode selection
        int mode = i.GetInt(NumInfo.Default, 0);
        //Distance used for radius and scatter amount 
        float dist = i.Get(NumInfo.Distance, 4);

        switch (mode)
        {
            //Random radius
            default:
            case 0:
                {
                    //Pick random point inside a circle 
                    return center + Random.insideUnitCircle * dist;
                }

            //Nearby room
            case 1:
                {
                    //Assumes the first room is the closest
                    RoomScript closest = God.GM.Rooms[0];

                    //Calculate its distance
                    float best = ((Vector2)closest.transform.position - center).sqrMagnitude;

                    //Search remaining rooms for something closer
                    for (int n = 1; n < God.GM.Rooms.Count; n++)
                    {
                        RoomScript r = God.GM.Rooms[n];

                        //Calculate how far the teleporter is from each room center
                        float d = ((Vector2)r.transform.position - center).sqrMagnitude;

                        //Find closest room
                        if (d < best)
                        {
                            best = d;
                            closest = r;
                        }
                    }

                    //Prefer room connected by doors 
                    List<GeoTile> neighbors = closest.Geo.Neighbors(true);

                    //Pick a random neighboring room
                    GeoTile next = neighbors[Random.Range(0, neighbors.Count)];

                    //Get the room center
                    Vector2 where = next.Room.transform.position;

                    //Scatter the landing point slightly
                    where += Random.insideUnitCircle * dist;

                    return where;
                }
        }
    }
}