using UnityEngine;

public class ProximityExplode_SarahS : Trait
{
    public ProximityExplode_SarahS()
    {
        Type = Traits.SarahS1;
        
        AddListen(EventTypes.OnSee);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSee:
            {
                float range = i.Get(NumInfo.MiscA, 2f);
                float damage = i.Get(NumInfo.Default, 3f);

                if (i.Who == null || i.Who.Thing == null) return;
                ThingInfo t = e.GetThing();


                if (t.Team == GameTeams.Enemy)
                {
                    i.Who.Target = t;
                }
                
                i.Who.Destruct(i.Who);

                break;
            }
        }
    }
    
}
