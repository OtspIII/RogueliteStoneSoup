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

        int l = God.Session.Level;
        
        //SCALES THE WIDTH EVERY 2 LEVELS//
        int width = 4;
        
        //SCALES THE HEIGHT EVERY 3 LEVELS//
        int height = 4;
       
        Size = new Vector2Int(width,height);
        LinkOdds = 1f;
        RoomSize = new Vector2Int(12, 12);
    }


public override void BuildGeoMap()
{
    int centerX = 3;
    int centerY = 3;

    int leftsideY = -2;

    int CurrentLevel = God.Session.Level;
        

    // WHERE THE PLAYER ROOM TILE IS AT//
    AddGeo(new GeoTile(centerX, centerY, this));

    
    //GO UP FROM PLAYER ROOM//
    for(int i = 1; i<4; i++)
    {
            
    AddGeo(new GeoTile(centerX, centerY + i, this));

    } 


    //GO DOWN FROM PLAYER ROOM//
    //STOPS AT Y = 1//
    for(int i = 1; i<3; i++)
    {
            


     AddGeo(new GeoTile(centerX, centerY - i, this));


            
    }


    //AT (3,1), MOVE RIGHT//
    for(int i = 1; i<4; i++)
    {
            

     AddGeo(new GeoTile(centerX + i, 1, this));


    }



    //AT (3,1), MOVE LEFT//
    for(int i = 1; i<4; i++)
    {
            

     AddGeo(new GeoTile(centerX - i, 1, this));


    }



    //GO DOWN FROM (6,1)//
    AddGeo(new GeoTile(6,0, this));


    //GO DOWN FROM (0,1)//
    for(int i = 3; i<5; i++)
     {
            
        AddGeo(new GeoTile(0, centerY - i, this));


    }


    //GO LEFT FROM (0,-1)//
    for(int i = 4; i<6; i++)
    {
            
        AddGeo(new GeoTile(centerX - i, -1, this));



    }

    //GO UP FROM (-3,-2)//

    for(int i = 1; i<3; i++)
    {
            
        AddGeo(new GeoTile(-3, leftsideY + i, this));



    }


    


}



    


   
    public override void ConnectAllGeos()
    {
        
        base.ConnectAllGeos();

    }


public override void BuildMainPath()
{
    // PLAYER START
    int centerX = 3;
    int centerY = 3;

    //START AT (3,3)
    PlayerSpawn = GetGeo(centerX, centerY);
    PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);



    GeoTile prev = PlayerSpawn;

    // THIS MOVES UP FROM (3,3)//
    for (int y = centerY + 1; y <= 6; y++)
    {
        GeoTile next = GetGeo(centerX, y);
        if (next == null) continue;

        prev.Links.Add(Directions.Up);
        next.Links.Add(Directions.Down);

        next.SetPath(GeoTile.GeoTileTypes.MainPath);
        prev = next;
    }

    // Exit at top
    Exit = GetGeo(centerX, 6);
    Exit.SetPath(GeoTile.GeoTileTypes.Exit);

    // THIS MOVES DOWN FROM (3,3)//
    prev = PlayerSpawn;
    for (int y = centerY - 1; y >= 1; y--)
    {
        GeoTile next = GetGeo(centerX, y);
        if (next == null) continue;

        prev.Links.Add(Directions.Down);
        next.Links.Add(Directions.Up);

        next.SetPath(GeoTile.GeoTileTypes.MainPath);
        prev = next;
    }

    // RIGHT FROM (3,1)
    GeoTile RightTile= GetGeo(centerX, 1);
    prev = RightTile;
    for (int x = centerX + 1; x <= 5; x++)
    {
        GeoTile next = GetGeo(x, 1);
        if (next == null) continue;

        prev.Links.Add(Directions.Right);
        next.Links.Add(Directions.Left);

        next.SetPath(GeoTile.GeoTileTypes.MainPath);
        prev = next;
    }

    // LEFT FROM (3,1)
    prev = RightTile;
    for (int x = centerX - 1; x >= 0; x--)
    {
        GeoTile next = GetGeo(x, 1);
        if (next == null) continue;

        prev.Links.Add(Directions.Left);
        next.Links.Add(Directions.Right);

        next.SetPath(GeoTile.GeoTileTypes.MainPath);
        prev = next;
    }


    //DOWN FROM (0,1)
    
    prev = GetGeo(0, 1);

    for (int y = 0; y >= -2; y--)
    {   
       GeoTile next = GetGeo(0, y);
       if (next == null) continue;

       prev.Links.Add(Directions.Down);
       next.Links.Add(Directions.Up);

       next.SetPath(GeoTile.GeoTileTypes.MainPath);
       prev = next;
    }



// TO THE LEFT OF (0, -1) -> STOP AT (-3, -1)

    prev = GetGeo(0, -1);

   for (int x = -1; x >= -3; x--)
   {
      GeoTile next = GetGeo(x, -1);
   
      if (next == null) continue;

     prev.Links.Add(Directions.Left);
     next.Links.Add(Directions.Right);

     next.SetPath(GeoTile.GeoTileTypes.MainPath);
     prev = next;
    
   }

}


 public override float JudgeRoom(GeoTile g, RoomOption o, bool backup = false)
{
    o.Audit();

    if (o.Author != Authors.JuliusP && o.Author != Authors.Universal)
        return 0;

    GameTags t = GameTags.Generic;

    if (g.Path == GeoTile.GeoTileTypes.PlayerStart)
        t = GameTags.Player;

    if (g.Path == GeoTile.GeoTileTypes.Exit)
        t = GameTags.Exit;

    if (g.Path == GeoTile.GeoTileTypes.Boss)
        t = GameTags.Boss;

    
   //  HALL -> BETWEEN 1 AND 4//
    if (g.Y == 1 && g.X >= 1 && g.X <= 4)
    {   
        return o.HasTag("Hall") ? 999 : 0;
    }

    //AT (0,1), SPAWN THE CORNER HALL//
    
     if (g.Y == 1 && g.X == 0)
    {   
        return o.HasTag("HallCorner") ? 999 : 0;
    }


    //AT (6,1), SPAWN THE FIRE ROOM//
    if(g.Y == 1 && g.X == 6)
    {
            
        
        return o.HasTag("FireRoom") ? 999 : 0; 


    }

    
    //AT (5,1), SPAWN THE HALLWAY TO FIRE ROOM//
    if(g.Y == 1 && g.X == 5)
    {
            
        
        return o.HasTag("HallwayToFire") ? 999 : 0; 


    }


    //SPAWN THE LOOT ROOM//
    if(g.Y == 0 && g.X == 6)
     {
            
        return o.HasTag("Loot") ? 999 : 0;



    }


    //FROM -1 TO -2 ON THE X//


    if (g.X >= -2 && g.X <= -1 && g.Y == -1)
    {
            
        return o.HasTag("Hall") ? 999 : 0;



    }



    //ROOM AT (0, -1)


     if (g.X == 0 && g.Y == -1)
    {
            
        return o.HasTag("CornerBottom") ? 999 : 0;



    }




    //BOSS ROOM AT -3,-1//


     if (g.X == -3 && g.Y == -1)
    {
            
        return o.HasTag("Boss") ? 999 : 0;



    }
 

    //BOSS LOOT ROOM AT (-3,0)//



     if (g.X == -3 && g.Y == 0)
    {
            
        return o.HasTag("LootBoss") ? 999 : 0;



    }
 




   


  
    return o.HasTag(t.ToString()) ? 1 : 0;
}
}