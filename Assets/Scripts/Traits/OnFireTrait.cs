using UnityEngine;

public class OnFireTrait : Trait
{
    public OnFireTrait()
    {
        Type = Traits.OnFire;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                float dur = i.GetFloat(NumInfo.Time,3);
                float ping = dur % i.GetFloat(NumInfo.Speed,1);
                ping -= Time.deltaTime;
                if(ping <= 0) i.Who.TakeEvent(God.E(EventTypes.Damage).Set(i.GetFloat(NumInfo.Default,1))
                    .Set(i.GetThing()).Set(i.Get(StrInfo.DType)));
                dur -= Time.deltaTime;
                i.Set(NumInfo.Time, dur);
                if (dur <= 0) i.Who.RemoveTrait(Type);
                break;
            }
        }
    }
}