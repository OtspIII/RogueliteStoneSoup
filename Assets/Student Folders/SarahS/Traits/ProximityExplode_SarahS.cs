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
                float range = i.Get(NumInfo.MiscA, 2f);
                float damage = i.Get(NumInfo.Default, 3f);

                if (i.Who == null || i.Who.Thing == null) return;

                ThingInfo bumber = e.GetThing(ThingEInfo.Source);
                if (bumber == null) return;


                if (bumber.Team == GameTeams.Enemy) 
                    i.Who.Destruct(i.Who);

                break;
            }
        }
    }
    
}
