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
            if((int)a < 20) continue;
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
        TraitDict.Add(Traits.Hostile,new HostileTrait());
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        TraitDict.Add(Traits.Lighting_RaphaelC,new Lighting_RaphaelC());
        TraitDict.Add(Traits.Teleport, new TeleportTrait());
=======
        TraitDict.Add(Traits.SarahS1, new ProximityExplode_SarahS());
>>>>>>> Stashed changes
=======
        TraitDict.Add(Traits.SarahS1, new ProximityExplode_SarahS());
>>>>>>> Stashed changes
    }
    
    public static ActionScript Get(Actions act,ThingInfo who,EventInfo e=null)
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
        God.LogError("UNCAUGHT ACTION: " + act);
        return new IdleAction(who,e);
    }
    
    public static Trait Get(Traits t)
    {
        if (TraitDict.TryGetValue(t, out Trait r)) return r;
        God.LogError("ERROR MISSING TRAIT: " + t+"\nMust add to TraitManager");
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

    /// Okay, so this is pretty silly. If you don't set an Author, it'll read the name of your computer
    /// If you add the name of your computer to this switch statement, it'll auto-set the author to be you if it's unset
    /// This way you won't need to fuss with the CurrentAuthor variable on the GameManager
    /// If you don't know your computer's name, just run "Debug.Log(Environment.UserName);" and it'll show in console
    public static Authors FindAuthor()
    {
        // Debug.Log(Environment.UserName);
        switch (Environment.UserName)
        {
            case "Ipos":case"otspi": return Authors.Universal;
        }
        return Authors.None;
    }
}

public enum Authors
{
    None=00,
    Universal=01,
    Random=02,
    MishaF=11,
    MazK=12,
    AdamD=20,
    AlejandroM=25,
    ElioR=30,
    JaidenB=35,
    JuliusP=40,
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
    Actor           =0001,//Handles Actions
    Health          =0002,//Handles Damage/Healing
    Drop            =0003,//Spawn Thing On Death
    Despawn         =0004,//Vanish After Time Passes
    Player          =0100,//Read Keyboard/Mouse
    Hostile         =0101,//Attacks the player if they see them
    Exit            =0200,//Win Game Upon Touching
    DamageZone      =0201,//Deal Damage Upon Touching
    Tool            =0300,//Can Be Used When Held
    Pickupable      =0301,//Can Be Picked Up
    HealPack        =0310,//Heals When Used
    GoldCoins       =0311,//Gives Points When Touched
    LimitedUse      =0312,//Self Destructs When Used Up
    Stackable       =0313,//Stacks When Picked Up
    Projectile      =0400,//Flies In A Direction
    OnFire          =0500,//I Didn't Finish This Yet
    //Misha Traits:  11##
    Misha1          =1101,
    //AdamD         =20##,
    AdamD1          =2001,
    //AlejandroM    =25##,
    AlejandroM1     =2501,
    //ElioR         =30##,
    ElioR1          =3001,
    //JaidenB       =35##,
    JaidenB1        =3501,
    //JuliusP       =40##,
    JuliusP1        =4001,
    //MazK          =45##,
    MazK1           =4501,
    //MichaelT      =50##,
    MichaelT1       =5001,
    //QixiangD      =55##,
    QixiangD1       =5501,
    //RaphaelC      =60##,
    Lighting_RaphaelC       =6001,
    //SabahE        =65##,
    SabahE1         =6501,
    //SamsonW       =70##,
    SamsonW1        =7001,
    //SarahS        =75##,
    SarahS1         =7501,
    //TracyH        =80##,
    TracyH1         =8001,
    Teleport        =8002,
    //WesleyP       =90##,
    WesleyP1        =9001,
    //YuChen        =95##,
    YuChen1         =9501,
}

public enum Actions
{
    //The Basics
    None            =0,
    Idle            =0001,//Do Nothing, Default Player Action
    Stun            =0002,//Spin Uncontrollably
    DefaultAction   =0003,//Find Your Default Action & Do It
    DefaultAttack   =0004,//Find Your Default Attack & Do It
    Use             =0005,//Generic Item Use Action
    
    //Attack Actions
    Swing           =0101,//Swing A Sword
    Lunge           =0102,//Fly Forward & Deal Impact Damage
    Shoot           =0103,//Fire A Projectile
    
    //AI Actions
    Patrol         =0201,//Walk Randomly
    Chase          =0202,//Run At A Target
    
    //Student Actions
    //AdamD         =20##,
    AdamD1          =2001,
    //AlejandroM    =25##,
    AlejandroM1     =2501,
    //ElioR         =30##,
    ElioR1          =3001,
    //JaidenB       =35##,
    JaidenB1        =3501,
    //JuliusP       =40##,
    JuliusP1        =4001,
    //MazK          =45##,
    MazK1           =4501,
    //MichaelT      =50##,
    MichaelT1       =5001,
    //QixiangD      =55##,
    QixiangD1       =5501,
    //RaphaelC      =60##,
    RaphaelC1       =6001,
    //SabahE        =65##,
    SabahE1         =6501,
    //SamsonW       =70##,
    SamsonW1        =7001,
    //SarahS        =75##,
    SarahS1         =7501,
    //TracyH        =80##,
    TracyH1         =8001,
    //WesleyP       =90##,
    WesleyP1        =9001,
    //YuChen        =95##,
    YuChen1         =9501,
    
}
