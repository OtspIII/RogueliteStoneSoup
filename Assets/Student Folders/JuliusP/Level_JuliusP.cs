using UnityEngine;
using System.Collections.Generic;

public class Level_JuliusP : LevelBuilder
{
    public Vector2Int OriginalLvlSize;

    //LEVEL 1 BOOL//
    public bool CanLinkToLootRoom = false;

    //LVEL 2 BOOLS//
    public bool Lv2BossKilled = false;
    public bool Lv2AllyFound = false;
    public bool Lv2RedLightKilled = false;

    //LVEL 3 BOOLS//
    public bool Lv3FirstRedLightKilled = false;
    public bool Lv3FirstLevel2ShieldEnemyKilled = false;
    public bool Lv3RedLight3Killed = false;
    public bool Lv3FinalShieldEnemKilled = false;
    public bool Lv3FinalDoorCanOpen = false;


    //lVL 4 BOOLS//

    public bool Lv4DestroyedLvl3Enem = false;


    //LV5.BOOLS//

    public bool Lv5MiniBossKilled = false;

    public bool Lv5FinalBossKilled = false;
    

    public float SpecialRoomChance = 0.3f;

    public Level_JuliusP()
    {
        Author = Authors.JuliusP;
    }

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
    

        

        //IF LEVEL IS 3, CHANGE ROOM SIZE//
        if(l == 3 || l == 4 || l == 5)
        {
            
         RoomSize = new Vector2Int(17, 20);  


        }
        

        else
        {
            
        RoomSize = new Vector2Int(12, 12);


        }


       
    }


public override void BuildGeoMap()
{
    int centerX = 3;
    int centerY = 3;
    int leftsideY = -2;

    int CurrentLevel = God.Session.Level;

    if (CurrentLevel == 1)
    {
        BuildLevel1(centerX, centerY, leftsideY);
    }

    else if(CurrentLevel == 2)
    {
        //BUILD LEVEL, KEEP MOST/ALL OF  LEVEL ONE//
        BuildLevel1(centerX, centerY, leftsideY); 

        //ADD THE NEW PARTS TO LEVEL 1//
        BuildLevel2(centerX, centerY, leftsideY);

    }


    else if(CurrentLevel == 3)
    {
        BuildLevel3(centerX, centerY, leftsideY); 
    }

    else if(CurrentLevel == 4)
    {
        BuildLevel4(centerX, centerY, leftsideY); 
    }

    
    else if(CurrentLevel == 5)
    {
        BuildLevel5(centerX, centerY, leftsideY); 
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

    PlayerSpawn = GetGeo(centerX, centerY);

    if (PlayerSpawn == null)
    {
       // Debug.LogError("PlayerSpawn is null at (3,3). Check BuildLevel1 geo map.");
        return;
    }

    PlayerSpawn.SetPath(GeoTile.GeoTileTypes.PlayerStart);

    GeoTile prev = PlayerSpawn;

  
    if (God.Session.Level == 1)
    {
        // UP PATH (main progression)
        for (int y = centerY + 1; y <= 3; y++)
        {
            GeoTile next = GetGeo(centerX, y);
            if (next == null) continue;

            prev.Links.Add(Directions.Up);
            next.Links.Add(Directions.Down);

            next.SetPath(GeoTile.GeoTileTypes.MainPath);
            prev = next;
        }

        Exit = GetGeo(centerX, 3);

        if (Exit != null)
            Exit.SetPath(GeoTile.GeoTileTypes.Exit);

        return;
    }

    // LEVEL 3 SPECIAL PATH
    if (God.Session.Level == 3)
    {
        for (int y = centerY + 1; y <= 5; y++)
        {
            GeoTile next = GetGeo(centerX, y);
            if (next == null) continue;

            prev.Links.Add(Directions.Up);
            next.Links.Add(Directions.Down);

            next.SetPath(GeoTile.GeoTileTypes.MainPath);

            prev = next;
        }

        Exit = GetGeo(centerX, 7);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        return;
    }

    // LEVEL 4 SPECIAL PATH
    if (God.Session.Level == 4)
    {
        for (int y = centerY + 1; y <= 5; y++)
        {
            GeoTile next = GetGeo(centerX, y);
            if (next == null) continue;

            prev.Links.Add(Directions.Up);
            next.Links.Add(Directions.Down);

            next.SetPath(GeoTile.GeoTileTypes.MainPath);

            prev = next;
        }

        Exit = GetGeo(centerX, 5);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);
        return;
    }

    // LEVEL 5 SPECIAL PATH
    if (God.Session.Level == 5)
    {
        for (int y = centerY + 1; y <= 6; y++)
        {
            GeoTile next = GetGeo(centerX, y);
            if (next == null) continue;

            prev.Links.Add(Directions.Up);
            next.Links.Add(Directions.Down);

            next.SetPath(GeoTile.GeoTileTypes.MainPath);

            prev = next;
        }

        Exit = GetGeo(centerX, 6);
        Exit.SetPath(GeoTile.GeoTileTypes.Exit);

        return;
    }

    // DEFAULT MAIN PATH
    for (int y = centerY + 1; y <= 6; y++)
    {
        GeoTile next = GetGeo(centerX, y);
        if (next == null) continue;

        prev.Links.Add(Directions.Up);
        next.Links.Add(Directions.Down);

        next.SetPath(GeoTile.GeoTileTypes.MainPath);
        prev = next;
    }

    Exit = GetGeo(centerX, 6);
    Exit.SetPath(GeoTile.GeoTileTypes.Exit);

    // DOWN BRANCH (3 → 1)
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

    // RIGHT BRANCH
    GeoTile RightTile = GetGeo(centerX, 1);
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

    // LEFT BRANCH
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

    // DOWN TO BOTTOM
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

    // LEFT BOTTOM BRANCH
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

    // RIGHT SIDE LINK
    GeoTile a = GetGeo(6, 0);
    GeoTile b = GetGeo(6, 1);

    if (a != null && b != null)
    {
        a.Links.Add(Directions.Down);
        b.Links.Add(Directions.Up);
    }

}

 public override float JudgeRoom(GeoTile g, RoomOption o, bool backup = false)
{
    o.Audit();

    int Level = God.Session.Level;

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

  
    if(g.Y == 1 && g.X == 6 && Level == 1)
    {
            
        
        return o.HasTag("FireRoom") ? 999 : 0; 


    }

    
    //AT (5,1), SPAWN THE HALLWAY TO FIRE ROOM//
    if(g.Y == 1 && g.X == 5)
    {
            
        
        return o.HasTag("HallwayToFire") ? 999 : 0; 


    }


    //SPAWN THE LOOT ROOM//
    if(g.Y == 0 && g.X == 6 && Level == 1)
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


     if (g.X == -3 && g.Y == -1 && Level == 1)
    {
            
        return o.HasTag("Boss") ? 999 : 0;



    }
 

    //BOSS LOOT ROOM AT (-3,0)//



     if (g.X == -3 && g.Y == 0 && Level == 1)
    {
            
        return o.HasTag("LootBoss") ? 999 : 0;



    }


    //THIS SECTION IS FOR LEVEL 2 LOGIC//
    if(Level == 2)
    {
            
       if (g.X == 2 && g.Y == 3)
        {
            
        
            return o.HasTag("LavaHall") ? 999 : 0;



        } 



        if (g.X == 1 && g.Y == 3)
        {
            
        
            return o.HasTag("LavaCorner") ? 999 : 0;



        } 


        //PLAYER SPAWN//
        if (g.X == 3 && g.Y == 3)
        {
            
        
            return o.HasTag("Lv2PlayerRoom") ? 999 : 0;



        } 



        //RIGHT OF THE PLAYER SPAWN IN LEVEL 2  X-> 4-5
        if (g.X >= 4 && g.X <= 5 && g.Y == 3)
        {
            
        
            return o.HasTag("Lv2Generic") ? 999 : 0;



        }



        //RIGHT OF THE PLAYER SPAWN IN LEVEL 2 THE CORNER FOLLOWING THE LAVA HALLWAY  X-> 6,3
        if (g.X == 6 && g.Y == 3)
        {
            
        
            return o.HasTag("LavaCornerTop") ? 999 : 0;



        }



        //RIGHT OF THE PLAYER SPAWN IN LEVEL 2 POINTS-> 6,4,
        if (g.X == 6 && g.Y == 4)
        {
            
        
            return o.HasTag("DeadlyHall") ? 999 : 0;



        }


        //TOP CORNER AT (6,5) IN LEVEL 2//

        if(g.X == 6 && g.Y == 5)
        {
                
            return o.HasTag("DeadlyTopCorner") ? 999 : 0;



        }


        ///LAVA PUZZLE ROOM//
        

        if(g.X == 0 && g.Y == 3)
        {
                
            return o.HasTag("Lava Puzzle") ? 999 : 0;



        }


        if(g.X == 0 && g.Y == 4)
        {
                
            return o.HasTag("Above") ? 999 : 0;



        }




       
    
       
         
        //FIRE ROOM// 
        if(g.X == 6 && g.Y == 1)
        {
                
            return o.HasTag("Fire") ? 999 : 0;



        }

        //FIRE ROOM// 
        if(g.X == 6 && g.Y == 0)
        {
                
            return o.HasTag("LootRoom2") ? 999 : 0;



        }


        //BOSS ROOM LEVEL 2//
        if(g.X == -3 && g.Y == 0)
        {
   
                
            return o.HasTag("Loot2") ? 999 : 0;

            



        }



         //BOSS ROOM LEVEL 2, CHANGE SIZE TEMPORARILY///
        if(g.X == -3 && g.Y == -1)
        {
   
                
            return o.HasTag("Lv2.Boss") ? 999 : 0;

            



        }




        //POTION ROOM AFTER KILLING RED LIGHT//
        if(g.X == 7 && g.Y == 5)
        {
   
                
            return o.HasTag("PotionRoom") ? 999 : 0;

            



        }


    }
 

 
        //LEVEL LAYOUT FOR LEVEL 3//


        if(Level == 3)
        {
            
        if(g.X == 3 && g.Y == 3)
        {
                

     
            return o.HasTag("Lv3.PlayerRoom") ? 999 : 0;



        }

        if(g.X == 3 && g.Y == 4)
        {
                

     
            return o.HasTag("Lv3.Generic") ? 999 : 0;



        }


        if(g.X == 3 && g.Y == 5)
        {
                

     
            return o.HasTag("Lv3.Generic 2") ? 999 : 0;



        }


        if(g.X == 3 && g.Y == 6)
        {
                

     
            return o.HasTag("Lv3.Generic 3") ? 999 : 0;



        }


        //EXIT ROOM//
        if(g.X == 3 && g.Y == 7)
        {
                

     
            return o.HasTag("Lv3.Exit") ? 999 : 0;



        }



        }


        //LEVEL 4 LAYOUT (ADDED)
        if(Level == 4)
        {
            if(g.X == 3 && g.Y == 3)
            {
                return o.HasTag("Lv4.PlayerRoom") ? 999 : 0;
            }

            if(g.X == 3 && g.Y == 4)
            {
                return o.HasTag("Lv4.Generic") ? 999 : 0;
            }

            if(g.X == 3 && g.Y == 5)
            {
                return o.HasTag("Lv4.Exit") ? 999 : 0;
            }
        }


        //LVEL 5 (FINAL ROOM)//



        if(Level == 5)
        {
            

            if(g.X == 3 && g.Y == 3)
            {
                return o.HasTag("Lv5.PlayerRoom") ? 999 : 0;
            } 


            if(g.X == 3 && g.Y == 4)
            {
                return o.HasTag("Lv5.Generic") ? 999 : 0;
            } 


            if(g.X == 3 && g.Y == 5)
            {
                return o.HasTag("Lv5.Boss") ? 999 : 0;
            } 


            if(g.X == 3 && g.Y == 6)
            {
                return o.HasTag("Lv5.Exit") ? 999 : 0;
            } 



        }
  
    return o.HasTag(t.ToString()) ? 1 : 0;
}



void BuildLevel1(int centerX, int centerY, int leftsideY)
{

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

    if(CurrentLevel != 2)
    {
            
 }
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


void BuildLevel2(int centerX, int centerY, int leftsideY)
{

    //NEW ADDITIONS FOR LEVEL 2//

    //GO LEFT FROM (3,3)-> PLAYERSPAWN//
    for(int i = 1; i<4; i++)
    {
        AddGeo(new GeoTile(centerX - i, 3, this));
    }


    //GO RIGHT FROM (3,3)-> PLAYERSPAWN//
    for(int i = 1; i<4; i++)
    {
        AddGeo(new GeoTile(centerX + i, 3, this));
    }



    //GO UP FROM (6,3)
    for(int i = 1; i<3; i++)
    {
        AddGeo(new GeoTile(6, centerY + i, this));
    }


    //ADD TILE (7,5)
    AddGeo(new GeoTile(7, 5, this));


    //ADD TILE (0,4)
    AddGeo(new GeoTile(0, 4, this));

}


void BuildLevel3(int centerX, int centerY, int leftsideY)
{
    

    // START TILE
    AddGeo(new GeoTile(centerX, centerY, this));

    // GO UP FROM (3,3)
    for(int i = 1; i < 5; i++)
    {
        AddGeo(new GeoTile(centerX, centerY + i, this));
    }
  
}



//LEVEL 4//


void BuildLevel4(int centerX, int centerY, int leftsideY)
{
    

    // START TILE
    AddGeo(new GeoTile(centerX, centerY, this));

    // GO UP FROM (3,3)
    for(int i = 1; i < 3; i++)
    {
        AddGeo(new GeoTile(centerX, centerY + i, this));
    }
  
}



void BuildLevel5(int centerX, int centerY, int leftsideY)
{
    

    // START TILE
    AddGeo(new GeoTile(centerX, centerY, this));

    // GO UP FROM (3,3)
    for(int i = 1; i < 4; i++)
    {
        AddGeo(new GeoTile(centerX, centerY + i, this));
    }
  
}

public override void FindQuotas()
{
    float rms = AllGeo.Count - 2;
    int level = God.Session.Level;

    float mons;

    // ENEMY SCALING BY LEVEL (1 → 5)
    if (level == 1)
    {
        mons = 2;
    }
    else if (level == 2)
    {
        mons = 4;
    }
    else if (level == 3)
    {
        mons = rms * 0.5f;
    }
    else if (level == 4)
    {
        mons = rms * 0.8f;
    }
    else 
    {
        mons = rms * 1.2f;
    }

  
    mons = Mathf.Max(0, mons);

    Quotas.Add(new Tag(GameTags.NPC, 1, mons));

    Quotas.Add(new Tag(GameTags.Weapon, 1, 2));
    Quotas.Add(new Tag(GameTags.Consumable, 1, rms * 0.2f));
    Quotas.Add(new Tag(GameTags.ScoreThing, 1, level));

    FindThings();
}


}