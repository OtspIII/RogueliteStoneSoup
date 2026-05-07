using UnityEngine;

public class SpawnWall : Trait
{
    ThingInfo spawnedWall;

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
                SpawnRequest req = new SpawnRequest("WallBlock");
                ThingOption wallOpt = God.Library.GetThing(req);

                if (wallOpt == null)
                {
                    God.LogError("No WallBlock found in library");
                    break;
                }

                Vector2 spawnPos = new Vector2(75.96f, 5.12f);

                spawnedWall = new ThingInfo(wallOpt);
                spawnedWall.Spawn(spawnPos);

                break;
            }

            case EventTypes.Update:
            {
                var level = God.LB as Level_JuliusP;

                if (level != null && level.CanLinkToLootRoom)
                {
                    if (spawnedWall != null)
                    {
                        spawnedWall.Destruct();
                        spawnedWall = null;
                    }
                }

                break;
            }
        }
    }
}