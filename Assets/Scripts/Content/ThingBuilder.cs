using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ThingBuilder
{
    public static Dictionary<string, ThingSeed> Things = new Dictionary<string, ThingSeed>();
    public static Dictionary<Tags, List<string>> TagDict = new Dictionary<Tags, List<string>>();
    //public static ThingSeed Player;
    
    public static void Init()
    {
        AddChar("Player", "Humanoid", "Smile", 3,Tags.Player).Trait(Traits.Player);
        
        AddChar("Sword Monster", "Humanoid", "", 2);
        AddChar("Lunger", "Lunger", "", 1);

        AddWeapon("Sword", "Sword", "Sword", Actions.Swing, 1);
        AddWeapon("Axe", "Sword", "Axe", Actions.Swing, 1);
        AddWeapon("Trample", "Trample", "", Actions.Lunge, 1);
        AddWeapon("Bow", "Sword", "Bow", Actions.Shoot, 1);

        AddProjectile("Arrow", "Arrow", "Arrow", 1, 10);
    }

    public static ThingSeed Add(string name, string body, string art, params Tags[] tags)
    {
        ThingSeed who = new ThingSeed(name,body,art);
        Things.Add(who.Name,who);
        foreach (Tags t in tags)
        {
            if(!TagDict.ContainsKey(t)) TagDict.Add(t,new List<string>());
            TagDict[t].Add(who.Name);
        }
        return who;
    }

    public static ThingSeed AddChar(string name, string body, string art, int hp, params Tags[] tags)
    {
        List<Tags> t = new List<Tags>();
        foreach(Tags tag in tags) t.Add(tag);
        if(t.Count == 0) t.Add(Tags.NPC);
        return Add(name, body, art, t.ToArray())
            .Trait(Traits.Actor)
            .Trait(Traits.Health,God.E().Set(hp));
    }
    
    public static ThingSeed AddWeapon(string name, string body, string art, Actions atk=Actions.Swing, int dmg=1, params Tags[] tags)
    {
        List<Tags> t = new List<Tags>();
        foreach(Tags tag in tags) t.Add(tag);
        if(t.Count == 0) t.Add(Tags.Weapon);
        return Add(name, body, art, t.ToArray())
            .Trait(Traits.Weapon,God.E().Set(EnumInfo.DefaultAction,(int)atk).Set(dmg));
    }
    
    public static ThingSeed AddProjectile(string name, string body, string art, int dmg=1,float speed=10, params Tags[] tags)
    {
        List<Tags> t = new List<Tags>();
        foreach(Tags tag in tags) t.Add(tag);
        if(t.Count == 0) t.Add(Tags.Projectile);
        return Add(name, body, art, t.ToArray())
            .Trait(Traits.Projectile,God.E().Set(dmg).Set(NumInfo.Speed,speed));
    }

    public static List<ThingSeed> GetTag(Tags tag)
    {
        if (!TagDict.ContainsKey(tag))
        {
            Debug.Log("UNFOUND TAG: " + tag);
            return new List<ThingSeed>();
        }
        List<ThingSeed> r = new List<ThingSeed>();
        foreach(string t in TagDict[tag])
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
}