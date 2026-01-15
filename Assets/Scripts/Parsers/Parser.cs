using System.Collections.Generic;
using UnityEngine;

public static class Parser 
{
    public static bool Setup = false;
    public static Dictionary<Traits, Trait> TraitDict = new Dictionary<Traits, Trait>();
    
    public static void Init()
    {
        if (Setup) return;
        Setup = true;
        TraitDict.Add(Traits.Health,new HealthTrait());
        TraitDict.Add(Traits.Actor,new ActorTrait());
        TraitDict.Add(Traits.Player,new PlayerTrait());
        TraitDict.Add(Traits.Tool,new ToolTrait());
        TraitDict.Add(Traits.Pickupable,new PickupableTrait());
        TraitDict.Add(Traits.HealPack,new HealPackTrait());
        TraitDict.Add(Traits.GoldCoins,new GoldCoinsTrait());
        TraitDict.Add(Traits.Projectile,new ProjectileTrait());
        TraitDict.Add(Traits.Exit,new ExitTrait());
        TraitDict.Add(Traits.DamageZone,new DamageZoneTrait());
        TraitDict.Add(Traits.Drop,new DropTrait());
        TraitDict.Add(Traits.Despawn,new DespawnTrait());
        TraitDict.Add(Traits.LimitedUse,new LimitedUseTrait());
        TraitDict.Add(Traits.Stackable,new StackableTrait());
    }
    
    public static ActionScript Get(Actions act,ThingController who,EventInfo e=null)
    {
        switch (act)
        {
            case Actions.Idle: return new IdleAction(who,e);
            case Actions.Stun: return new StunAction(who,e);
            case Actions.Swing: return new SwingAction(who,e);
            case Actions.Shoot: return new ShootAction(who,e);
            case Actions.Lunge: return new LungeAction(who,e);
            case Actions.Patrol: return new PatrolAction(who,e);
            case Actions.Chase: return new ChaseAction(who,e);
            case Actions.Use: return new UseAction(who, e);
        }
        Debug.Log("UNCAUGHT ACTION: " + act);
        return new IdleAction(who,e);
    }
    
    public static Trait Get(Traits t)
    {
        if (TraitDict.TryGetValue(t, out Trait r)) return r;
        Debug.Log("ERROR MISSING TRAIT: " + t+"\nMust add to TraitManager");
        return null;
    }
}


public enum Actions
{
    //The Basics
    None=0,
    Idle=1,
    Stun=2,
    DefaultAction=3,
    DefaultAttack=4,
    Use=5,
    
    //Attack Actions
    Swing=101,
    Lunge=102,
    Shoot=103,
    
    //AI Actions
    Patrol=201,
    Chase=202,
    
    //Misc Actions
    // SelfDestruct=301,
}

public enum ProjTypes
{
    None=0,
    Arrow=1,
}