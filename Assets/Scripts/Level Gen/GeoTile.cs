using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// A 'slot' that a room can go into on a map. Used to figure out what rooms can go where
[System.Serializable]
public class GeoTile
{
    public int X; //X any Y coordinates
    public int Y;
    public GeoTileTypes Path = GeoTileTypes.Unreachable; //Is this slot on the main path/the player start/etc?
    public List<Directions> Links = new List<Directions>(); //What directions does this slot need to be able to connect to?
    public int Depth = 0;  //How far off the main path is this slot?
    public LevelBuilder Builder;  //Just a link to the LevelBuilder that made me
    public RoomOption RoomType;  //A link to the room type that will spawn in my slot
    public RoomScript Room;  //A link to the actual room that spawned in my slot
    
    ///Constructor, at first all I know is where I am and in what level
    public GeoTile(int x, int y, LevelBuilder b)
    {
        Builder = b;
        X = x;
        Y = y;
    }

    ///Returns a list of all directions I might be able to connect to--ie, that don't lead off the edge of the map
    public List<Directions> PotentialLinks(bool urOnly=true)
    {
        List<Directions> r = new List<Directions>();
        if(Y != Builder.Size.y - 1 && !Links.Contains(Directions.Up)) r.Add(Directions.Up);
        if(!urOnly && Y != 0 && !Links.Contains(Directions.Down)) r.Add(Directions.Down);
        if(X != Builder.Size.x - 1 && !Links.Contains(Directions.Right)) r.Add(Directions.Right);
        if(!urOnly && X != 0 && !Links.Contains(Directions.Left)) r.Add(Directions.Left);
        return r;
    }

    ///Returns the tile that sits to my side in a direction (takes an enum)
    public GeoTile Neighbor(Directions d)
    {
        return Neighbor(God.DirToV(d));
    }
    ///Returns the tile that sits to my side in a direction (takes a Vector2)
    public GeoTile Neighbor(Vector2Int d)
    {
        return Neighbor(d.x, d.y);
    }
    ///Returns the tile that sits to my side in a direction (takes x and y offset)
    public GeoTile Neighbor(int x, int y)
    {
        int xx = X + x;
        int yy = Y + y;
        return Builder.GetGeo(xx, yy);
    }
    
    ///Returns a list of the tiles that are connected to me
    ///If linkedOnly is true, returns all tiles adjacent to me
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

    ///Updates the type of slot I am--on the path, player spawn, exit, boss, off the path, or unreachable
    public void SetPath(GeoTileTypes t)
    {
        //If you're telling me to be something less specific than what I already am, do nothing
        //Ie--if I'm 'on the path' and you tell me to be 'connected to the path' do nothing
        if ((int)t < (int)Path)
            Path = t;
    }

    //Overrides what I look like when you put me in a Debug.Log
    public override string ToString()
    {
        return "GeoTile[" + X + "." + Y + " / " + Path + "]";
    }

    //The types of tiles that can exist
    public enum GeoTileTypes
    {
        None=0,        //Should never happen
        PlayerStart=1, //The room the player spawns in
        Exit=2,        //The room the exit spawns in
        Boss=3,        //The room the boss spawns in
        MainPath=4,    //Part of the 'default' path the player can take to get to the exit from their start point
        Connected=5,   //Reachable from the player start point, but not on the main path
        Unreachable=6, //Not reachable by the player at all
    }
}
