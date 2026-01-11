using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolTrait : Trait
{
    public ToolTrait()
    {
        Type = Traits.Tool;
        AddListen(EventTypes.GetDefaultAttack);
        AddListen(EventTypes.OnUse);
        AddListen(EventTypes.GetDamage);
        AddListen(EventTypes.GetProjectile);
        AddListen(EventTypes.OnHoldStart);
        AddListen(EventTypes.GetActSpeed);
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
            case EventTypes.OnHoldStart:
            {
                e.GetActor().AttackRange = i.Get(NumInfo.Distance, 1.5f);
                break;
            }
            case EventTypes.GetActSpeed:
            {
                if(e.Get<Actions>(EnumInfo.Action) == Actions.Shoot) Debug.Log("GIVE ACT SPEED");
                e.Set(NumInfo.Amount, i.Get(NumInfo.Speed, 1));
                e.Set(NumInfo.Max, i.Get(NumInfo.Max, 0));
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
        AddListen(EventTypes.Interact);
        AddListen(EventTypes.PlayerTouched);
        AddListen(EventTypes.PlayerLeft);
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
        AddListen(EventTypes.OnUseStart);
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

public class GoldCoinsTrait : Trait
{
    public GoldCoinsTrait()
    {
        Type = Traits.GoldCoins;
        AddListen(EventTypes.PlayerTouched);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.PlayerTouched:
            {
                e.GetActor().TakeEvent(God.E(EventTypes.AddScore).Set(i.GetN(NumInfo.Amount,1)).Set(i.Who));
                i.Who.Destruct();
                break;
            }
        }
    }
}