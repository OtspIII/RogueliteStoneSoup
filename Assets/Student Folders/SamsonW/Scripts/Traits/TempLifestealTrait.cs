using UnityEngine;

public class TempLifestealTrait : Trait
{
    public TempLifestealTrait()
    {
        Type = Traits.TempLifesteal;
        AddListen(EventTypes.DamageDealt);
        AddListen(EventTypes.Update);
    }

    public override void ReUp(TraitInfo old, EventInfo n)
    {
        if (n == null)
            return;

        float addedDuration = n.GetFloat(NumInfo.Time, 0f);
        if (addedDuration <= 0f)
            return;

        float currentDuration = old.GetFloat(NumInfo.Time, 0f);
        old.Set(NumInfo.Time, currentDuration + addedDuration);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.DamageDealt:
            {
                if (i.Who == null)
                    return;
                
                ThingInfo victim = e.GetThing();
                float damage = e.GetN();
                if (damage <= 0f)
                    return;
                if (victim == i.Who)
                    return;

                i.Who.TakeEvent(God.E(EventTypes.Healing).Set(damage)); // heal for damage dealt

                string attackerName = i.Who.Thing != null ? i.Who.Thing.gameObject.name : i.Who.Name;
                string victimName = victim != null && victim.Thing != null ? victim.Thing.gameObject.name : "Unknown";
                Debug.Log($"[TempLifesteal] {attackerName} healed {damage} after hitting {victimName}");
                break;
            }
            case EventTypes.Update:
            {
                float duration = i.GetFloat(NumInfo.Time, 0f);
                duration -= Time.deltaTime;
                i.Set(NumInfo.Time, duration);

                if (duration <= 0f)
                {
                    if (i.Who != null && i.Who.Thing != null)
                        Debug.Log($"[TempLifesteal] expired on {i.Who.Thing.gameObject.name}");

                    i.Who.RemoveTrait(Type);
                }
                break;
            }
        }
    }
}