using UnityEngine;

public class SpawnWall : Trait
{
    ThingInfo spawnedWall;

    // LEVEL 2 WALLS
    ThingInfo level2Wall;

    ThingInfo level2EndWall,level2EndWall2,Level2WallNearRedLight,Level2WallNearAlly;


    //LEVEL 3 WALLS//
     
    ThingInfo level3SecondRoomDoor, level3SecondRoomDoorToThird, level3ThirdRoomTop, Level3FinalDoor, Level3DoorToNPC;



    //LEVEL 4 WALLS//

    ThingInfo Level4Wall;


    //LEVEL 5 WALLS//

    ThingInfo BeginningLv5Wall, Lv5FinalBossDoor;

    Level_JuliusP LJP;

    public SpawnWall()
    {
        Type = Traits.SpawnWall_JuliusP;

        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
            {
                LJP = God.LB as Level_JuliusP;

                int Level = God.Session.Level;

                // LEVEL 1 WALL
                if (Level == 1)
                {
                    spawnedWall = SpawnWallAt(new Vector2(75.96f, 5.12f));
                }

                // LEVEL 2 WALL SETUP
                if (Level == 2)
                {
                    // MAIN WALL
                    level2Wall = SpawnWallAt(new Vector2(-23.01f, -6.22f));

                    // END WALLS
                    level2EndWall = SpawnWallAt(new Vector2(41.77f, 38.71f));
                    
                    level2EndWall2 = SpawnWallAt(new Vector2(44.58f, 38.71f));

                    // RED LIGHT WALL
                    Level2WallNearRedLight = SpawnWallAt(new Vector2(80.15f, 54.47f));

                    // ALLY WALL
                    Level2WallNearAlly = SpawnWallAt(new Vector2(75.98f, 4.96f));

                   // Debug.Log("Spawned all Level 2 walls");
                }


                //LEVEL 3 WALL SETUP

                if(Level == 3)
                {
                    //FIRST WALL IN 2ND ROOM//
                    level3SecondRoomDoor = SpawnWallAt(new Vector2(63.07f, 78.82f));

                    //SECONDS WALL IN 2ND ROOM LEADING TO THIRD ROOM//
                    level3SecondRoomDoorToThird = SpawnWallAt(new Vector2(63.08f, 84.9f));


                    //THIRD ROOM AT TOP//
                    level3ThirdRoomTop = SpawnWallAt(new Vector2(62.99f, 103.91f));


                    //FINAL DOOR//
                    Level3FinalDoor = SpawnWallAt(new Vector2(63f, 123.13f));


                    //DOOR TO NPC//

                    Level3DoorToNPC = SpawnWallAt(new Vector2(63f, 112.03f));


                }



                if(Level == 4)
                {

                    //DOOR TO MINIBOSS//
                    Level4Wall = SpawnWallAt(new Vector2(63f, 84.94f));



                }


                if(Level == 5)
                {

                  

                    //BEGINNNING DOOR IN LV5 ROOM//
                     BeginningLv5Wall = SpawnWallAt(new Vector2(63f, 83.89f));


                     //FINAL BOSS DOOR//

                     Lv5FinalBossDoor = SpawnWallAt(new Vector2(63f, 102.65f));




                }

                break;
            }

            case EventTypes.Update:
            {
                var level = God.LB as Level_JuliusP;

                // REMOVE FIRST WALL
                if (level != null &&level.CanLinkToLootRoom)
                {
                    if (spawnedWall != null)
                    {
                        spawnedWall.Destruct();
                        spawnedWall = null;
                    }
                }

                // DESTROY MAIN WALL WHEN BOSS DIES
                if (level2Wall != null && LJP != null && LJP.Lv2BossKilled)
                {
                    level2Wall.Destruct();
                    level2Wall = null;
                }

                // DESTROY ALLY WALL
                if (Level2WallNearAlly != null && LJP != null && LJP.Lv2AllyFound)
                {
                    Level2WallNearAlly.Destruct();
                    Level2WallNearAlly = null;
                }

                // DESTROY RED LIGHT WALL
                if (Level2WallNearRedLight != null && LJP != null && LJP.Lv2RedLightKilled)
                {
                    Level2WallNearRedLight.Destruct();
                    Level2WallNearRedLight = null;
                }

                // DESTROY END WALLS
                if (level2EndWall != null && level2EndWall2 != null && LJP != null && LJP.Lv2RedLightKilled && LJP.Lv2AllyFound && LJP.Lv2BossKilled)
                {
                    level2EndWall.Destruct();
                    level2EndWall2.Destruct();

                    level2EndWall = null;
                    level2EndWall2 = null;
                }

                //LV3 WALLS//

                //DESTORY WALLS IN LEVEL 3//
                if (level3SecondRoomDoor != null && LJP != null && LJP.Lv3FirstRedLightKilled)
                {
                    level3SecondRoomDoor.Destruct();
                    level3SecondRoomDoor = null;
                }

                
                //DESTORYS DOOR LEADING TO THIRD ROOM//
                if (level3SecondRoomDoorToThird != null && LJP != null && LJP.Lv3FirstLevel2ShieldEnemyKilled)
                {
                    level3SecondRoomDoorToThird.Destruct();
                    level3SecondRoomDoorToThird = null;
                }


                //DESTORYS DOOR LEADING TO FOURTH ROOM//
                if (level3ThirdRoomTop != null && LJP != null && LJP.Lv3RedLight3Killed)
                {
                    level3ThirdRoomTop.Destruct();
                    level3ThirdRoomTop = null;
                }

                
                //DESTORYS FINAL DOOR/
                if (Level3DoorToNPC!= null && LJP != null && LJP.Lv3FinalShieldEnemKilled)
                {
                    Level3DoorToNPC.Destruct();
                    Level3DoorToNPC = null;
                }



                //DESTORYS FINAL DOOR TO EXIT ROOM/
                if (Level3FinalDoor!= null && LJP != null && LJP.Lv3FinalDoorCanOpen)
                {
                    Level3FinalDoor.Destruct();
                    Level3FinalDoor = null;
                }



                //LEVEL 4 WALLS//

                if(Level4Wall != null && LJP != null && LJP.Lv4DestroyedLvl3Enem)
                {
                        
                    Level4Wall.Destruct();
                    Level4Wall = null;


                }




                //LEVEL 5 WALLS//


                //BEGINNING WALL/DOOR
                if(BeginningLv5Wall != null && LJP != null && LJP.Lv5MiniBossKilled)
                {
                        
                    BeginningLv5Wall.Destruct();
                    BeginningLv5Wall = null;


                }


                //FINAL BOSS DOOR//
                if(Lv5FinalBossDoor!= null && LJP != null && LJP.Lv5FinalBossKilled)
                {
                        
                    Lv5FinalBossDoor.Destruct();
                    Lv5FinalBossDoor = null;


                }








                break;
            }
        }
    }

    // HELPER FUNCTION
    ThingInfo SpawnWallAt(Vector2 pos)
    {
        SpawnRequest req =  new SpawnRequest("WallBlock");

        ThingOption wallOpt = God.Library.GetThing(req);

        if (wallOpt == null)
        {
            //God.LogError("No WallBlock found in library");

            return null;
        }

        ThingInfo wall = new ThingInfo(wallOpt);

        wall.Spawn(pos);

        return wall;
    }
}




public class RemoveExit : Trait
{
    ThingInfo shieldEnemy;

    bool FoundEnemy = false;
    bool FinalBossDead = false;

    Level_JuliusP LJP;

    public RemoveExit()
    {
        Type = Traits.RemoveExit_JuliusP;

        AddListen(EventTypes.Update);
        AddListen(EventTypes.OnSpawn);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
            {
                LJP = God.LB as Level_JuliusP;

                shieldEnemy = null;

                FoundEnemy = false;

                FinalBossDead = false;

                break;
            }

            case EventTypes.Update:
            {
                int Level = God.Session.Level;

                if (Level != 5)
                    break;

                LJP = God.LB as Level_JuliusP;

                // FIND THE SHIELD ENEMY
                if (!FoundEnemy)
                {
                    FindShieldEnemy();
                }

                // CHECK IF DEAD
                if (FoundEnemy && (shieldEnemy == null || shieldEnemy.Thing == null) && !FinalBossDead)
                {
                    //Debug.Log("Shield Enemy is dead");

                    LJP.Lv5FinalBossKilled = true;

                    FinalBossDead = true;
                }

                break;
            }
        }
    }

    void FindShieldEnemy()
    {
        foreach (ThingController t in God.GM.Things)
        {
            if (t?.Info == null)
                continue;

            //Debug.Log(t.gameObject.name);

            if (t.gameObject.name.Contains("Lv4.Shield Enemy"))
            {
                shieldEnemy = t.Info;

                FoundEnemy = true;

                //Debug.Log("Shield Enemy cached");

                break;
            }
        }
    }
}
