public enum EventTypes{
    //Core
    None            =0000,
    TraitInfo       =0001,
    Setup           =0002,
    Update          =003,
    Start           =004,
    StartAction     =0101,
    SetPhase        =0102,
    
    //Common Combat
    Damage          =1001,
    Healing         =1002,
    Death           =1003,
    Knockback       =1004,
    // GainTrait       =1100,
    // LoseTrait       =1101,
    
    //Common Actions
    OnTouch         =2000,  //Touch is Hitbox to Hitbox
    OnTouchInside   =2003,
    OnTouchWall     =2004,
    OnHit           =2010,  //Hit is hurtbox to hitbox
    OnHitInside     =2003,
    OnHitWall       =2004,
    OnClash         =2030,  //Clash is hurtbox to hurtbox
    TryPickup       =2040,  //Pickup happens when the player bumps into something
    
    //Questions
    ShownHP         =9001,
    GetCurrentAction=9002,
    GetDefaultAction=9003,
    GetDefaultAttack=9004,
    IsPlayer        =9005,
}

public enum NumInfo
{
    None=0,
    Amount=1,
    Max=2,
    Min=3,
    Speed=4,
}

public enum StrInfo
{
    None=0,
    Text=1,
    Debug=2,
}

public enum EnumInfo
{
    None=0,
    Default=1,
    Action=1001,
    DefaultAction=1002,
    DamageType=2001,
}

public enum BoolInfo
{
    None=0,
    Default=1,
}

public enum ActorInfo
{
    None=0,
    Target=1,
    Source=2,
}

public enum OptionInfo
{
    None=0,
    Default=1,
}

public enum VectorInfo
{
    None=0,
    Amount=1,
    Dir=2,
}


public enum GameTags
{
    None=0,
    Player=1,
    NPC=2,
    Weapon=3,
    Projectile=4,
    Exit=5,
    Centerpiece=6,
}

public enum RoomTags
{
    None=0,
    Generic=1,
    PlayerStart=2,
    Exit=3,
}