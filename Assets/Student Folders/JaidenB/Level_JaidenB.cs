using UnityEngine;

public class Level_JaidenB : LevelBuilder
{
    public Level_JaidenB()
    {
        Author = Authors.JaidenB;
    }

    public virtual void Customize()
    {
        //As you go deeper the map gets bigger
        int l = God.Session.Level;
        //Width starts at 2, and every third level grows by 1
        int w = 5 + Mathf.FloorToInt(l / 3);
        //Height starts at 2, and every other level grows by 1
        int h = 5 + Mathf.FloorToInt(l / 2);
        //If this is a test level, make it just be 1x2 so the exit touches the start point
        if (l == -1)
        {
            w = 1;
            h = 2;
        }
        //Set the size we calculated
        Size = new Vector2Int(w, h);
        //If we haven't created a player yet for this playthrough, make one.
        if (God.Session.Player == null)
            God.Session.Player = God.Library.GetThing(new SpawnRequest(GameTags.Player)).Create();
        //Pick a boss. If this isn't null, it'll spawn a boss room.
        Boss = God.Library.GetThing(new SpawnRequest(GameTags.Boss), this, false); //the false means 'its okay if you dont find one'
    }

    public virtual void BuildGeoMap()
    {
        //Two nested for loops lets us build a grid of room slots at our desired size
        for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
            {
                //Spawn a blank room slot into the position 
                GeoTile g = new GeoTile(x, y, this);
                AddGeo(g);
            }
    }

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
            if (end == start) end = Random.Range(0, Size.x);
            int x = start;
            //If this is the bottom row. . .
            if (y == 0)
            {
                //Then our start column is the player start point
                PlayerSpawn = GetGeo(x, 0);
                PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
            }
            //While we haven't reached out destination. . .
            while (x != end)
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
            //If I'm on the top row. . .
            if (y == Size.y - 1)
            {
                //If we picked a boss, we need to make them a room
                if (Boss != null)
                {
                    //Make a new tile above the current exit
                    GeoTile g = new GeoTile(start, y + 1, this);
                    AddGeo(g);
                    //And move the exit to that new tile
                    Exit = GetGeo(start, y + 1);
                    Exit.SetPath(GeoTile.GeoTileTypes.Exit);
                    //Then take the current tile and make it the boss tile
                    GeoTile boss = GetGeo(start, y);
                    boss.SetPath(GeoTile.GeoTileTypes.Boss);
                    //Link the boss and exit rooms
                    boss.Links.Add(Directions.Up);
                    Exit.Links.Add(Directions.Down);
                }
                else
                {
                    //But if we're at the top of the map, mark our ultimate position as the exit spawn location
                    Exit = GetGeo(start, y);
                    Exit.SetPath(GeoTile.GeoTileTypes.Exit);
                }
            }
            else //If I'm not on the top row. . .
            {
                GeoTile a = GetGeo(start, y);
                GeoTile b = GetGeo(start, y + 1);
                a.Links.Add(Directions.Up);
                b.Links.Add(Directions.Down);
                b.SetPath(GeoTile.GeoTileTypes.MainPath);
            }
        }
    }
}
