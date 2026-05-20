using UnityEngine;

public class PermBarrierTrait : Trait
{
    public PermBarrierTrait()
    {
        Type = Traits.PermBarrier;
        AddPreListen(EventTypes.Damage);
    }

    // runs when stacked
    public override void ReUp(TraitInfo old, EventInfo n)
    {
        if (n == null)
            return;
        
        float addedBarrier = n.GetFloat(NumInfo.Default, 0f);
        if (addedBarrier < 0)
            return;
        
        float currentBarrier = old.GetFloat(NumInfo.Default, 0f);
        old.Set(NumInfo.Default, currentBarrier + addedBarrier);
    }

    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Damage:
            {
                float incomingDamage = e.GetFloat();
                if (incomingDamage <= 0f)
                    return;

                float currentBarrier = i.GetFloat(NumInfo.Default, 0f);
                if (currentBarrier <= 0f)
                    return;

                float newBarrier = Mathf.Max(0f, currentBarrier - incomingDamage);
                i.Set(NumInfo.Default, newBarrier);

                string ownerName = i.Who != null && i.Who.Thing != null
                    ? i.Who.Thing.gameObject.name
                    : "Unknown";

                Debug.Log($"[PermBarrier] {ownerName}: incoming damage {incomingDamage:F1}, barrier now {newBarrier:F1}");

                e.Abort = true;
                break;
            }
        }
    }
}