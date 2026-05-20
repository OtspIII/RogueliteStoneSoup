using UnityEngine;

public class LockedExit_qixiangdong : Trait
{
    public LockedExit_qixiangdong()
    {
        Type = Traits.LockedExit_qixiangdong;
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.Message);
        
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type) { 
            case EventTypes.OnSpawn:
                if (i.Who.Thing != null)
                    i.Who.Thing.Body.SR.color=Color.red;
                SpawnBarrier(i);
                break;
            
            case EventTypes.Message:
                if (e.GetString() != "UnlockExit") return;

                if(i.Who.Thing != null)
                    i.Who.Thing.Body.SR.color=Color.green;
                RemoveBarrier(i);

                i.Who.TakeEvent(God.E(EventTypes.LoseTrait).Set(Traits.LockedExit_qixiangdong));
                i.Who.TakeEvent(God.E(EventTypes.GainTrait).Set(Traits.Exit));
                break;
        }
    }

    void SpawnBarrier(TraitInfo i)
    {
        ThingOption wall = God.Library.GetThing(new SpawnRequest(GameTags.Wall));
        if(wall == null) return;

        Vector2 exitPos= (Vector2)i.Who.Thing.Body.transform.position;
        int width = 4;
        int height = 5;
        for (int x = -width/2; x <= width/2; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                ThingInfo wallInfo = wall.Create();
                ThingController wallThing = wallInfo.Spawn(exitPos + new Vector2(x, -y));
                wallInfo.Name = "BulletHellBarrier";
            }

        }
    }

    void RemoveBarrier(TraitInfo i)
    {
        foreach(ThingController tc in God.GM.Things)
        {
            if(tc == null)continue;
            if(tc.gameObject.name == "BulletHellBarrier")
            {
                tc.Info.TakeEvent(God.E(EventTypes.OnDestroy));
            }
        }
        
    }

}
