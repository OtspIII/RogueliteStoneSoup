using UnityEngine;
using UnityEngine.SceneManagement;

public class HoldableTrait : Trait
{
    public HoldableTrait()
    {
        Type = Traits.Holdable;
        TakeListen.Add(EventTypes.GetDefaultAttack);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.GetDefaultAttack:
            {
                e.Set(EnumInfo.DefaultAction, (int)i.Get<Actions>(EnumInfo.DefaultAction));
                break;
            }
        }
    }
}


public class PickupTrait : Trait
{
    public PickupTrait()
    {
        Type = Traits.Pickup;
        TakeListen.Add(EventTypes.TryPickup);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.TryPickup:
            {
                Debug.Log("TRY PICKUP: " + i.Who.Name + " / " + e.GetActor()?.Name);
                break;
            }
        }
    }
}

public class HealPackTrait : Trait
{
    public HealPackTrait()
    {
        Type = Traits.HealPack;
        TakeListen.Add(EventTypes.TryPickup);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.TryPickup:
            {
                EventInfo heal = God.E(EventTypes.Healing).Set(5);
                e.GetActor().TakeEvent(heal,true);
                i.Who.Destruct();
                break;
            }
        }
    }
}