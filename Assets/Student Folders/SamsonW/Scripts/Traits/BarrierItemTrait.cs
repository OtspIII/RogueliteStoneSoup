using UnityEngine;

public class BarrierItemTrait : Trait
{
    public BarrierItemTrait()
    {
        Type = Traits.BarrierItem;
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

                float barrierAmount = i.GetFloat(NumInfo.Default, 1f);
                if (barrierAmount <= 0)
                    return;
                
                EventInfo gainBarrier = God.E().Set(NumInfo.Default, barrierAmount);
                
                user.AddTrait(Traits.PermBarrier, gainBarrier);

                Debug.Log($"[BarrierItem] Applied {barrierAmount} barrier to {user.Name}");
                break;
            }
        }
    }
}