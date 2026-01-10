using UnityEngine;
using UnityEngine.SceneManagement;

public class HoldableTrait : Trait
{
    public HoldableTrait()
    {
        Type = Traits.Holdable;
        TakeListen.Add(EventTypes.GetDefaultAttack);
        TakeListen.Add(EventTypes.Interact);
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
            case EventTypes.Interact:
            {
                ThingInfo who = e.GetActor();
                who.SetWeapon(i.Who);
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
        TakeListen.Add(EventTypes.Interact);
        TakeListen.Add(EventTypes.PlayerTouched);
        TakeListen.Add(EventTypes.PlayerLeft);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Interact:
            {
                i.Who.TakeEvent(God.E(EventTypes.OnPickup).Set(e.GetActor()));
                i.Who.Destruct();
                break;
            }
            case EventTypes.PlayerTouched:
            {
                i.Who.Thing.Icon.gameObject.SetActive(true);
                break;
            }
            case EventTypes.PlayerLeft:
            {
                i.Who.Thing.Icon.gameObject.SetActive(false);
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
        TakeListen.Add(EventTypes.OnPickup);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnPickup:
            {
                EventInfo heal = God.E(EventTypes.Healing).Set(5);
                e.GetActor().TakeEvent(heal,true);
                break;
            }
        }
    }
}