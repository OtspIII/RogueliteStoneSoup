using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GeoTile
{
    public int X;
    public int Y;
    public GeoTileTypes Path = GeoTileTypes.Unreachable;
    // public bool PlayerStart;
    public List<Directions> Links = new List<Directions>();
    // public bool MainPath;
    public int Depth = 0;
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
