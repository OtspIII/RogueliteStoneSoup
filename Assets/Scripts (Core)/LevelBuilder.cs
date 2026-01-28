using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBuilder
{
    public Vector2Int Size = new Vector2Int(2, 2);
    public Dictionary<int, Dictionary<int, GeoTile>> GeoMap = new Dictionary<int, Dictionary<int, GeoTile>>();
    public List<GeoTile> AllGeo = new List<GeoTile>();
    public float LinkOdds = 0.1f;
    public GeoTile PlayerSpawn;
    public GeoTile Exit;
    public List<SpawnPointController> SpawnPoints = new List<SpawnPointController>();
    public List<SpawnPointController> SpawnPointsFixed = new List<SpawnPointController>();
    public List<GameTags> ToSpawnTags = new List<GameTags>();
    public List<ThingOption> ToSpawn = new List<ThingOption>();
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
    
    public virtual void Customize()
    {
        //As you go deeper the map gets bigger
        int l = God.Session.Level;
        int w = 2 + Mathf.FloorToInt(l/3);
        int h = 2 + Mathf.FloorToInt(l/2);
        Size = new Vector2Int(w, h);
        if (God.Session.Player == null)
            God.Session.Player = God.Library.GetThing(new SpawnRequest(GameTags.Player)).Create();
    }

    public virtual void BuildGeoMap()
    {
        for (int x = 0; x < Size.x; x++)for (int y = 0; y < Size.y; y++)
        {
            GeoTile g = new GeoTile(x, y,this);
            if(!GeoMap.ContainsKey(x)) GeoMap.Add(x,new Dictionary<int, GeoTile>());
            GeoMap[x].Add(y,g);
            AllGeo.Add(g);
        }
    }

    public virtual void BuildMainPath()
    {
        //Make a safe path leading to the exit
        int start = Random.Range(0, Size.x);
        //For each row. . .
        for (int y = 0; y < Size.y; y++)
        {
            //Pick a slot to move the path towards
            int end = Random.Range(0, Size.x);
            if(end == start) end = Random.Range(0, Size.x);
            int x = start;
            //If this is the bottom row, the first place we are is the player spawn
            if (y == 0)
            {
                PlayerSpawn = GetGeo(x, 0);
                PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
            }

            God.Log("Y: " + y + " / X:" + start+"->"+end);
            while(x != end)
            {
                int old = x;
                x = (int)Mathf.MoveTowards(x, end, 1);
                GeoTile a = GetGeo(old, y);
                GeoTile b = GetGeo(x, y);
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
            start = end;
            //Move up row up and repeat
            if (y < Size.y - 1)
            {
                GeoTile a = GetGeo(start, y);
                GeoTile b = GetGeo(start, y+1);
                a.Links.Add(Directions.Up);
                b.Links.Add(Directions.Down);
            }
            else
            {
                Exit = GetGeo(start, y); 
                Exit.SetPath(GeoTile.GeoTileTypes.Exit);
            }
        }
    }

    public virtual void ConnectAllGeos()
    {
        //Open up a bunch of random links between rooms
        foreach (GeoTile g in AllGeo)
        {
            List<Directions> maybe = g.PotentialLinks();
            foreach (Directions d in maybe)
            {
                if (!God.CoinFlip(LinkOdds)) continue;
                GeoTile other = g.Neighbor(d);
                if (other == null) continue;
                g.Links.Add(d);
                other.Links.Add(God.OppositeDir(d));
            }
        }

        //Do we have any isolated rooms you can't get to?
        //If so, open some more links
        List<GeoTile> uncon = UnconnectedTest();
        int safety = 99;
        while (uncon.Count > 0 && safety > 0)
        {
            safety--;
            foreach (GeoTile g in uncon)
            {
                List<Directions> maybe = g.PotentialLinks(false);
                if (maybe.Count == 0)
                {
                    Debug.Log("TILE BOTH LINKLESS AND UNCONNECTABLE: " + g);
                    continue;
                }
                //Roll twice and take the smaller to bias towards up/down links
                int roll = Mathf.Min(Random.Range(0, maybe.Count), Random.Range(0, maybe.Count));
                Directions d = maybe[roll];
                GeoTile other = g.Neighbor(d);
                if (other == null)
                {
                    Debug.Log("POTENTIAL LINK NULL: " + g + " / " + d + " / " + other);
                    continue;
                }
                if ((int)other.Path <= (int)GeoTile.GeoTileTypes.Connected)
                {
                    g.Links.Add(d);
                    other.Links.Add(God.OppositeDir(d));
                    g.SetPath(GeoTile.GeoTileTypes.Connected);
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
    
    public float JudgeThing(SpawnRequest sr, ThingOption o)
    {
        Debug.Log("AUTHORS: " + sr.Author + " / " + o.Author);
        //Make sure it's the right author. If either the option or the game is universal, it's okay
        if (sr.Author != Authors.Universal && o.Author != Authors.Universal && o.Author != sr.Author) return 0;
        // if (!sr.JudgeLevel(o)) return 0;
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
    
    public virtual void FindQuotas()
    {
        float rms = AllGeo.Count - 2; //How many rooms other than start and end are there?
        //We're going to spawn 1 monster per room on average, but each level the density rises
        float mons = God.RoundRand(rms * (1f + (God.Session.Level * 0.1f)));
        for(float n=0;n<mons;n++) ToSpawnTags.Add(GameTags.NPC);
        float wpn = God.RoundRand(1 + (rms * 0.05f));
        for(float n=0;n<wpn;n++) ToSpawnTags.Add(GameTags.Weapon);
        float con = God.RoundRand(rms * 0.25f);
        for(float n=0;n<con;n++) ToSpawnTags.Add(GameTags.Consumable);
        float scr = God.RoundRand(rms * 0.5f);
        for(float n=0;n<scr;n++) ToSpawnTags.Add(GameTags.ScoreThing);
        
        //Then we take those tags and use them to decide on a list of actual things to spawn
        foreach (GameTags tag in ToSpawnTags)
        {
            ThingOption o = God.Library.GetThing(new SpawnRequest(tag));
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

[System.Serializable]
public class GeoTile
{
    public int X;
    public int Y;
    public GeoTileTypes Path = GeoTileTypes.Unreachable;
    // public bool PlayerStart;
    public List<Directions> Links = new List<Directions>();
    // public bool MainPath;
    // public int Depth = 999;
    public LevelBuilder Builder;
    public RoomOption RoomType;
    public RoomScript Room;
    
    public GeoTile(int x, int y, LevelBuilder b)
    {
        Builder = b;
        X = x;
        Y = y;
    }

    public List<Directions> PotentialLinks(bool urOnly=true)
    {
        List<Directions> r = new List<Directions>();
        if(Y != Builder.Size.y - 1 && !Links.Contains(Directions.Up)) r.Add(Directions.Up);
        if(!urOnly && Y != 0 && !Links.Contains(Directions.Down)) r.Add(Directions.Down);
        if(X != Builder.Size.x - 1 && !Links.Contains(Directions.Right)) r.Add(Directions.Right);
        if(!urOnly && X != 0 && !Links.Contains(Directions.Left)) r.Add(Directions.Left);
        return r;
    }

    public GeoTile Neighbor(Directions d)
    {
        return Neighbor(God.DirToV(d));
    }
    public GeoTile Neighbor(Vector2Int d)
    {
        return Neighbor(d.x, d.y);
    }
    public GeoTile Neighbor(int x, int y)
    {
        int xx = X + x;
        int yy = Y + y;
        return Builder.GetGeo(xx, yy);
    }

    public List<GeoTile> Neighbors(bool linkedOnly=false)
    {
        List<GeoTile> r = new List<GeoTile>();
        List<Directions> dirs = linkedOnly ? Links : God.Dirs;
        foreach(Directions d in dirs)
        {
            GeoTile nei = Neighbor(d);
            if(nei != null && !r.Contains(nei))
                r.Add(nei);
        }
        return r;
    }

    public void SetPath(GeoTileTypes t)
    {
        if ((int)t < (int)Path)
            Path = t;
    }

    public override string ToString()
    {
        return "GeoTile[" + X + "." + Y + " / " + Path + "]";
    }

    public enum GeoTileTypes
    {
        None=0,
        PlayerStart=1,
        Exit=2,
        MainPath=3,
        Connected=4,
        Unreachable=5
    }
}
