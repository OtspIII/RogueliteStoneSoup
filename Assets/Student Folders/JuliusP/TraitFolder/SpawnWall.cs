using UnityEngine;

public class SpawnWall : Trait
{
    ThingInfo spawnedWall;


    //LEVEL 2 WALLS//
   
    // LEVEL 2 WALL
    ThingInfo level2Wall;


    ThingInfo level2EndWall, level2EndWall2, Level2WallNearRedLight, Level2WallNearAlly;

    

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
                // FIRST WALL

                if(Level == 1)
                {
                        
                spawnedWall = SpawnWallAt(new Vector2(75.96f, 5.12f));
                }
               
                break;
            }

            case EventTypes.Update:
            {
                var level = God.LB as Level_JuliusP;

                int Level = God.Session.Level;

                // REMOVE FIRST WALL
                if (level != null && level.CanLinkToLootRoom)
                {
                    if (spawnedWall != null)
                    {
                        spawnedWall.Destruct();
                        spawnedWall = null;
                    }
                }

                // LEVEL 2 WALL
                if (Level == 2)
                {
                    // SPAWN ONLY ONCE
                    if (level2Wall == null)
                    {
                        level2Wall = SpawnWallAt(new Vector2(-23.01f, -6.22f));
                    }

                    // DESTROY WHEN BOSS DIES
                    if (level2Wall != null && LJP != null && LJP.Lv2BossKilled)
                    {
                        level2Wall.Destruct();
                        level2Wall = null;

                        //Debug.Log("Destroyed Level 2 Wall");
                    }


                    //SPAWN END WALL + ALL OTHER LEVEL 2 WALLS//
                    if (level2EndWall == null && level2EndWall2 == null && Level2WallNearRedLight == null && Level2WallNearAlly == null)
                    {
                        level2EndWall = SpawnWallAt(new Vector2(41.77f, 38.71f));

                        level2EndWall2 = SpawnWallAt(new Vector2(44.58f, 38.71f));

                        Level2WallNearRedLight = SpawnWallAt(new Vector2(80.15f, 54.47f));

                        Level2WallNearAlly = SpawnWallAt(new Vector2(75.98f, 4.96f));
                    }


                    //LOGIC TO DESTORY WALLS IN LEVEL 2//


                    if (Level2WallNearAlly != null && LJP != null && LJP.Lv2AllyFound)
                    {
                            
                        Level2WallNearAlly.Destruct();
                        Level2WallNearAlly = null;

                    }



                    if (Level2WallNearRedLight != null && LJP != null && LJP.Lv2RedLightKilled)
                    {
                            
                        Level2WallNearRedLight.Destruct();
                        Level2WallNearRedLight = null;

                    }


                    if (level2EndWall != null && level2EndWall2 != null && LJP != null && LJP.Lv2RedLightKilled && LJP.Lv2AllyFound && LJP.Lv2BossKilled)
                    {
                            
                       level2EndWall.Destruct();
                       level2EndWall2.Destruct();

                       level2EndWall = null;
                       level2EndWall2 = null;






                    }








                }

                break;
            }
        }
    }

    // HELPER FUNCTION
    ThingInfo SpawnWallAt(Vector2 pos)
    {
        SpawnRequest req = new SpawnRequest("WallBlock");

        ThingOption wallOpt = God.Library.GetThing(req);

        if (wallOpt == null)
        {
            God.LogError("No WallBlock found in library");
            return null;
        }

        ThingInfo wall = new ThingInfo(wallOpt);

        wall.Spawn(pos);

        return wall;
    }
}