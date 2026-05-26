using UnityEngine;

public class Level_QixiangD : LevelBuilder
{
    public Level_QixiangD()
    {
        Author = Authors.QixiangD;
    }
    public override void Customize()
    {
        base.Customize();
        Size = new Vector2Int(1, 1);
        RoomSize = new Vector2Int(41, 41);
        LinkOdds = 0f;
        Boss = null;
    }
    public override void FindQuotas()
    {
        FindThings();
    }
    public override void SpawnThings()
    {
        base.SpawnThings();
        SpawnManager();
        SpawnArchers();
    }
    void SpawnManager()
    {
        ThingOption manager = God.Library.GetThing("BulletHellManager");
        if (manager == null)
        {
            Debug.LogWarning("BulletHellManager not found");
            return;
        }
        ThingInfo m = manager.Create();
        m.Spawn(GetRoomCentre());
    }
    void SpawnArchers()
    {
        int level = God.Session.Level;

        Vector2[] positions;

        if (level >= 3)
        {
            positions = new Vector2[]
            {
            new Vector2(40.5f, 5f),
            new Vector2(40.5f, -5f),
            new Vector2(35f, 0.5f),
            new Vector2(46f, 0.5f),
            new Vector2(37f, 4f),
            new Vector2(44f, -4f),
            new Vector2(37f, -4f),
            new Vector2(44f, 4f),
            
            };
        }
        else if (level >= 2)
        {
            positions = new Vector2[]
            {
            new Vector2(40.5f, 5f),
            new Vector2(40.5f, -5f),
            new Vector2(35f, 0.5f),
            new Vector2(46f, 0.5f),
            new Vector2(37f, 4f),
            new Vector2(44f, -4f),
            new Vector2(37f, -4f),
            new Vector2(44f, 4f),
           
            };
        }
        else
        {
            positions = new Vector2[]
            {
            new Vector2(40.5f, 5f),
            new Vector2(40.5f, -5f),
            new Vector2(35f, 0.5f),
            new Vector2(46f, 0.5f),
            
            };
        }

        SpawnAtPositions(positions);
    }
    Vector2 GetRoomCentre()
    {
        
        foreach (ThingController tc in God.GM.Things)
        {
            if (tc == null) continue;
            if (tc.Info.Has(Traits.Player))
            {
                Debug.Log("Found player at: " + tc.transform.position);
                return tc.transform.position;
            }
        }
        
        return Vector2.zero;
    }
    void SpawnAtPositions(Vector2[] positions)
    {
        int level = God.Session.Level;

        foreach (Vector2 pos in positions)
        {
            string tag;

            if (level >= 5)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < 0.85f)
                    tag = "SneakyNinja";
                else if (rand < 0.95f)
                    tag = "BurstNinja";
                else
                    tag = "SpinNinja";
            }
            else if (level >= 3)
            {
                float rand = Random.Range(0f, 1f);
                if (rand < 0.9f)
                    tag = "SneakyNinja";
                else
                    tag = "BurstNinja";
            }
            else
            {
                tag = "SneakyNinja";
            }

            ThingOption ninja = God.Library.GetThing(tag);
            if (ninja == null)
            {
                Debug.LogWarning(tag + " not found");
                continue;
            }
            ThingInfo n = ninja.Create();
            if (n == null) continue;
            n.Spawn(pos);
        }
    }
}