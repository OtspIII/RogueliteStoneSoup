using UnityEngine;

public class LifestealItemTrait : Trait
{
    public LifestealItemTrait()
    {
        Type = Traits.LifestealItem;
        AddListen(EventTypes.OnUse);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnUse:
            {
                ThingInfo user = e.GetThing();
                if (user == null)
                    return;

                float duration = i.GetFloat(NumInfo.Time, 5f);
                if (duration <= 0f)
                    return;

                EventInfo grantLifesteal = God.E().Set(NumInfo.Time, duration);
                user.AddTrait(Traits.TempLifesteal, grantLifesteal);

                Debug.Log($"[LifestealItem] Applied {duration}s lifesteal to {user.Name}");
                break;
            }
        }
    }
}