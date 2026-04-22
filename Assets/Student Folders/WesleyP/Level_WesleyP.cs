using UnityEngine;

public class Level_WesleyP : LevelBuilder
{
    public Level_WesleyP()
    {
        Author = Authors.WesleyP;
    }
     public override void Customize()
    {
        //If we haven't created a player yet for this playthrough, make one.
        SpawnPlayer();
        Size = new Vector2Int(4, 4); //forces the grid to be a certain size
    }

    public override void BuildGeoMap()
    {
        AddGeo(new GeoTile(1, 1, this));
        AddGeo(new GeoTile(1, 2, this));
        AddGeo(new GeoTile(1, 3, this));
        AddGeo(new GeoTile(1, 4, this));
        AddGeo(new GeoTile(2, 4, this));

        AddGeo(new GeoTile(0, 4, this));
    }

    public override void BuildMainPath()
    {

        Exit = GetGeo(1, 1 + 1);
        Exit = GetGeo(2, 1 + 2);
    }
}


