using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameLibrary : MonoBehaviour
{
    public ThingController ActorPrefab;
    // public BodyController HumanoidBodyPrefab;
    // public BodyController LungerBodyPrefab;
    public WeaponController SwordPrefab;
    public WeaponController TramplePrefab;
    public ProjectileController ProjectilePrefab;
    private List<RoomOption> RoomOptions = new List<RoomOption>();
    private List<ThingOption> ThingOptions = new List<ThingOption>();
    // public ExitController ExitPrefab;

    public Dictionary<string, Sprite> CharacterArt = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> WeaponArt = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> ProjectileArt = new Dictionary<string, Sprite>();
    public Dictionary<string, WeaponStats> Weapons = new Dictionary<string, WeaponStats>();
    public Dictionary<string, ProjStats> Projectiles = new Dictionary<string, ProjStats>();

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
        foreach (Sprite s in Resources.LoadAll<Sprite>("ProjectileArt/"))
        {
            ProjectileArt.Add(s.name,s);
            // Debug.Log(s);
        }

        foreach (ThingOption o in Resources.LoadAll<ThingOption>("Things/"))
            ThingOptions.Add(o);
        foreach (RoomOption o in Resources.LoadAll<RoomOption>("Rooms/"))
            RoomOptions.Add(o);
    }

    public void Setup()
    {
        foreach (WeaponStats w in God.JSON.Weapons)
        {
            Weapons.Add(w.Name,w);
            // Debug.Log("ADDED: " + w.Name);
        }
        foreach (ProjStats w in God.JSON.Projectiles)
        {
            Projectiles.Add(w.Name,w);
            // Debug.Log("ADDED: " + w.Name);
        }
    }

    // public BodyController GetBody(string which)
    // {
    //     switch (which)
    //     {
    //         case "Humanoid": return HumanoidBodyPrefab;
    //         case "Lunger": return LungerBodyPrefab;
    //     }
    //     Debug.Log("INVALID BODY NAME: " + which);
    //     return HumanoidBodyPrefab;
    // }

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
        Debug.Log("BACKUP WEAPON ART: " + which);
        if (backup != null) return backup;
        Debug.Log("INVALID WEAPON NAME: " + which);
        return null;
    }
    
    public ProjStats GetProjectile(string which)
    {
        Projectiles.TryGetValue(which, out ProjStats r);
        if (r != null) return r;
        Debug.Log("INVALID PROJECTILE NAME: " + which);
        return null;
    }
    
    public ProjectileController GetProjectilePrefab()
    {
        return ProjectilePrefab;
    }
    
    public Sprite GetProjectileArt(string which, Sprite backup=null)
    {
        if (string.IsNullOrEmpty(which)) return backup;
        ProjectileArt.TryGetValue(which, out Sprite r);
        if (r != null) return r;
        if (backup != null) return backup;
        Debug.Log("INVALID Projectile Art NAME: " + which);
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
    
    public RoomOption GetRoom(GeoTile g,LevelBuilder b)
    {
        RoomTags t = RoomTags.Generic;
        if (g.Path == GeoTile.GeoTileTypes.PlayerStart) t = RoomTags.PlayerStart;
        else if (g.Path == GeoTile.GeoTileTypes.Exit) t = RoomTags.Exit;
        List<RoomOption> opts = new List<RoomOption>();
        foreach (RoomOption rs in RoomOptions)
        {
            if(rs.Tags.Contains(t)) opts.Add(rs);
        }

        if (opts.Count == 0)
        {
            Debug.Log("NO VALID ROOMS: " + t + " / " + g + " / + b");
            return null;
        }
        return opts.Random();
    }

    public ThingOption GetThing(SpawnRequest sr,LevelBuilder b=null)
    {
        List<ThingOption> opts = new List<ThingOption>();
        foreach (ThingOption o in ThingOptions)
        {
            if(sr.Judge(o) > 0) opts.Add(o);
        }

        if (opts.Count == 0)
        {
            Debug.Log("NO VALID ROOMS: " + sr + " / " + b);
            return null;
        }
        return opts.Random();
    }
}
