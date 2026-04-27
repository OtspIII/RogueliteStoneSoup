using UnityEngine;
using System.Collections.Generic;

public class Level_JuliusP : LevelBuilder
{
    public Vector2Int OriginalLvlSize;

    public float SpecialRoomChance = 0.3f;

    public Level_JuliusP()
    {
        Author = Authors.JuliusP;
    }


    //OVVERIDE CUSTOMIZE FUNCTION FROM LEVEL BUILDER//
  // public override void Customize()
  // {

    //As you go deeper the map gets bigger

    //CURRENT LEVEL NUMBER//
  //  int l = God.Session.Level;

   // SpawnPlayer();

   // LinkOdds = 0.1f;

    //WIDTH AND HEIGHT OF THE GRID//
   // int w;
   // int h;

    //EVERY 5 LEVELS SET WIDTH TO BE 4 AND HEIGHT TO BE 5//
   // if (l % 5 == 0)
    //{
      // w = 4;
      //  h = 5;
   // }


    //EVERY 3 LEVELS, A SPECIAL ROOM APPEARS//
   // else if(l % 3 == 0 && Random.value < SpecialRoomChance)
   // {

       // w = 5;
        // h = 4;

   // }

   // else
   // {

        //WIDTH STARTS AT 5, AND EVERY THIRD LEVEL GROWS BY 1//
       // w = 6 + Mathf.FloorToInt(l / 3);

        //HEIGHT STARTS AT 5, AND EVERY OTHER LEVEL GROWS BY 1//
      //  h = 5 + Mathf.FloorToInt(l / 2);
   // }

    //Size = new Vector2Int(w, h);

    // if (God.Session.Player == null)
    //     God.Session.Player = God.Library.GetThing(new SpawnRequest(GameTags.Player)).Create();

    // Boss = God.Library.GetThing(new SpawnRequest(GameTags.Boss), this, false);

   // }


   public override void Customize()
    {
        SpawnPlayer();
        Size = new Vector2Int(2, 4);
        LinkOdds = 1f;
        RoomSize = new Vector2Int(11, 11);
    }


    public override void BuildGeoMap()
    {
       for (int x = 0; x < Size.x; x++)
       for (int y = 0; y < Size.y; y++)
        {
        AddGeo(new GeoTile(x, y, this));
        }
    }

  public override void BuildMainPath()
{
    // SET UP PLAYER TILE //
    PlayerSpawn = GetGeo(1, 0);
    PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);

    // GETS REFERECNE TO TILES//
    GeoTile up1 = GetGeo(1, 1);
    GeoTile up2 = GetGeo(1, 2);
    GeoTile up3 = GetGeo(1, 3);

    // SETS UP THE EXIT
    Exit = up3;
    Exit.SetPath(GeoTile.GeoTileTypes.Exit);

   

    //ADDS LINKS//
    PlayerSpawn.Links.Add(Directions.Up);
    up1.Links.Add(Directions.Down);
    up1.SetPath(GeoTile.GeoTileTypes.MainPath);

   
    up1.Links.Add(Directions.Up);
    up2.Links.Add(Directions.Down);
    up2.SetPath(GeoTile.GeoTileTypes.MainPath);

   
    up2.Links.Add(Directions.Up);
    up3.Links.Add(Directions.Down);
}
    public override float JudgeRoom(GeoTile g, RoomOption o, bool backup = false)
    {
        o.Audit();

        // SET TO MY ROOMS//
        if (o.Author != Authors.JuliusP && o.Author != Authors.Universal)
            return 0;

        GameTags t = GameTags.Generic;

        if (g.Path == GeoTile.GeoTileTypes.PlayerStart)
            t = GameTags.Player;

        if (g.Path == GeoTile.GeoTileTypes.Exit)
            t = GameTags.Exit;

        if (g.Path == GeoTile.GeoTileTypes.Boss)
            t = GameTags.Boss;

        if (o.HasTag(t.ToString()))
            return 1;

        return 0;
    }
}