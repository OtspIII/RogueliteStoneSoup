using System.Collections;
using UnityEngine;

public class CursedObject_SarahS : Trait
{
    public CursedObject_SarahS()
    {
        Type = Traits.CursedObjectSarahS;
        AddListen(EventTypes.OnPickup);
        AddListen(EventTypes.OnDrop);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnPickup:
            {
                ThingInfo holder = e.GetThing();
                if (holder == null) return;
                
                i.SetBool(BoolInfo.Default, true);
                God.C(CurseLoop(i, holder));
                break;
            }
            case EventTypes.OnDrop:
            {
                i.SetBool(BoolInfo.Default, false);
                break;
            }
        }
    }

    private IEnumerator CurseLoop(TraitInfo i, ThingInfo holder)
    {
        float timeUntilDrain = i.GetFloat(NumInfo.Default, 3f);
        float damage = i.GetFloat(NumInfo.Max, 0.5f);
        float drainInterval = i.GetFloat(NumInfo.Min, 3f);

        while (timeUntilDrain > 0)
        {
            if (!i.GetBool(BoolInfo.Default)) yield break;
            timeUntilDrain -= Time.deltaTime;
            yield return null;
        }
        
        float drainTimer = drainInterval;
        while (i.GetBool(BoolInfo.Default))
        {
            drainTimer -= Time.deltaTime;
            if (drainTimer <= 0)
            {
                holder.TakeEvent(God.E(EventTypes.Damage).Set(damage).Set(i.Who));
                drainTimer = drainInterval;
            }
            yield return null;
        }
    }
}
