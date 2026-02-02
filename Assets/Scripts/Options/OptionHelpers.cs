using System;
using System.Collections.Generic;
using UnityEngine;

///The parent class of all Options--all Options share a name and an author.
public class GameOption : ScriptableObject
{
    public string Name;
    public Authors Author;
}

//This and the below classes just exist so the player can input data in the Unity Inspector when customizing Options
[System.Serializable]
public class TraitBuilder
{
    public Traits Trait;
    public List<InfoNumber> Numbers;
    public List<InfoOption> Prefabs;
    public List<InfoAction> Acts;
    public SpawnRequest SpawnReq;
}

[System.Serializable]
public class InfoNumber
{
    public NumInfo Type=NumInfo.Amount;
    public float Value;
}

[System.Serializable]
public class InfoOption
{
    public OptionInfo Type=OptionInfo.Default;
    public ThingOption Value;
}

[System.Serializable]
public class InfoAction
{
    public ActionInfo Type=ActionInfo.None;
    public Actions Act;
}