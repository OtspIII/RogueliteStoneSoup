public enum EventTypes{
    //Core
    None=0000,
    TraitInfo=0001,
    Setup=0002,
    Update=003,
    Start=004,
    StartAction=0101,
    SetPhase=0102,
    
    //Common Combat
    Damage=1001,
    Healing=1002,
    Death=1003,
    Knockback=1004,
    
    //Questions
    ShownHP=9001,
    GetCurrentAction=9002,
    GetDefaultAction=9003,
    GetDefaultAttack=9004,
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

public enum VectorInfo
{
    None=0,
    Amount=1,
    Dir=2,
}


public enum Tags
{
    None=0,
    Player=1,
    NPC=2,
    Weapon=3,
    Projectile=4,
}