using UnityEngine;

public class ProximityExplode_SarahS : Trait
{
    public ProximityExplode_SarahS()
    {
        Type = Traits.ProximityExplodeSarahS;
        
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnTouch:
            {
                if (e.Collision == null || e.Collision.Other == null) return;
                ThingInfo bumper = e.Collision.Other.Info;

                if (i.Who == null || i.Who.Thing == null) return;
                if (bumper.Team == GameTeams.Neutral)
                {
                    God.Library.GetGnome("Blood").Spawn(bumper.Thing.transform.position, 15);
                    bumper.TakeEvent(God.E(EventTypes.Death).Set(i.Who));
                }

                break;
            }
        }
    }
    
}
