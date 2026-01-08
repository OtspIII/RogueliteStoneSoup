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