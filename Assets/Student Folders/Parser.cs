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
        // MishaF Traits
        TraitDict.Add(Traits.TestTrait_Misha,new TestTrait_Misha());
        
        // AdamD Traits
        TraitDict.Add(Traits.statusEffectOnProjectile, new StatusEffectOnProjectileTrait_AdamD("stuff", 5));
        TraitDict.Add(Traits.StatusResist, new StatusResist());
        TraitDict.Add(Traits.UpgradeTrait, new UpgradeTrait());
        // AlejandroM Traits
        TraitDict.Add(Traits.ShieldPotion_AlejandroM, new ShieldPotion_AlejandroM());
        TraitDict.Add(Traits.SpeedPotion_AlejandroM, new SpeedPotion_AlejandroM());
        // ElioR Traits
        TraitDict.Add(Traits.Barrier, new BarrierTrait_ElioR());
        TraitDict.Add(Traits.ParryTrait_ElioR, new ParryTrait());
        TraitDict.Add(Traits.Blight_ElioR, new Blight_ElioR());
        TraitDict.Add(Traits.BlightGiver_ElioR, new BlightGiver_ElioR());
        TraitDict.Add(Traits.LifeSteal_ElioR, new LifeSteal_ElioR());
        // JaidenB Traits
        TraitDict.Add(Traits.InvertControls, new InvertControlsTrait());
        TraitDict.Add(Traits.Freeze, new FreezeTrait());
        TraitDict.Add(Traits.MoneyDrop, new MoneyDropTrait());
        TraitDict.Add(Traits.Lava, new LavaTrait());
        // Julius Traits
        TraitDict.Add(Traits.Rage, new RageTrait());
        TraitDict.Add(Traits.LowHealthWarrior_JuliusP, new UltimateRage());
        TraitDict.Add(Traits.Dash, new DashTrait());
        TraitDict.Add(Traits.SelfDestruct_JuliusP, new SelfDestruction());
        TraitDict.Add(Traits.IgnoreDamage_JuliusP, new IgnoreDamage());
        TraitDict.Add(Traits.StunNegation_JuliusP, new StunCancel());
        TraitDict.Add(Traits.NoTimerStunNegation_JuliusP, new FullStunNegation());
        TraitDict.Add(Traits.TemporaryDash_JuliusP, new TemporaryDashAbility());
        TraitDict.Add(Traits.TemporaryStunImmunity_JuliusP, new StunDrink());
        TraitDict.Add(Traits.TemporaryDmgResist_JuliusP, new TemporaryDamageResist());
        TraitDict.Add(Traits.GainInvis_JuliusP, new GainInvisibility());
        TraitDict.Add(Traits.MonadoArts_JuliusP, new MonadoPower());
        TraitDict.Add(Traits.Slowed_JuliusP, new Slowed());
        TraitDict.Add(Traits.SlowOnhit_JuliusP, new SlowingProjectileTrait());
        TraitDict.Add(Traits.AlwaysRage_JuliusP, new RageAlwaysOn());
        TraitDict.Add(Traits.ApplyHeavyKnockBack_JuliusP, new HeavyKnockback());
        TraitDict.Add(Traits.DodgeInvis_JuliusP, new Dodge_invis());
        TraitDict.Add(Traits.RedLight, new RedGreen());
        TraitDict.Add(Traits.SpawnWall_JuliusP, new SpawnWall());
        TraitDict.Add(Traits.AddTraitByScore_JuliusP, new AddTraitByScore());
        TraitDict.Add(Traits.Lv2RedLight_JuliusP,new Lv2RedLight());
        TraitDict.Add(Traits.TempInvis_JuliusP, new InvisPotion_JuliusP());
        TraitDict.Add(Traits.ShieldTrait_JuliusP, new ShieldTrait());
        TraitDict.Add(Traits.Lv3RedLight_JuliusP, new Lv3RedLight());
        TraitDict.Add(Traits.DetectDeath_JuliusP, new DetectDeath());
        TraitDict.Add(Traits.Lv3Homing_JuliusP, new Lv3Homing());
        TraitDict.Add(Traits.SelfDestruct2_JuliusP, new SelfDestructV2());
        TraitDict.Add(Traits.RemoveExit_JuliusP, new RemoveExit());

       


       


    


        // MazK Traits
        // MichaelT Traits
        TraitDict.Add(Traits.Bleed_MichaelT, new BleedTrait_MichaelT());
        // QixiangD Traits
        TraitDict.Add(Traits.Sneaky_qixiangdong, new Sneaky_qixiangdong());
        TraitDict.Add(Traits.Thrill_qixiangdong, new Thrill_qixiangdong());
        TraitDict.Add(Traits.BulletHell_qixiangdong, new BulletHell_qixiangdong());
        TraitDict.Add(Traits.LockedExit_qixiangdong, new LockedExit_qixiangdong());
        // RaphaelC Traits
        TraitDict.Add(Traits.Lighting_RaphaelC,new Lighting_RaphaelC());
        TraitDict.Add(Traits.BasicHeal_RaphaelC,new BasicHeal_RaphaelC());
        TraitDict.Add(Traits.KillSpeedBoost,new KillSpeedBoost());
        TraitDict.Add(Traits.WhirlPool_RaphaelC,new WhirlPool_RaphaelC());
        TraitDict.Add(Traits.BossDeath_RaphaelC,new BossDeath_RaphaelC());
        // SabahE Traits
        TraitDict.Add(Traits.SpeedUpSabahE, new SpeedUpTrait_SabahE());
        TraitDict.Add(Traits.RallySabahE, new RallyTrait_SabahE());
        // SamsonW Traits
        TraitDict.Add(Traits.TeleportRandomRoom,new TeleportRandomRoomTrait());
        TraitDict.Add(Traits.DamageReflect,new DamageReflectTrait());
        TraitDict.Add(Traits.HealZone,new HealZoneTrait());
        
        TraitDict.Add(Traits.GrappleHook,new GrappleHookTrait());
        TraitDict.Add(Traits.StackableDagger,new StackableDaggerTrait());
        TraitDict.Add(Traits.DelayedActionAfterStartingAction,new DelayedActionAfterStartingActionTrait());
        TraitDict.Add(Traits.ComboBreak,new ComboBreakTrait());
        TraitDict.Add(Traits.CrowdControlNegation,new CrowdControlNegation());
        TraitDict.Add(Traits.BarrierItem,new BarrierItemTrait());
        TraitDict.Add(Traits.PermBarrier,new PermBarrierTrait());
        TraitDict.Add(Traits.LifestealItem,new LifestealItemTrait());
        TraitDict.Add(Traits.TempLifesteal,new TempLifestealTrait());
        TraitDict.Add(Traits.StartWithItem, new StartWithItemTrait());
        // SarahS Traits
        TraitDict.Add(Traits.ProximityExplodeSarahS,new ProximityExplode_SarahS());
        TraitDict.Add(Traits.SlowMoSarahS,new SlowMo_SarahS());
        TraitDict.Add(Traits.CursedObjectSarahS,new CursedObject_SarahS());
        TraitDict.Add(Traits.MimicEnemySarahS,new MimicEnemy_SarahS());
        TraitDict.Add(Traits.ProtectionCircleSarahS,new ProtectionCircle_SarahS());
        TraitDict.Add(Traits.ProtectionSpellSarahS,new ProtectionSpell_SarahS());
        TraitDict.Add(Traits.DamageFlashSarahS,new DamageFlash_SarahS());
        TraitDict.Add(Traits.FleeSarahS,new Flee_SarahS());
        TraitDict.Add(Traits.ContactDamageSarahS,new ContactDamage_SarahS());
        // TracyH Traits
        TraitDict.Add(Traits.Teleport_TracyH, new TeleportTrait_TracyH());
        TraitDict.Add(Traits.Freeze_TracyH, new FreezeTrait_TracyH());
        TraitDict.Add(Traits.FreezeProjectile_TracyH, new FreezeProjectileTrait_TracyH());
        TraitDict.Add(Traits.Slow_TracyH, new SlowTrait_TracyH());
        TraitDict.Add(Traits.SlowProjectile_TracyH, new SlowProjectileTrait_TracyH());
        TraitDict.Add(Traits.SlowZone_TracyH, new SlowZoneTrait_TracyH());
        TraitDict.Add(Traits.Homing_TracyH, new HomingTrait_TracyH());
        // WesleyP Traits
        TraitDict.Add(Traits.Knockback_WesleyP1, new KnockbackTrait_Wesley());
        TraitDict.Add(Traits.CharacterSwap_WesleyP1, new CharacterSwapTrait_Wesley());
        TraitDict.Add(Traits.HealingAlly_WesleyP1, new HealingAllyTrait_Wesley());
        // YuChen Traits
        TraitDict.Add(Traits.Criticaldamage, new CriticaldamageTrait());
        TraitDict.Add(Traits.SpeedIncrease_yu, new SpeedIncrease_yuTrait());
        

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
            case Actions.MishaTestAct1:return new TestAction1_Misha(who, e);
            case Actions.MishaTestAct2:return new TestAction2_Misha(who, e);
            // MazK=12,
            // AdamD=20,
            case Actions.DefendAction_AdamD:return new DefendAction(who,e);
            case Actions.RestAction_AdamD:return new RestAction(who,e);
            // AlejandroM=25,
            // ElioR=30,
            case Actions.ParryAction_ElioR: return new ParryAction(who,e);
            // JaidenB=35,
            case Actions.ExplodeAction_JaidenB:return new ExplodeAction_JaidenB(who, e);
            case Actions.RobAction_JaidenB: return new RobAction_JaidenB(who, e);
            case Actions.BlasterAction_JaidenB: return new BlasterAction_JaidenB(who, e);
            // JuliusP=40,
            case Actions.BarrierShield_JuliusP:return new BarrierShieldAction_JuliusP(who,e);
            case Actions.Lv2_BarrierShield_JuliusP:return new Lv2_BarrierShield_JuliusP(who,e);
            case Actions.Lv3_BarrierShield_JuliusP:return  new Lv3BarrierShield(who, e);
            case Actions.Cloak_JuliusP:return new InvisbilityAction(who, e);
            case Actions.Lv2_Cloak_JuliusP: return new Lv2Invis(who, e);
            case Actions.Lv3_Cloak_JuliusP: return new Lv3Invis(who, e);
            case Actions.TradeHp_JuliusP:return new TradeHp(who, e);
            case Actions.GiveItem_JuliusP:return new GiveItemv1(who,e);
            case Actions.GiveItemLv2_JuliusP:return new GiveItem_Lv2(who,e);
            case Actions.EvasiveJuke_JuliusP:return new EvasiveJuke(who, e);
            case Actions.BleakWatcher_JuliusP: return new BleakWatcher(who, e);
            case Actions.Scan_JuliusP: return new Scan(who, e);
            case Actions.BarrierShieldType2_JuliusP: return new Lv1BarrierShieldType2_JuliusP(who, e);
            case Actions.GiveItem_Lv3_JuliusP: return new GiveItem_Lv3(who, e);
            case Actions.DoubleShootV2_JuliusP: return new DoubleShootV2(who, e);
            case Actions.Lv4_BarrierShield_JuliusP: return new Lv4_BarrierShield_JuliusP(who,e);


            

         
          
            
        
            // MichaelT=50,
            case Actions.BleedAttack_MichaelT: return new BleedAttackAction_MichaelT(who, e);
            case Actions.DoMoreDamageAttack_MichaelT: return new DoMoreDamageAction_MichaelT(who,e);
            // QixiangD=55,
            case Actions.Sidestep_qixiangdong: return new Sidestep_qixiangdong(who,e);
            case Actions.Charging_qixiangdong: return new Charging_qixiangdong(who,e);
            case Actions.BulletSpawner_qixiangdong: return new BulletSpawner_qixiangdong(who,e);
            case Actions.SpinShoot_qixiangdong: return new SpinShoot_qixiangdong(who,e);
            case Actions.BurstShoot_qixiangdong: return new BurstShoot_qixiangdong(who,e);

            // RaphaelC=60,
            case Actions.CurveChase_RaphaelC:return new CurveChaseAction_RaphaelC(who,e);
            case Actions.Invisible_RaphaelC:return new Invisible_RaphaelC(who,e);
            case Actions.SpinShoot_RaphaelC:return new SpinShoot_RaphaelC(who,e);

            // SabahE=65,
            case Actions.Dash_SabahE: return new Dash_SabahE(who, e);
            case Actions.GroundSlam_SabahE: return new GroundSlam_SabahE(who, e);
            case Actions.SabahClassAction: return new SabahClassAction(who, e);
            case Actions.SabahClassAction2: return new SabahClassAction2(who, e);
            // SamsonW=70,
            case Actions.SelfKill: return new SelfKillAction(who, e);
            case Actions.Grapple: return new GrappleAction(who, e);
            // SarahS=75,
            case Actions.StalkSarahS: return new Stalk_SarahS(who,e);
            case Actions.RiseFromDeadSarahS: return new RiseFromDead_SarahS(who,e);
            case Actions.PossessionSarahS: return new Possession_SarahS(who, e);
            case Actions.PanicRunSarahS: return new PanicRun_SarahS(who, e);
            case Actions.HopSarahS: return new Hop_SarahS(who, e);
            case Actions.RepelSarahS: return new Repel_SarahS(who, e);
            case Actions.RhythmChaseSarahS: return new RhythmChase_SarahS(who, e);
            case Actions.RhythmPatrolSarahS: return new RhythmPatrol_SarahS(who,e);
            // TracyH=80,
            case Actions.Charge_TracyH: return new ChargeAction_TracyH(who, e);
            // WesleyP=90,
            case Actions.TradeHp_WesleyP: return new TraeHp(who, e);
            case Actions.GiveItem_WesleyP: return new GiveItemLv1(who, e);
            case Actions.GiveItemLv2_WesleyP: return new GiveItem_LvL2(who, e);
            // YuChen=95,
            case Actions.spinAction_Yu: return new SpinAction_Yuchen(who, e);
            case Actions.TeleportSwingAction_Yu: return new TeleportSwingAction_Yu(who, e);
            case Actions.SwingThenShootAction_Yu: return new Swingthenshoot_yu(who, e);
            case Actions.LungeandTeleportAction_yu: return new LungeandTeleportAction_yu(who, e);
            case Actions.DoubleShootAction_yu: return new DoubleShootAction_yu(who, e);
                
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
            case Authors.MishaF: return new Level_MishaF();
            case Authors.AdamD: return new Level_AdamD();
            case Authors.AlejandroM: return new Level_AlejandroM();
            case Authors.ElioR: return new Level_ElioR();
            case Authors.JaidenB: return new Level_JaidenB();
            case Authors.JuliusP: return new Level_JuliusP();
            case Authors.MichaelT: return new Level_MichaelT();
            case Authors.QixiangD: return new Level_QixiangD();
            case Authors.RaphaelC: return new Level_RaphaelC();
            case Authors.SabahE: return new Level_SabahE();
            case Authors.SamsonW: return new Level_SamsonW();
            case Authors.SarahS: return new Level_SarahS();
            case Authors.TracyH: return new Level_TracyH();
            case Authors.WesleyP: return new Level_WesleyP();
            case Authors.YuChen: return new Level_YuChen();
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
    Demo=100,
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
    TestTrait_Misha =1101,
    //AdamD         =20##,
    AdamD1          =2001,
    statusEffectOnProjectile=2002,
    StatusResist=2003, 
    UpgradeTrait=2004,
    //AlejandroM    =25##,
    AlejandroM1     =2501,
    ShieldPotion_AlejandroM = 2502,
    SpeedPotion_AlejandroM = 2503,
    //ElioR         =30##,
    ElioR1          =3001,
    Barrier         =3002, //this will negate one instance of taken damage taken.
    ParryTrait_ElioR = 3003,
    LifeSteal_ElioR=3004,
    Blight_ElioR = 3005,
    BlightGiver_ElioR = 3006,
    //JaidenB       =35##,
    JaidenB1        =3501,
    InvertControls  =3502,
    Freeze          =3503,
    MoneyDrop       =3504,
    Lava            =3505,
    //JuliusP       =40##,
    JuliusP1        =4001,
    Rage            =4002, 
    Dash            =4003,
    SelfDestruct_JuliusP = 4004,
    IgnoreDamage_JuliusP = 4005,
    StunNegation_JuliusP = 4006,
    NoTimerStunNegation_JuliusP = 4007,
    TemporaryStunImmunity_JuliusP = 4008,
    TemporaryDash_JuliusP = 4009,
    TemporaryDmgResist_JuliusP = 4010,
    GainInvis_JuliusP = 4011,
    MonadoArts_JuliusP = 4012,
    Slowed_JuliusP = 4013,
    SlowOnhit_JuliusP = 4014,
    LowHealthWarrior_JuliusP = 4015,
    AlwaysRage_JuliusP = 4016,
    ApplyHeavyKnockBack_JuliusP = 4017,
    DodgeInvis_JuliusP = 4018,
    RedLight = 4019,
    SpawnWall_JuliusP = 4020,
    AddTraitByScore_JuliusP = 4021,
    Lv2RedLight_JuliusP = 4022,
    TempInvis_JuliusP = 4023,
    ShieldTrait_JuliusP = 4024,
    Lv3RedLight_JuliusP = 4025,
    DetectDeath_JuliusP = 4026,
    Lv3Homing_JuliusP = 4027,
    SelfDestruct2_JuliusP = 4028,
    RemoveExit_JuliusP = 4029,





   
    
    
 
  

    //MazK          =45##,
    MazK1           =4501,
    //MichaelT      =50##,
    MichaelT1       =5001,
    Bleed_MichaelT = 5002, //Ticks Damage for 1 second 
    //QixiangD      =55##,
    QixiangD1       =5501,
    Sneaky_qixiangdong = 5502,
    Thrill_qixiangdong = 5503,
    BulletHell_qixiangdong = 5504,
    LockedExit_qixiangdong = 5505,
    //RaphaelC      =60##,
    Lighting_RaphaelC       =6001,
    Trait2_RaphaelC         =6002,
    BasicHeal_RaphaelC  =6003,
    KillSpeedBoost =6004,
    WhirlPool_RaphaelC=6005,
    BossDeath_RaphaelC =6006,
    //SabahE        =65##,
    SabahE1         =6501,
    SpeedUpSabahE   =6502, //Speedup for 10s when you get hit
    RallySabahE = 6503,
    //SamsonW       =70##,
    SamsonW1        =7001,
    TeleportRandomRoom=7002, //Use to teleport user to random room that isnt own room
    DamageReflect   =7003, //Reflects damage, thornmail effect
    HealZone        =7004, //Heals player when standing inside zone
    DelayedActionAfterStartingAction=7005, //Switches action after X secs after entering an action
    StackableDagger =7006, //Buffs dagger for each stack equipped
    GrappleHook     =7007, //Triggers grappling action on collision
    ComboBreak      =7008, //After getting stunned for X secs in a row, become immune to CC for Y secs
    CrowdControlNegation =7009, //CC negation trait blocks stuns and knockback for configured duration
    BarrierItem     =7010, //On use, grants a permanent barrier shield trait
    PermBarrier     =7011, //Permanent barrier shield that blocks damage while it has charge
    LifestealItem   =7012, //On use, grants temporary lifesteal
    TempLifesteal   =7013, //Heals for damage dealt while active
    StartWithItem = 7016, //Gives a ThingOption (item) to the Thing on spawn
    //SarahS        =75##,
    ProximityExplodeSarahS  =7501,
    SlowMoSarahS    =7502,
    CursedObjectSarahS    =7503,
    MimicEnemySarahS =7504,
    ProtectionCircleSarahS =7505,
    ProtectionSpellSarahS =7506,
    DamageFlashSarahS =7507,
    FleeSarahS    =7508,
    ContactDamageSarahS =7509,
    //TracyH        =80##,
    TracyH1 = 8001,
    Teleport_TracyH = 8002, //Teleport player between radius or nearby room(Zone)
    Freeze_TracyH = 8003, //Freezes target
    FreezeProjectile_TracyH = 8004, //Applies freeze on hit 
    Slow_TracyH = 8005, //Slows target
    SlowProjectile_TracyH = 8006, //Applies slow on hit 
    SlowZone_TracyH = 8007, //Applies slow on Zone
    Homing_TracyH = 8008, //Chase player(work in progress)
    //WesleyP       =90##,
    WesleyP1        =9001,
    Knockback_WesleyP1=9002,
    CharacterSwap_WesleyP1  =9003,
    HealingAlly_WesleyP1=9004,
    MoneysDrop_WesleyP1 = 9005, //Spawns money on death
    //YuChen        =95##,
    YuChen1         =9501,
    Criticaldamage=9502,
    SpeedIncrease_yu = 9503,
    
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
    MishaTestAct1   =1000,
    MishaTestAct2   =1001,
    //AdamD         =20##,
    AdamD1          =2001,
    RestAction_AdamD=2002,
    DefendAction_AdamD=2003,
    //AlejandroM    =25##,
    AlejandroM1     =2501,
    //ElioR         =30##,
    ElioR1          =3001,
    ParryAction_ElioR     =3002,
    //JaidenB       =35##,
    JaidenB1        =3501,
    ExplodeAction_JaidenB = 3502,
    RobAction_JaidenB = 3503,
    BlasterAction_JaidenB = 3504,
    //JuliusP       =40##,
    JuliusP1        =4001,
    BarrierShield_JuliusP = 4002,
    Lv2_BarrierShield_JuliusP = 4003,
    Lv3_BarrierShield_JuliusP = 4004,
    Cloak_JuliusP = 4005,
    Lv2_Cloak_JuliusP = 4006,
    Lv3_Cloak_JuliusP = 4007,
    TradeHp_JuliusP = 4008,
    GiveItem_JuliusP = 4009,
    GiveItemLv2_JuliusP = 4010,
    EvasiveJuke_JuliusP = 4011,
    BleakWatcher_JuliusP = 4012,
    slowingproj_JuliusP = 4013,
    Berserker_JuliusP = 4014,
    Scan_JuliusP = 4015,
    BarrierShieldType2_JuliusP = 4016,
    GiveItem_Lv3_JuliusP = 4017,
    DoubleShootV2_JuliusP = 4018,
    Lv4_BarrierShield_JuliusP = 4019,



   
 


  


    //MazK          =45##,
    MazK1           =4501,
    //MichaelT      =50##,
    MichaelT1       =5001,
    BleedAttack_MichaelT = 5003, //Applies Bleed Trait to Target on Hit
    DoMoreDamageAttack_MichaelT = 5004,
    //QixiangD      =55##,
    Sidestep_qixiangdong = 5501,
    Charging_qixiangdong = 5502,
    BulletSpawner_qixiangdong = 5503,
    ConeAttack_qixiangdong = 5504,
    SpinShoot_qixiangdong = 5505,
    BurstShoot_qixiangdong = 5506,
    //RaphaelC      =60##,
    RaphaelC1       =6001,
    CurveChase_RaphaelC = 6002,
    Invisible_RaphaelC = 6003,
    SpinShoot_RaphaelC = 6004,
    //SabahE        =65##,
    SabahE1         =6501,
    Dash_SabahE     =6502,
    GroundSlam_SabahE =6503,
    SabahClassAction =6504,
    SabahClassAction2 =6505,
    //SamsonW       =70##,
    SamsonW1        =7001,
    SelfKill        =7002, // Immediately kills thing on enter after 1 frame
    Grapple         =7003,
    //SarahS        =75##,
    StalkSarahS     =7501,
    RiseFromDeadSarahS =7502,
    PossessionSarahS =7503,
    PanicRunSarahS  =7504,
    HopSarahS     =7505,
    RepelSarahS     =7506,
    RhythmChaseSarahS =7507,
    RhythmPatrolSarahS =7508,
    //TracyH        =80##,
    TracyH1         =8001,
    Charge_TracyH   =8002,
    //WesleyP       =90##,
    WesleyP1        =9001,
    CharacterSwap_WesleyP =9002, 
    HealingAlly_WesleyP =9003,
    TradeHp_WesleyP = 9004,
    GiveItem_WesleyP = 9005,
    GiveItemLv2_WesleyP = 9006,
    //YuChen        =95##,
    YuChen1         =9501,
    spinAction_Yu = 9502,
    TeleportSwingAction_Yu = 9503,
    SwingThenShootAction_Yu = 9504,
    LungeandTeleportAction_yu = 9505,
    DoubleShootAction_yu = 9506
}
