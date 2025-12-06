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
    // public CharacterStats Player;
    // public List<CharacterStats> NPCs;
    public List<WeaponStats> Weapons;
    public List<ProjStats> Projectiles;
}

// [System.Serializable]
// public class CharacterStats
// {
//     public string Name;
//     // public string Body;
//     public string Art;
//     public TraitStats[] Traits;
//     public float Speed = 5;
//     public float HP;
//     //public string Weapon;
//     public string DefaultAction;
// }

// [System.Serializable]
// public class TraitStats
// {
//     public string Trait;
//     public float Amount;
// }

[System.Serializable]
public class WeaponStats
{
    public string Name;
    public float Damage;
    public string Body;
    public string DefaultAttack;
    public string Art;
    public string Projectile;
}

[System.Serializable]
public class ProjStats
{
    public string Name;
    public float Damage;
    public string Body;
    public string Art;
    public float Speed;
}

// [System.Serializable]
// public class RawInfoJSON
// {
//     public float? Amt;
//     public float? Priority;
//     public string Text;
// }