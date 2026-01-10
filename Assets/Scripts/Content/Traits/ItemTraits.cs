using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolTrait : Trait
{
    public ToolTrait()
    {
        Type = Traits.Tool;
        TakeListen.Add(EventTypes.GetDefaultAttack);
        TakeListen.Add(EventTypes.OnUse);
        TakeListen.Add(EventTypes.GetDamage);
        TakeListen.Add(EventTypes.GetProjectile);
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
            case EventTypes.OnUse:
            {
                e.GetActor().TakeEvent(God.E(EventTypes.StartAction).SetEnum(EnumInfo.Action,(int)Actions.DefaultAttack));
                break;
            }
            case EventTypes.GetDamage:
            {
                e.Set(i.GetFloat());
                break;
            }
            case EventTypes.GetProjectile:
            {
                e.Set(i.GetOption());
                break;
            }
        }
    }
}


public class PickupableTrait : Trait
{
    public PickupableTrait()
    {
        Type = Traits.Pickupable;
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
                e.GetActor().TakeEvent(God.E(EventTypes.DidPickup).Set(i.Who));
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
        TakeListen.Add(EventTypes.OnUseStart);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            // case EventTypes.OnPickup:
            // {
            //     EventInfo heal = God.E(EventTypes.Healing).Set(5);
            //     e.GetActor().TakeEvent(heal,true);
            //     break;
            // }
            case EventTypes.OnUseStart:
            {
                EventInfo heal = God.E(EventTypes.Healing).Set(5);
                e.GetActor().TakeEvent(heal,true);
                // e.GetActor().
                e.GetActor().DropHeld();
                break;
            }
        }
    }
}