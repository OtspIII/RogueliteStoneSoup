using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelBuilder
{
    public Vector2Int Size = new Vector2Int(3, 3);
    public Dictionary<int, Dictionary<int, GeoTile>> GeoMap = new Dictionary<int, Dictionary<int, GeoTile>>();
    public List<GeoTile> AllGeo = new List<GeoTile>();
    public float LinkOdds = 0.1f;
    public GeoTile PlayerSpawn;
    public GeoTile Exit;

    public LevelBuilder()
    {
        for (int x = 0; x < Size.x; x++)for (int y = 0; y < Size.y; y++)
        {
            GeoTile g = new GeoTile(x, y,this);
            if(!GeoMap.ContainsKey(x)) GeoMap.Add(x,new Dictionary<int, GeoTile>());
            GeoMap[x].Add(y,g);
            AllGeo.Add(g);
        }

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
                GetGeo(start, y).SetPath(GeoTile.GeoTileTypes.Exit);
            }
        }

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
