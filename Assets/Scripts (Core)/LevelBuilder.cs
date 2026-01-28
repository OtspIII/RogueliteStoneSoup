using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBuilder
{
    //These are variables meant to be set in Customize as desired
    //How big should the level be?
    public Vector2Int Size;
    //What is the % chance that any two adjacent rooms that don't need to be are connected
    public float LinkOdds = 0.1f;
    
    //And here are the variables that are just bookkeeping for the level dev process
    //A grid of tiles, lets you find a tile by its x/y coordinate
    public Dictionary<int, Dictionary<int, GeoTile>> GeoMap = new Dictionary<int, Dictionary<int, GeoTile>>();
    //A list of all the tiles, in case we just want to iterate through them
    public List<GeoTile> AllGeo = new List<GeoTile>();
    //What tile does the player spawn at?
    public GeoTile PlayerSpawn;
    //What tile does the exit spawn at?
    public GeoTile Exit;
    //A list of all the spawn points in the game you can spawn stuff at
    public List<SpawnPointController> SpawnPoints = new List<SpawnPointController>();
    //A list of all the spawn points that just want to spawn their own thing
    public List<SpawnPointController> SpawnPointsFixed = new List<SpawnPointController>();
    //A list of all the types of things we want to spawn somewhere in the level
    public List<SpawnRequest> SpawnRequests = new List<SpawnRequest>();
    //A list of all the specific things we want to spawn in the level
    public List<ThingOption> ToSpawn = new List<ThingOption>();
    //If a SpawnPoint can spawn "Something" it'll be okay for all of these options 
    public List<string> Somethings = new List<string>(){"NPC","Weapon","Consumable","ScoreThing"};

    public virtual void Build()
    {
        God.LB = this;
        //Does the specific level we're on change any basic info (Size/LinkOdds/etc)? Do that here
        Customize();
        //Build out a zoomed-out map of the level, without specific rooms
        BuildGeoMap();
        //Connect geomorphs to each other to make sure there's a path from player spawn to exit
        BuildMainPath();
        //Open more connections between geomorphs to make sure all are accessable
        ConnectAllGeos();
        //Pick a room from the list of options for each geomorph
        PickRooms();
        //Actually spawn the rooms
        BuildRooms();
        //Decide how much stuff to spawn per level
        FindQuotas();
        //Actually spawn all the objects into the levels
        SpawnThings();
    }
    
    ///Does the specific level we're on change any basic info (Size/LinkOdds/etc)? Do that here
    public virtual void Customize()
    {
        //As you go deeper the map gets bigger
        int l = God.Session.Level;
        //Width starts at 2, and every third level grows by 1
        int w = 2 + Mathf.FloorToInt(l/3);
        //Height starts at 2, and every other level grows by 1
        int h = 2 + Mathf.FloorToInt(l/2);
        Size = new Vector2Int(w, h);
        //If we haven't created a player yet for this playthrough, make one.
        if (God.Session.Player == null)
            God.Session.Player = God.Library.GetThing(new SpawnRequest(GameTags.Player)).Create();
    }

    ///Build out a zoomed-out map of the level, without specific rooms
    public virtual void BuildGeoMap()
    {
        //Two nested for loops lets us build a grid of room slots at our desired size
        for (int x = 0; x < Size.x; x++)for (int y = 0; y < Size.y; y++)
        {
            //Spawn a blank room slot into the position 
            GeoTile g = new GeoTile(x, y,this);
            //If our dictionary doesn't have this X-row added, add it
            if(!GeoMap.ContainsKey(x)) GeoMap.Add(x,new Dictionary<int, GeoTile>());
            //Add it to the dictionary and list of slots
            GeoMap[x].Add(y,g);
            AllGeo.Add(g);
        }
    }

    ///Connect geomorphs to each other to make sure there's a path from player spawn to exit
    public virtual void BuildMainPath()
    {
        //Make a safe path leading to the exit
        //Pick the column the player spawns in (at the bottom of the level)
        int start = Random.Range(0, Size.x);
        //For each row. . .
        for (int y = 0; y < Size.y; y++)
        {
            //Pick a slot to move the path towards before moving up a row
            int end = Random.Range(0, Size.x);
            if(end == start) end = Random.Range(0, Size.x);
            int x = start;
            //If this is the bottom row. . .
            if (y == 0)
            {
                //Then our start column is the player start point
                PlayerSpawn = GetGeo(x, 0);
                PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
            }
            //While we haven't reached out destination. . .
            while(x != end)
            {
                int old = x;
                //Take a step towards the destination
                x = (int)Mathf.MoveTowards(x, end, 1);
                //Open up a connection between the tile we came from and the one we're at now
                GeoTile a = GetGeo(old, y);
                GeoTile b = GetGeo(x, y);
                //And make sure it's marked as being on the main path (does nothing for now)
                b.SetPath(GeoTile.GeoTileTypes.MainPath);
                if (start > end)
                {
                    a.Links.Add(Directions.Left);
                    b.Links.Add(Directions.Right);
                }
                else
                {
                    a.Links.Add(Directions.Right);
                    b.Links.Add(Directions.Left);
                }
            }
            //Mark our end point as the start of the path on the next row
            start = end;
            //Move up row up and repeat, linking to the slot below
            if (y < Size.y - 1)
            {
                GeoTile a = GetGeo(start, y);
                GeoTile b = GetGeo(start, y+1);
                a.Links.Add(Directions.Up);
                b.Links.Add(Directions.Down);
            }
            else
            {
                //But if we're at the top of the map, mark our ultimate position as the exit spawn location
                Exit = GetGeo(start, y); 
                Exit.SetPath(GeoTile.GeoTileTypes.Exit);
            }
        }
    }

    ///Open more connections between geomorphs to make sure all are accessable
    public virtual void ConnectAllGeos()
    {
        //Open up a bunch of random links between rooms
        //For each room slot that exists. . .
        foreach (GeoTile g in AllGeo)
        {
            //Get a list of the slots above it and to its right
            List<Directions> maybe = g.PotentialLinks();
            //For each possible link. . .
            foreach (Directions d in maybe)
            {
                //Flip a (weighted) coin. If it comes up false, don't link the room slots
                if (!God.CoinFlip(LinkOdds)) continue;
                //But if it came up true, and its neighbor actually exists, connect them!
                GeoTile other = g.Neighbor(d);
                if (other == null) continue;
                g.Links.Add(d);
                other.Links.Add(God.OppositeDir(d));
            }
        }

        //Do we have any isolated rooms you can't get to?
        //If so, open some more links
        List<GeoTile> uncon = UnconnectedTest();
        //Only do this while loop 99 times, in case we cause an infinite loop
        int safety = 99;
        while (uncon.Count > 0 && safety > 0)
        {
            safety--;
            //For each tile that's not connected to anyone. . .
            foreach (GeoTile g in uncon)
            {
                //Get a list of all four of its neighbors
                List<Directions> maybe = g.PotentialLinks(false);
                //If it has no neighbors, something's wrong and we should throw an error
                if (maybe.Count == 0)
                {
                    Debug.LogError("TILE BOTH LINKLESS AND UNCONNECTABLE: " + g);
                    continue;
                }
                //Up/down links are more level-design-fun than left/right ones, and up/down are returned first in the list
                //So roll twice and take the smaller to bias towards up/down links
                int roll = Mathf.Min(Random.Range(0, maybe.Count), Random.Range(0, maybe.Count));
                Directions d = maybe[roll];
                //Find the neighbor in that direction and make sure it exists
                GeoTile other = g.Neighbor(d);
                if (other == null)
                {
                    Debug.LogWarning("POTENTIAL LINK NULL: " + g + " / " + d + " / " + other);
                    continue;
                }
                //If the tile we're connecting to is reachable, connect to it and mark yourself as reachable
                if (other.Path != GeoTile.GeoTileTypes.Unreachable)
                {
                    g.Links.Add(d);
                    other.Links.Add(God.OppositeDir(d));
                    g.SetPath(GeoTile.GeoTileTypes.Connected);
                    g.Depth = other.Depth + 1;
                }

            }
            uncon = UnconnectedTest();
        }
    }
    
    public virtual void PickRooms()
    {
        foreach (GeoTile g in AllGeo)
        {
            g.RoomType = God.Library.GetRoom(g, this);
        }
    }

    public virtual void BuildRooms()
    {
        foreach (GeoTile g in AllGeo)
        {
            if (g.RoomType == null) continue;
            g.Room = g.RoomType.Build(g, this);
            if (g.Room == null) continue;
            God.GM.Rooms.Add(g.Room);
            foreach (SpawnPointController spc in g.Room.Spawners)
            {
                // spc.ToSpawn.Refine();
                if(spc.AlwaysSpawn) SpawnPointsFixed.Add(spc);
                else SpawnPoints.Add(spc);
            }
        }
    }
    
    ///Do a flood of all the room slots in the dungeon and return a list of all that aren't connected 
    public List<GeoTile> UnconnectedTest()
    {
        //Make a flood to see if all the rooms are connected
        //Start with a list of all the tiles
        List<GeoTile> unc = new List<GeoTile>();
        unc.AddRange(AllGeo);
        List<GeoTile> done = new List<GeoTile>();
        List<GeoTile> pend = new List<GeoTile>(){PlayerSpawn};
        while (pend.Count > 0)
        {
            GeoTile chosen = God.Random(pend);
            pend.Remove(chosen);
            done.Add(chosen);
            unc.Remove(chosen);
            chosen.SetPath(GeoTile.GeoTileTypes.Connected);
            List<GeoTile> neigh = chosen.Neighbors(true);
            foreach (GeoTile n in neigh)
            {
                if (done.Contains(n)) continue;
                if (pend.Contains(n)) continue;
                pend.Add(n);
            }
        }
        God.Log("FLOATERS: " + unc.Count);
        return unc;
    }

    public GeoTile GetGeo(int x, int y)
    {
        if (!GeoMap.ContainsKey(x)) return null;
        return GeoMap[x].ContainsKey(y) ? GeoMap[x][y] : null;
    }

    public float JudgeRoom(GeoTile g, RoomOption o)
    {
        RoomTags t = RoomTags.Generic;
        if (g.Path == GeoTile.GeoTileTypes.PlayerStart) t = RoomTags.PlayerStart;
        if (g.Path == GeoTile.GeoTileTypes.Exit) t = RoomTags.Exit;
        if(o.Tags.Contains(t)) return 1;
        return 0;
    }
    
    public float JudgeThing(SpawnRequest sr, ThingOption o,bool backup=false)
    {
        if (!backup)
        {
            //Make sure it's the right author. If either the option or the game is universal, it's okay
            if (sr.Author != Authors.Universal && o.Author != Authors.Universal && o.Author != sr.Author) return 0;
        }
        float w = 1;
        foreach(Tag t in sr.Mandatory)
            if (o.HasTag(t.Value, out float tw))
            {
                w = God.MergeWeight(w,tw);
            }
            else return 0;
        if (sr.Any.Count > 0)
        {
            bool any = false;
            foreach (Tag t in sr.Any)
            {
                if (o.HasTag(t.Value, out float tw))
                {
                    w = God.MergeWeight(w, tw);
                    any = true;
                }
            }
            if(!any)
                return 0;
        }
        return w;
    }

    public virtual void AddSpawn(params GameTags[] t)
    { SpawnRequests.Add(new SpawnRequest(t)); }
    public virtual void AddSpawn(params string[] t)
    { SpawnRequests.Add(new SpawnRequest(t)); }
    public virtual void AddSpawn(SpawnRequest sr)
    { SpawnRequests.Add(sr); }
    
    public virtual void FindQuotas()
    {
        float rms = AllGeo.Count - 2; //How many rooms other than start and end are there?
        //We're going to spawn 1 monster per room on average, but each level the density rises
        float mons = God.RoundRand(rms * (1f + (God.Session.Level * 0.1f)));
        for(float n=0;n<mons;n++) AddSpawn(GameTags.NPC);
        float wpn = God.RoundRand(1 + (rms * 0.05f));
        for(float n=0;n<wpn;n++) AddSpawn(GameTags.Weapon);
        float con = God.RoundRand(rms * 0.25f);
        for(float n=0;n<con;n++) AddSpawn(GameTags.Consumable);
        float scr = God.RoundRand(rms * 0.5f);
        for(float n=0;n<scr;n++) AddSpawn(GameTags.ScoreThing);
        
        //Then we take those tags and use them to decide on a list of actual things to spawn
        foreach (SpawnRequest sr in SpawnRequests)
        {
            ThingOption o = God.Library.GetThing(sr);
            ToSpawn.Add(o);
        }
        
    }
    
    public virtual void SpawnThings()
    {
        SpawnPointController playerStart = null;
        foreach (SpawnPointController s in PlayerSpawn.Room.Spawners)
        {
            if (s.ToSpawn.HasTag(GameTags.Player))
            {
                // Debug.Log("PLAYER SPAWN FOUND");
                playerStart = s;
                SpawnPoints.Remove(s);
                SpawnPointsFixed.Remove(s);
                break;
            }
        }

        if (playerStart != null) God.Session.Player.Spawn(playerStart);
        else
        {
            Debug.Log("ERROR: Player Spawn Room has no spawners for a player: " + PlayerSpawn.Room);
            God.Session.Player.Spawn(PlayerSpawn.Room.transform.position);
        }
        int totalToSpawn = ToSpawn.Count;
        int totalSpawns = SpawnPoints.Count;
        while(ToSpawn.Count > 0)
        {
            if (SpawnPoints.Count == 0)
            {
                Debug.Log("Ran out of spawn points ("+totalSpawns+")! " + ToSpawn.Count+"/"+totalToSpawn + " items left to spawn.");
                break;
            }
            ThingOption o = ToSpawn.Random();
            ToSpawn.Remove(o);
            List<SpawnPointController> s = new List<SpawnPointController>();
            s.AddRange(SpawnPoints);
            ThingInfo i=null;
            while (s.Count > 0)
            {
                SpawnPointController chosen = s.Random();
                s.Remove(chosen);
                if (chosen.CanSpawn(o,this))
                {
                    i = o.Create();
                    i.Spawn(chosen);
                    SpawnPoints.Remove(chosen);
                    break;
                }
            }
            if(i == null) Debug.Log("Thing couldn't find a place to spawn: " + o.Name);
        }

        foreach (SpawnPointController s in SpawnPointsFixed)
        {
            s.Spawn();
        }
        // foreach (GeoTile g in AllGeo)
        // {
        //     g.Room.Spawn();
        // }
    }
}
