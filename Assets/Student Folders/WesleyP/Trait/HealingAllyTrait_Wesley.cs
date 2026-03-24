using UnityEngine;

public class HealingAllyTrait_Wesley : Trait
{

    public HealingAllyTrait_Wesley()
    {
        Type = Traits.HealZone;
        AddListen(EventTypes.OnInside);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnInside:
                {
                    float timer = i.Get(NumInfo.Speed, 1);
                    float amt = i.Get(NumInfo.Default, 1);

                    HitboxController hb = e.GetHitbox();
                    foreach (ThingController tc in hb.Touching.ToArray())
                    {
                        ThingInfo t = tc.Info;
                        if (amt > 0) t.TakeEvent(God.E(EventTypes.Healing).Set(amt));
                    }
                    hb.Timer = timer;
                    return;
                }
            default: return;
        }
    }
}
