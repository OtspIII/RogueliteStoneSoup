public enum EventTypes{
    //Core
    None=0000,
    TraitInfo=0001,
    Setup=0002,
    Update=003,
    StartAction=0101,
    SetPhase=0102,
    
    //Common Combat
    Damage=1001,
    Healing=1002,
    Death=1003,
    
    //Questions
    ShownHP=9001,
    GetCurrentAction=9002,
    GetDefaultAction=9003,
}

public enum NumInfo
{
    None=0,
    Amount=1,
    Max=2,
    Min=3,
}

public enum StrInfo
{
    None=0,
    Debug=1,
}

public enum EnumInfo
{
    None=0,
    Action=1001,
    DefaultAction=1002,
    DamageType=2001,
}

public enum BoolInfo
{
    None=0
}

public enum ActorInfo
{
    None=0,
    Source=1,
    Target=2,
}

public enum VectorInfo
{
    None=0,
    Dir=1,
}