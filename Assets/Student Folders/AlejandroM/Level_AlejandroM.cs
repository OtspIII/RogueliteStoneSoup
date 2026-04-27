using UnityEngine;

public class Level_AlejandroM : LevelBuilder
{
    public Level_AlejandroM()
    {
        Author = Authors.AlejandroM;
    }

    public override void Customize()
    {
        SpawnPlayer(); // spawns player

        Size = new Vector2Int(3, 3); //size is a 3x3

        LinkOdds = 0.2f; //a chance to create more rooms to connect

        Boss = null; // no boss yet
    }

    public override void BuildGeoMap()
    {   // top row of T
        AddGeo(new GeoTile(0, 2, this));
        AddGeo(new GeoTile(1, 2, this));
        AddGeo(new GeoTile(2, 2, this));
        // middle row of T
        AddGeo(new GeoTile(1, 1, this));
        // bottom row of T
        AddGeo(new GeoTile(1, 0, this));
    }

    public override void BuildMainPath() //connects tiles
    {   // sets player spwn 
        PlayerSpawn = GetGeo(1, 0);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
        // sets exit
        Exit = GetGeo(2, 2);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        // Connects the tiles 
        GetGeo(1, 0).Links.Add(Directions.Up);
        GetGeo(1, 1).Links.Add(Directions.Down);

        GetGeo(1, 1).Links.Add(Directions.Up);
        GetGeo(1, 2).Links.Add(Directions.Down);

        GetGeo(1, 2).Links.Add(Directions.Left);
        GetGeo(0, 2).Links.Add(Directions.Right);

        GetGeo(1, 2).Links.Add(Directions.Right);
        GetGeo(2, 2).Links.Add(Directions.Left);
    }
}