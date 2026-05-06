using Unity.VisualScripting;
using UnityEngine;

public class Level_JaidenB : LevelBuilder
{
    public Level_JaidenB()
    {
        Author = Authors.JaidenB;
    }

    public override void Customize()
    {
        //If we haven't created a player yet for this playthrough, make one.
        SpawnPlayer();
        Size = new Vector2Int(2, 3); //forces the grid to be a certain size
    }

    public override void BuildGeoMap()
    {
        //AddGeo(new GeoTile(0, 0, this));
        // AddGeo(new GeoTile(0, 1, this));
        // AddGeo(new GeoTile(1, 1, this));
        // AddGeo(new GeoTile(1, 2, this));
        //AddGeo(new GeoTile(1, 3, this));
        // AddGeo(new GeoTile(1, 4, this));
        // AddGeo(new GeoTile(2, 4, this));
        // AddGeo(new GeoTile(0, 4, this));
        //Two nested for loops lets us build a grid of room slots at our desired size
        int RandomY = Random.Range(4, 8);

        for (int x = 0; x < Size.x; x++)
        for (int y = 0; y < RandomY; y++)
            {
                //Spawn a blank room slot into the position 
                AddGeo(new GeoTile(x, y, this));
            }
    }

    public override void BuildMainPath()
    {
        int Start = Random.Range(0, Size.x);

        Exit = GetGeo(1, 2);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);

        for (int y = 0; y < Size.y; y++) 
        {
            int End = Random.Range(0, Size.x);
            if (End == Start) { End = Random.Range(0, Size.x); }

            int x = Start;
            Exit = GetGeo(End, y);
            if (y == 0) { PlayerSpawn = GetGeo(0, y); PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart); }
        }
    }
}
