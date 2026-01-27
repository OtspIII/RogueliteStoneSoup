using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Parser 
{
    public static bool Setup = false;
    public static Dictionary<Traits, Trait> TraitDict = new Dictionary<Traits, Trait>();
    public static List<Authors> AllAuthors = new List<Authors>();
    
    public static void Init()
    {
        if (Setup) return;
        Setup = true;
        foreach (Authors a in Enum.GetValues(typeof(Authors)))
        {
            if((int)a <= 11) continue;
            AllAuthors.Add(a);
        }
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

    public static LevelBuilder GetLB(Authors a)
    {
        if (a == Authors.Universal) a = AllAuthors[Random.Range(0,AllAuthors.Count)];
        switch (a)
        {
            case Authors.MishaF: return new LevelBuilder();
            default: return new LevelBuilder();
        }
    }
}

public enum Authors
{
    None=00,
    Universal=01,
    MishaF=11,
    AdamD=20,
    AlejandroM=25,
    ElioR=30,
    JaidenB=35,
    JuliusP=40,
    MazK=45,
    MichaelT=50,
    QixiangD=55,
    RaphaelC=60,
    SabahE=65,
    SamsonW=70,
    SarahS=75,
    TracyH=80,
    WesleyP=90,
    YuChen=95,
}

public enum Traits
{
    //Basic Traits:  0###
    None            =0000,
    Actor           =0001,
    Health          =0002,
    Drop            =0003,
    Despawn         =0004,
    Player          =0100,
    Exit            =0200,
    DamageZone      =0201,
    Tool            =0300,
    Pickupable      =0301,
    HealPack        =0310,
    GoldCoins       =0311,
    LimitedUse      =0312,
    Stackable       =0313,
    Projectile      =0400,
    OnFire          =0500,
    //Misha Traits:  11##
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
