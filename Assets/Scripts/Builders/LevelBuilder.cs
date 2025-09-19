using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder
{
    public Vector2Int Size = new Vector2Int(3, 3);
    public Dictionary<int, Dictionary<int, GeoTile>> GeoMap = new Dictionary<int, Dictionary<int, GeoTile>>();
    public List<GeoTile> AllGeo = new List<GeoTile>();

    public LevelBuilder()
    {
        for (int x = 0; x < Size.x; x++)for (int y = 0; y < Size.y; y++)
        {
            GeoTile g = new GeoTile(x, y);
            if(!GeoMap.ContainsKey(x)) GeoMap.Add(x,new Dictionary<int, GeoTile>());
            GeoMap[x].Add(y,g);
            AllGeo.Add(g);
        }
        God.Random(AllGeo).PlayerStart = true;
    }

    public GeoTile GetGeo(int x, int y)
    {
        if (!GeoMap.ContainsKey(x)) return null;
        return GeoMap[x].ContainsKey(y) ? GeoMap[x][y] : null;
    }
}

public class GeoTile
{
    public int X;
    public int Y;
    public bool PlayerStart;
    public List<Directions> Links;
    
    public GeoTile(int x, int y)
    {
        X = x;
        Y = y;
    }
}
