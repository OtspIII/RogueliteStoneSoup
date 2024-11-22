using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Static class that has the JSON manager code on it
public static class JSONReader
{
    public static LevelJSON ParseJSON(string txt)
    {
        return JsonUtility.FromJson<LevelJSON>(txt);
    }
}

//Your JSON files should only have these variables in them
[System.Serializable]
public class LevelJSON
{
    public string Author;
    public CharacterJSON Player;
    public List<CharacterJSON> NPCs;
}

[System.Serializable]
public class CharacterJSON
{
    public string Name;
    public float Speed;
    public float HP;
    // public string DefaultAction;
}

[System.Serializable]
public class RawInfoJSON
{
    public float? Amt;
    public float? Priority;
    public string Text;
}