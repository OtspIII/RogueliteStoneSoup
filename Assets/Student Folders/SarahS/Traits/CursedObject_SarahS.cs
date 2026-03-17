using UnityEngine;

public class CursedObject_SarahS : Trait
{
    public CursedObject_SarahS()
    {
        Type = Traits.CursedObjectSarahS;
        AddListen(EventTypes.OnPickup);
        AddListen(EventTypes.OnDrop);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnPickup:
            {
                ThingInfo holder = e.GetThing(ThingEInfo.Source);
                if (holder == null) return;
                
                i.SetThing(ThingEInfo.Target, holder);
                i.SetFloat(NumInfo.Time, i.GetFloat(NumInfo.Default, 3f));
                i.SetBool(BoolInfo.Default, true);
                break;
            }
            case EventTypes.OnDrop:
            {
                i.SetBool(BoolInfo.Default, false);
                i.SetThing(ThingEInfo.Target, null);
                break;
            }
            case EventTypes.Update:
            {
                if (!i.GetBool(BoolInfo.Default)) return;
                
                ThingInfo holder = e.GetThing(ThingEInfo.Target);
                if (holder == null) return;

                float timeUntilDrain = i.GetFloat(NumInfo.Time, 0f);
                if (timeUntilDrain > 0f)
                {
                    timeUntilDrain -= Time.deltaTime;
                    i.SetFloat(NumInfo.Time, timeUntilDrain);
                    return;
                }
                
                float drainTimer = i.GetFloat(NumInfo.Speed, 0f);
                drainTimer -= Time.deltaTime;

                if (drainTimer <= 0f)
                {
                    float damage = i.GetFloat(NumInfo.Max, 1f);
                    holder.TakeEvent(God.E(EventTypes.Damage)
                        .Set(damage)
                        .Set(i.Who)
                        .Set(StrInfo.DType, "Curse"));

                    float drainInterval = i.GetFloat(NumInfo.Min, 1f);
                    i.SetFloat(NumInfo.Speed, drainInterval);
                }
                else
                {
                    i.SetFloat(NumInfo.Speed, drainTimer);
                }

                break;
            }
        }
    }
}
