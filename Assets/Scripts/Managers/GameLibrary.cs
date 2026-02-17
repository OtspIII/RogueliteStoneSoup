using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

///All the prefabs used by the game go here so we don't clog up other scripts with them. 
public class GameLibrary : MonoBehaviour
{
    //The base that all objects in our game use
    public ThingController ActorPrefab;
    //Equippable items have two bodies--one on the ground and one when held. Unless specified, they use this on the ground
    public BodyController PickupDefaultBody;
    //A generic gnome that gets customized when spawned
    public SfXGnome GnomePrefab;
    //A generic hitbox used by ThingController.AddHitbox
    public HitboxController Hitbox;
    //We don't need to load our Resources folder more than once, so let's have a bool that says once we've done it
    public static bool Setup = false;
    //A list of all the possible spawnable rooms/actors/gnomes the procgen system can pull from.
    //Static so we don't lose them on a level reload
    private static List<RoomOption> RoomOptions = new List<RoomOption>();
    private static List<ThingOption> ThingOptions = new List<ThingOption>();
    private static List<GnomeOption> GnomeOptions = new List<GnomeOption>();
    //I also sort the gnomes by name to make them easy to find
    private static Dictionary<string, GnomeOption> GnomeDict = new Dictionary<string, GnomeOption>();

    private void Awake()
    {
        //Register myself to a static variable for easy finding
        God.Library = this;
        //If we already loaded our Resources folder then we don't need to do any of the below
        if (Setup) return;
        Setup = true;
        //Read the Resources folder to find all the Options we've crafted for it to use when generating levels
        foreach (ThingOption o in Resources.LoadAll<ThingOption>("/"))
            ThingOptions.Add(o);
        foreach (RoomOption o in Resources.LoadAll<RoomOption>("/"))
            RoomOptions.Add(o);
        foreach (GnomeOption o in Resources.LoadAll<GnomeOption>("/"))
        {
            GnomeOptions.Add(o);
            if(!GnomeDict.ContainsKey(o.Name))
                GnomeDict.Add(o.Name,o);
            else
                God.LogWarning("Duplicate Gnome: " + o.Name);
        }
            
    }

    ///Finds an appropriate room to fill a GeoTile during level generation
    public RoomOption GetRoom(GeoTile g,LevelBuilder b)
    {
        //We're going to record how likely each room is to appear
        Dictionary<RoomOption, float> opts = new Dictionary<RoomOption, float>();
        //Look at each room option that exists. . .
        foreach (RoomOption rs in RoomOptions)
        {
            //Ask the level builder to judge how likely it is for the room to show up. 1 is 'normal'
            float w = b.JudgeRoom(g, rs);
            //If it has any chance, add it to the dictionary
            if(w > 0) opts.Add(rs,w);
        }
        //If we didn't find any valid options, throw a warning. You might need to make more room types
        if (opts.Count == 0)
        {
            God.LogWarning("NO VALID ROOMS: " + g);
            return null;
        }
        //Return a random option
        return WRandom(opts);
    }
    
    ///Finds an appropriate thing to spawn in response to a SpawnRequest
    public ThingOption GetThing(SpawnRequest sr,LevelBuilder b=null)
    {
        //There's an option on the GameManager to set a debug thing to spawn in the player's room
        //If we have something like that set, any spawner set to spawn 'Debug' will spawn that object
        //Otherwise, nothing will happen
        if (sr.HasTag(GameTags.Debug))
        {
            return God.GM.DebugSpawn != null ? God.GM.DebugSpawn : null;
        }
        //Make a list of all the things that *might* spawn here
        Dictionary<ThingOption, float> opts = new Dictionary<ThingOption, float>();
        //If we didn't set the level builder to use, just use the one that built the current level
        if (b == null) b = God.LB;
        //Look at all the objects in the game
        foreach (ThingOption o in ThingOptions)
        {
            //Ask the level builder how much it likes each option as a possible thing to spawn
            float w = b.JudgeThing(sr,o);
            //If it likes it at all, add it (and how much it liked it) to the list
            if(w > 0) opts.Add(o,w);
        }
        //If we get zero results, do a fallback where we don't care about the author.
        //Maybe you just don't have anything with this tag yet
        if (opts.Keys.Count == 0)
        {
            foreach (ThingOption o in ThingOptions)
            {
                //By setting this override true it'll ignore authors of Options
                float w = b.JudgeThing(sr,o,true);
                if(w > 0) opts.Add(o,w);
            }
        }
        //If there was nothing that could be spawned, throw a warning. You might need to make more things!
        if (opts.Keys.Count == 0)
        {
            God.LogWarning("NO VALID THINGS: " + sr);
            return null;
        }
        //Return a random option, with more liked options being more likely
        return WRandom(opts);
    }
    
    ///Finds the gnome with the given name and returns it. Nothing too complicated.
    public GnomeOption GetGnome(string g)
    {
        if (GnomeDict.TryGetValue(g, out GnomeOption r)) return r;
        return null;
    }
    
    ///Takes a dictionary of options with a 'weight' and returns one random--with 'heavier' ones more likely
    public T WRandom<T>(Dictionary<T, float> opts) where T:GameOption
    {
        //We're going to add up the weights of all possible options using this float
        float total = 0;
        //Look at each option and add its weight to 'total'
        foreach (float v in opts.Values) total += v;
        //We then generate a random number with a value less than that total
        //So if we had three options of weight 1 each, it'd generate a number between 0-3
        float roll = Random.Range(0, total);
        //Go through each option and subtract its weight from the roll
        //If this brings it below 0, select it!
        //This means that bigger weights are more likely to be selected
        foreach (T k in opts.Keys)
        {
            roll -= opts[k];
            if (roll <= 0) return k;
        }
        //This should never happen. Things that should never happen often happen. That's why it's good to put a warning on it.
        God.LogWarning("Weighted Random With No Result Somehow: " + opts.Keys.Count);
        return null;
    }
}
