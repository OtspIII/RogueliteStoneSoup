using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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

    // public ThingOption GetThing(LevelBuilder b,params GameTags[] tags)
    // {
    //     // Debug.Log("GT1: " + tags);
    //     return GetThing(new SpawnRequest(tags));
    // }
    
    public ThingOption GetThing(SpawnRequest sr)
    {
        if (sr.HasTag(GameTags.Something) && God.GM.DebugSpawn != null)
        {
            return God.GM.DebugSpawn;
        }
        // Debug.Log("GT2: " + sr);
        Dictionary<ThingOption, float> opts = new Dictionary<ThingOption, float>();
        foreach (ThingOption o in ThingOptions)
        {
            float w = sr.Judge(o);
            if(w > 0) opts.Add(o,w);
            // Debug.Log("THING: " + sr.Judge(o) + " / " + o.Name);
        }

        if (opts.Keys.Count == 0)
        {
            Debug.Log("NO VALID THINGS: " + sr + " / " + God.LB + " / " + ThingOptions.Count);
            return null;
        }
        return WRandom(opts);
    }
    
    public ThingOption WRandom(Dictionary<ThingOption, float> opts)
    {
        float total = 0;
        foreach (float v in opts.Values) total += v;
        float roll = Random.Range(0, total);
        foreach (ThingOption k in opts.Keys)
        {
            roll -= opts[k];
            if (roll <= 0) return k;
        }
        Debug.Log("Weighted Random With No Result Somehow: " + opts.Keys.Count);
        return null;
    }
}
