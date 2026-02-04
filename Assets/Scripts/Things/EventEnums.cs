public enum EventTypes{
    //Core
    None            =0000,
    TraitInfo       =0001,
    Setup           =0002,
    OnSpawn         =0003,
    Update          =0004,
    FixedUpdate     =0005,
    Message         =0006, //Generic event, you can use this for events that don't have enums yet (just set StrInfo.Message)
    StartAction     =0101,
    SetPhase        =0102,
    UseHeld         =0103,
    UseHeldStart    =0104,
    UseHeldEnd      =0105,
    UseHeldComplete =0106,
    UseHeldAbort    =0107,
    
    //Common Combat
    Damage          =1001,
    Healing         =1002,
    Death           =1003,
    Knockback       =1004,
    GainTrait       =1100,
    LoseTrait       =1101,
    
    //Common Actions
    OnTouch         =2000,  //Touch is Hitbox to Hitbox
    OnTouchEnd      =2001,
    OnInside        =2002,
    OnTouchWall     =2003,
    OnDestroy       =2010,
    Interact        =2040,
    PlayerTouched   =2041,
    PlayerLeft      =2042,
    OnPickup        =2043,  //Called on the item being picked up
    OnDrop          =2044,
    DidPickup       =2045,  //Called on the thing picking the item up
    DidDrop         =2046,
    OnUse           =2047,  //Called when you click while holding
    OnUseStart      =2048,  //Only called the first time you click
    OnUseEnd        =2049,
    OnUseAbort      =2050,
    OnHoldStart     =2060,
    AddScore        =2100,
    ChangeStack     =2101,
    
    //Questions
    ShownHP         =9001,
    ShownName       =9002,
    GetDamage       =9007,
    GetProjectile   =9008,  //To be replaced
    GetScore        =9009,
    GetCurrentAction=9100,
    GetDefaultAction=9101,
    GetDefaultAttack=9102,
    GetActSpeed     =9103,  //Amount is mult, Max is duration
}

public enum NumInfo
{
    None=0,
    Default=1,
    Max=2,
    Min=3,
    Speed=4,
    Distance=5,
    Size=6,
    Priority=7,
    Time=8,
    Phase=9,
    MiscA=10,
    MiscB=11,
    MiscC=12,
    MiscD=13,
    MiscE=14,
}

public enum StrInfo
{
    None=0,
    Default=1,
    Debug=2,
    DType=3,
    Message=4,
    D=5,
    E=6,
    F=7
}

public enum ActionInfo
{
    None=0,
    Default=1,
    Action=2,
    DefaultAction=3,
    DefaultChaseAction=4,
    D=5,
    E=6,
    F=7
}

public enum BoolInfo
{
    None=0,
    Default=1,
    Abort=2,
    Success=3,
    C=4,
    D=5,
    E=6,
    F=7
}

public enum ThingEInfo
{
    None=0,
    Default=1,
    Source=2,
    Target=3,
    C=4,
    D=5,
    E=6,
    F=7
}

public enum OptionInfo
{
    None=0,
    Default=1,
    A=2,
    B=3,
    C=4,
    D=5,
    E=6,
    F=7
}

public enum VectorInfo
{
    None=0,
    Amount=1,
    Dir=2,
    B=3,
    C=4,
    D=5,
    E=6,
    F=7
}


public enum HitboxInfo
{
    None=0,
    Theirs=1,
    Mine=2,
    B=3,
    C=4,
    D=5,
    E=6,
    F=7
}


public enum GameTags
{
    Custom          =0000,
    Player          =0001,
    Something       =0002,
    Debug           =0003,
    NPC             =0102,
    Pickup          =0200,
    Weapon          =0201,
    Consumable      =0202,
    ScoreThing      =0203,
    Projectile      =0300,
    Centerpiece     =0400,
    Exit            =0401,
}

public enum RoomTags
{
    None           =0000,
    Generic        =0001,
    PlayerStart    =0002,
    Exit           =0003,
}