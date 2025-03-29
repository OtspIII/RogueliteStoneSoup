using System.Collections.Generic;
using UnityEngine;

public static class ThingBuilder
{
    public static Dictionary<string, ThingSeed> Things = new Dictionary<string, ThingSeed>();
    public static Dictionary<string, List<string>> Tags = new Dictionary<string, List<string>>();
    public static ThingSeed Player;
    
    public static void Init()
    {
        Player = Add("Player", "Humanoid", "Smile", "Player")
            .Trait(Traits.Player)
            .Trait(Traits.Actor,God.E().Set(EnumInfo.DefaultAction,(int)Actions.Idle))
            .Trait(Traits.Health,God.E().Set(NumInfo.Max,3));
        
        Add("Sword Monster", "Humanoid", "", "NPC")
            .Trait(Traits.Actor)
            .Trait(Traits.Health,God.E().Set(NumInfo.Max,2));
        
        Add("Lunger", "Lunger", "", "NPC")
            .Trait(Traits.Actor)
            .Trait(Traits.Health,God.E().Set(NumInfo.Max,1));
    }

    public static ThingSeed Add(string name, string body, string art, params string[] tags)
    {
        ThingSeed who = new ThingSeed(name,body,art);
        Things.Add(who.Name,who);
        foreach (string t in tags)
        {
            if(!Tags.ContainsKey(t)) Tags.Add(t,new List<string>());
            Tags[t].Add(who.Name);
        }
        return who;
    }

    public static List<ThingSeed> GetTag(string tag)
    {
        if (!Tags.ContainsKey(tag))
        {
            Debug.Log("UNFOUND TAG: " + tag);
            return new List<ThingSeed>();
        }
        List<ThingSeed> r = new List<ThingSeed>();
        foreach(string t in Tags[tag])
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