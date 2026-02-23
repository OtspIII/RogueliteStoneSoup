using Unity.VisualScripting;
using UnityEngine;

public class Lighting_RaphaelC : Trait
{
    public Lighting_RaphaelC()
    {
        Type = Traits.Lighting_RaphaelC;
        AddListen(EventTypes.OnUseStart);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnUseStart:
            {
                float range = i.Get(NumInfo.Default, 11);
                float dmg = i.Get(NumInfo.Default,1);

                Vector3 center = i.Who.Thing.transform.position;
                Collider2D[] hits = Physics2D.OverlapCircleAll(center, range);
                foreach (Collider2D hit in hits)
                {
                    ThingController tc = hit.GetComponent<ThingController>();
                    if (tc != null)
                    {
                        ThingInfo t = tc.Info;
                        if (t.Has(Traits.Player))
                            continue;
                        
                        tc.Info.TakeEvent(God.E(EventTypes.Damage).Set(dmg).Set(i.Who));
                    }
                }
                return;
            }
            default: return;
        }
    }
}