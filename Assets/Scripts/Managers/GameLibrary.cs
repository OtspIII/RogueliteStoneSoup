using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameLibrary : MonoBehaviour
{
    public ThingController ActorPrefab;
    public BodyController PickupDefaultBody;
    private List<RoomOption> RoomOptions = new List<RoomOption>();
    private List<ThingOption> ThingOptions = new List<ThingOption>();

    private void Awake()
    {
        God.Library = this;
        foreach (ThingOption o in Resources.LoadAll<ThingOption>("/"))
            ThingOptions.Add(o);
        foreach (RoomOption o in Resources.LoadAll<RoomOption>("Rooms/"))
            RoomOptions.Add(o);
    }

    public void Setup()
    {
        
    }

    
    public RoomOption GetRoom(GeoTile g,LevelBuilder b)
    {
        RoomTags t = RoomTags.Generic;
        if (g.Path == GeoTile.GeoTileTypes.PlayerStart) t = RoomTags.PlayerStart;
        else if (g.Path == GeoTile.GeoTileTypes.Exit) t = RoomTags.Exit;
        List<RoomOption> opts = new List<RoomOption>();
        foreach (RoomOption rs in RoomOptions)
        {
            if(rs.Tags.Contains(t)) opts.Add(rs);
        }

        if (opts.Count == 0)
        {
            Debug.Log("NO VALID ROOMS: " + t + " / " + g + " / + b");
            return null;
        }
        return opts.Random();
    }

    public ThingOption GetThing(params GameTags[] tags)
    {
        return GetThing(new SpawnRequest(tags));
    }
    
    public ThingOption GetThing(SpawnRequest sr,LevelBuilder b=null)
    {
        List<ThingOption> opts = new List<ThingOption>();
        foreach (ThingOption o in ThingOptions)
        {
            if(sr.Judge(o) > 0) opts.Add(o);
            // Debug.Log("THING: " + sr.Judge(o) + " / " + o.Name);
        }

        if (opts.Count == 0)
        {
            Debug.Log("NO VALID ROOMS: " + sr + " / " + b + " / " + ThingOptions.Count);
            return null;
        }
        return opts.Random();
    }
}
