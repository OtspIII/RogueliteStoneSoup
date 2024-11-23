using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLibrary : MonoBehaviour
{
    public ActorController ActorPrefab;
    public BodyController HumanoidBodyPrefab;
    public BodyController LungerBodyPrefab;
    public WeaponController SwordPrefab;
    public WeaponController TramplePrefab;
    // public Sprite SmileArt;
    // public Sprite VampArt;
    // public Sprite SpitterArt;
    // public Sprite SwordWArt;
    // public Sprite AxeWArt;

    public Dictionary<string, Sprite> CharacterArt = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> WeaponArt = new Dictionary<string, Sprite>();
    public Dictionary<string, WeaponStats> Weapons = new Dictionary<string, WeaponStats>();

    private void Awake()
    {
        God.Library = this;
        // Sprite[] chars = Resources.LoadAll<Sprite>("Characters/");
        // Debug.Log(chars.Length);
        foreach (Sprite s in Resources.LoadAll<Sprite>("Characters/"))
        {
            CharacterArt.Add(s.name,s);
            // Debug.Log(s);
        }
        foreach (Sprite s in Resources.LoadAll<Sprite>("WeaponArt/"))
        {
            WeaponArt.Add(s.name,s);
            // Debug.Log(s);
        }
    }

    public void Setup()
    {
        foreach (WeaponStats w in God.JSON.Weapons)
        {
            Weapons.Add(w.Name,w);
            // Debug.Log("ADDED: " + w.Name);
        }
    }

    public BodyController GetBody(string which)
    {
        switch (which)
        {
            case "Humanoid": return HumanoidBodyPrefab;
            case "Lunger": return LungerBodyPrefab;
        }
        Debug.Log("INVALID BODY NAME: " + which);
        return HumanoidBodyPrefab;
    }

    public WeaponStats GetWeapon(string which)
    {
        Weapons.TryGetValue(which, out WeaponStats r);
        if (r == null)
        {
            Debug.Log("COULD NOT FIND WEAPON: " + which);
            return null;
        }
        return r;
    }
    
    public WeaponController GetWeaponPrefab(string which)
    {
        switch (which)
        {
            case "Sword": return SwordPrefab;
            case "Trample": return TramplePrefab;
        }
        Debug.Log("INVALID WEAPON NAME: " + which);
        return SwordPrefab;
    }
    
    public Sprite GetWeaponArt(string which, Sprite backup=null)
    {
        if (string.IsNullOrEmpty(which)) return backup;
        WeaponArt.TryGetValue(which, out Sprite r);
        if (r != null) return r;
        // switch (which)
        // {
        //     case "Sword": return SwordWArt;
        //     case "Axe": return AxeWArt;
        //     case "Trample": return null;
        // }
        if (backup != null) return backup;
        Debug.Log("INVALID WEAPON NAME: " + which);
        return null;
    }
    
    public Sprite GetArt(string which,Sprite backup=null)
    {
        if (string.IsNullOrEmpty(which)) return backup;
        CharacterArt.TryGetValue(which, out Sprite r);
        if (r != null) return r;
        // switch (which)
        // {
        //     case "Smile": return SmileArt;
        //     case "Vamp": return VampArt;
        //     case "Spitter": return SpitterArt;
        // }
        if (backup != null) return backup;
        Debug.Log("INVALID ART NAME: " + which);
        return null;
    }
}
