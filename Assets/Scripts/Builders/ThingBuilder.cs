using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ThingBuilder
{
    public static Dictionary<string, ThingSeed> Things = new Dictionary<string, ThingSeed>();
    public static Dictionary<GameTags, List<string>> TagDict = new Dictionary<GameTags, List<string>>();
    //public static ThingSeed Player;
    
    public static void Init()
    {
        // AddChar("Player", "Humanoid", "Smile", 3,"Sword",GameTags.Player).Trait(Traits.Player);
        //
        // AddChar("Sword Monster", "Humanoid", "", 2, "Axe");
        // AddChar("Lunger", "Lunger", "", 1, "Trample");

        AddWeapon("Sword", "Sword", "Sword", Actions.Swing, 1);
        AddWeapon("Axe", "Sword", "Axe", Actions.Swing, 1);
        AddWeapon("Trample", "Trample", "", Actions.Lunge, 1);
        AddWeapon("Bow", "Sword", "Bow", Actions.Shoot, 1,"Arrow");

        AddProjectile("Arrow", "Arrow", "Arrow", 1, 10);
    }

    public static ThingSeed Add(string name, string body, string art, params GameTags[] tags)
    {
        ThingSeed who = new ThingSeed(name,body,art);
        Things.Add(who.Name,who);
        foreach (GameTags t in tags)
        {
            if(!TagDict.ContainsKey(t)) TagDict.Add(t,new List<string>());
            TagDict[t].Add(who.Name);
        }
        return who;
    }

    public static ThingSeed AddChar(string name, string body, string art, int hp, string weapon,params GameTags[] tags)
    {
        List<GameTags> t = new List<GameTags>();
        foreach(GameTags tag in tags) t.Add(tag);
        if(t.Count == 0) t.Add(GameTags.NPC);
        return Add(name, body, art, t.ToArray())
            .Trait(Traits.Actor)
            .Trait(Traits.Health,God.E().Set(hp))
            .SetWeapon(weapon);
    }
    
    public static ThingSeed AddWeapon(string name, string body, string art, Actions atk=Actions.Swing, int dmg=1,
        string projectile="",params GameTags[] tags)
    {
        List<GameTags> t = new List<GameTags>();
        foreach(GameTags tag in tags) t.Add(tag);
        if(t.Count == 0) t.Add(GameTags.Weapon);
        return Add(name, body, art, t.ToArray())
            .Trait(Traits.Tool,God.E().Set(EnumInfo.DefaultAction,(int)atk).Set(dmg).Set(projectile));
    }
    
    public static ThingSeed AddProjectile(string name, string body, string art, int dmg=1,float speed=10, params GameTags[] tags)
    {
        List<GameTags> t = new List<GameTags>();
        foreach(GameTags tag in tags) t.Add(tag);
        if(t.Count == 0) t.Add(GameTags.Projectile);
        return Add(name, body, art, t.ToArray())
            .Trait(Traits.Projectile,God.E().Set(dmg).Set(NumInfo.Speed,speed));
    }

    public static List<ThingSeed> GetTag(GameTags gameTag)
    {
        if (!TagDict.ContainsKey(gameTag))
        {
            Debug.Log("UNFOUND TAG: " + gameTag);
            return new List<ThingSeed>();
        }
        List<ThingSeed> r = new List<ThingSeed>();
        foreach(string t in TagDict[gameTag])
            r.Add(Things[t]);
        return r;
    }

}

public class ThingSeed
{
    public string Name;
    public string Body;
    public string Art;
    public Dictionary<Traits,EventInfo> Traits = new Dictionary<Traits, EventInfo>();
    public float Speed = 5;//To be moved to a trait
    public string Weapon = "Sword";//To be moved to a trait
    
    public ThingSeed(string name, string body, string art="")
    {
        Name = name;
        Body = body;
        Art = art;
    }

    public ThingSeed Trait(Traits t, EventInfo i=null)
    {
        Traits.Add(t,i);
        return this;
    }
    
    public ThingSeed SetWeapon(string w)
    {
        Weapon = w;
        return this;
    }


}