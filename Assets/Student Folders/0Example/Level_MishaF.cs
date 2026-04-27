using UnityEngine;

public class Level_MishaF : LevelBuilder
{
    public Level_MishaF()
    {
        Author = Authors.MishaF;
    }

    public override void Customize()
    {
        //If we haven't created a player yet for this playthrough, make one.
        SpawnPlayer();
        Size = new Vector2Int(4, 4);
        LinkOdds = 1;
        RoomSize = new Vector2Int(15, 15);
    }

    public override void BuildGeoMap()
    {
        AddGeo(new GeoTile(0, 0,this));
        AddGeo(new GeoTile(0, 1,this));
        AddGeo(new GeoTile(0, 2,this));
        AddGeo(new GeoTile(-1, 2,this));
        AddGeo(new GeoTile(1, 2,this));
    }

    public override void BuildMainPath()
    {
        PlayerSpawn = GetGeo(0, 0);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);
        Exit = GetGeo(1, 2); 
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);
    }
}
