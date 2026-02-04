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
                e.Set(ActionInfo.DefaultAction, i.Get(ActionInfo.DefaultAction));
                break;
            }
            case EventTypes.OnUse:
            {
                e.GetThing().TakeEvent(God.E(EventTypes.StartAction).Set(ActionInfo.Action,Actions.DefaultAttack));
                break;
            }
            case EventTypes.GetDamage:
            {
                e.Set(i.GetFloat());
                break;
            }
            case EventTypes.GetProjectile:
            {
                ThingOption p = i.SpawnReq.FindThing();
                e.Set(p);
                break;
            }
            case EventTypes.OnHoldStart:
            {
                e.GetThing().AttackRange = i.Get(NumInfo.Distance, 1.5f);
                break;
            }
            case EventTypes.GetActSpeed:
            {
                if(e.Get(ActionInfo.Action) == Actions.Shoot) Debug.Log("GIVE ACT SPEED");
                e.Set(NumInfo.Default, i.Get(NumInfo.Speed, 1));
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
                EventInfo pu = God.E(EventTypes.OnPickup).Set(e.GetThing());
                i.Who.TakeEvent(pu,true);
                if (pu.Abort) break;
                
                e.GetThing().TakeEvent(God.E(EventTypes.DidPickup).Set(i.Who));
                i.Who.DestroyForm();
                break;
            }
            case EventTypes.PlayerTouched:
            {
                i.Who.Thing?.Icon.gameObject.SetActive(true);
                break;
            }
            case EventTypes.PlayerLeft:
            {
                i.Who.Thing?.Icon.gameObject.SetActive(false);
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
        AddListen(EventTypes.OnUse);
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
            case EventTypes.OnUse:
            {
                EventInfo heal = God.E(EventTypes.Healing).Set(5);
                e.GetThing().TakeEvent(heal,true);
                // e.GetActor().
                // e.GetActor().DropHeld();
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
                e.GetThing().TakeEvent(God.E(EventTypes.AddScore).Set(i.GetN(NumInfo.Default,1)).Set(i.Who));
                i.Who.Destruct();
                break;
            }
        }
    }
}


public class LimitedUseTrait : Trait
{
    public LimitedUseTrait()
    {
        Type = Traits.LimitedUse;
        AddPreListen(EventTypes.OnUse);
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.OnUse);
        AddListen(EventTypes.OnUseEnd);
        AddListen(EventTypes.ShownName,5);
        AddListen(EventTypes.ChangeStack);
    }

    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnUse:
            {
                int uses = i.GetInt();
                if (uses <= 0) e.Abort = true;
                break;
            }
        }
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                TraitInfo stack = i.Who.Get(Traits.Stackable);
                if (stack != null)
                {
                    int uses = Mathf.Max(i.GetInt(), stack.GetInt());
                    i.Set(uses);
                    stack.Set(uses);
                }
                break;
            }
            case EventTypes.OnUse:
            {
                i.Who.TakeEvent(God.E(EventTypes.ChangeStack).Set(-1).Set(e.GetThing()),true);
                break;
            }
            case EventTypes.OnUseEnd:
            {
                int uses = i.GetInt();
                if (uses <= 0) e.GetThing().DropHeld(true);
                break;
            }
            case EventTypes.ShownName:
            {
                string r = e.GetString();
                int uses = i.GetInt();
                r = uses + "x " + r;
                e.Set(r);
                e.Abort = true;
                break;
            }
            case EventTypes.ChangeStack:
            {
                i.Change(e.GetInt());
                if (e.GetThing() == God.Player)
                    God.GM.UpdateInvText();
                break;
            }
        }
    }
}


public class StackableTrait : Trait
{
    public StackableTrait()
    {
        Type = Traits.Stackable;
        AddListen(EventTypes.OnPickup,1);
        AddListen(EventTypes.ChangeStack);
        AddListen(EventTypes.ShownName,4);
    }


    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnPickup:
            {
                if (e.GetThing() != God.Player)
                {
                    e.Abort = true;
                    break;
                }
                foreach (ThingInfo t in God.Session.PlayerInventory)
                {
                    if (t.Type == i.Who.Type)
                    {
                        t.TakeEvent(God.E(EventTypes.ChangeStack).Set(i.GetInt()).Set(e.GetThing()));
                        e.Abort = true;
                        i.Who.Destruct();
                        break;
                    }
                }
                break;
            }
            case EventTypes.ChangeStack:
            {
                float n = i.Change(e.GetInt());
                //Huh, should the item being destroyed flow from here?
                //It needs to happen 'late' for usable items, though
                if (!i.Who.Has(Traits.LimitedUse))
                {
                    if (n <= 0)
                    {
                        i.Who.Destruct();
                    }

                    if (e.GetThing() == God.Player)
                        God.GM.UpdateInvText();
                }

                break;
            }
            case EventTypes.ShownName:
            {
                string r = e.GetString();
                int uses = i.GetInt();
                r = uses + "x " + r;
                e.Set(r);
                e.Abort = true;
                break;
            }
        }
    }
}
