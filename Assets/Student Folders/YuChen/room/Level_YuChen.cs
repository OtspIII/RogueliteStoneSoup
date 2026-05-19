using UnityEngine;

public class Level_YuChen : LevelBuilder
{

    public Level_YuChen()
    {
        Author = Authors.YuChen;
    }
    public override void Customize()
    {
        //If we haven't created a player yet for this playthrough, make one.
        SpawnPlayer();
        //As you go deeper the map gets bigger
        int l = God.Session.Level;


        
        int w = 11 + Mathf.FloorToInt(l / 2);
        
        int h = 10 + Mathf.FloorToInt(l / 2);
  
        //Set the size we calculated
        Size = new Vector2Int(w, h);
        //Pick a boss. If this isn't null, it'll spawn a boss room.
        Boss = God.Library.GetThing(new SpawnRequest(GameTags.Boss), this, false); //the false means 'its okay if you dont find one'
    }

    public override void BuildGeoMap()
    {

        AddGeo(new GeoTile(6, 1, this));

        int CurrentLevel = God.Session.Level;

        int cx = 6;
        int cy = 1;
        int r = CurrentLevel;

        for (int i = 1; i <= r; i++)
        {
            AddGeo(new GeoTile(cx, cy + i, this));
            AddGeo(new GeoTile(cx + i, cy, this));
            AddGeo(new GeoTile(cx - i, cy, this));
        }




    }

    public override void BuildMainPath()
    {
        int CurrentLevel = God.Session.Level;

        PlayerSpawn = GetGeo(6, 1);
        PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);

        if (CurrentLevel == 1)
        {
            Exit = GetGeo(5, 1);
            Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        }
        if (CurrentLevel == 2)
        {
            Exit = GetGeo(6, 3);
            Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        }
        if (CurrentLevel == 3)
        {
            Exit = GetGeo(9, 1);
            Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        }
        if (CurrentLevel == 4)
        {
            Exit = GetGeo(2, 1);
            Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        }
        if (CurrentLevel == 5)
        {
            Exit = GetGeo(6, 7);
            Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        }
        if (CurrentLevel == 6)
        {
            Exit = GetGeo(11, 1);
            Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        }
        if (CurrentLevel >= 7)
        {
            Exit = GetGeo(6, 7);
            Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        }

    }

    

}
