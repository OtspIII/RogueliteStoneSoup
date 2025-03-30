using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ThingBuilder
{
    public static Dictionary<string, ThingSeed> Things = new Dictionary<string, ThingSeed>();
    public static Dictionary<Tags, List<string>> Tags = new Dictionary<Tags, List<string>>();
    //public static ThingSeed Player;
    
    public static void Init()
    {
        AddChar("Player", "Humanoid", "Smile", 3,global::Tags.Player).Trait(Traits.Player);
        
        AddChar("Sword Monster", "Humanoid", "", 2);
        
        AddChar("Lunger", "Lunger", "", 1);
    }

    public static ThingSeed Add(string name, string body, string art, params Tags[] tags)
    {
        ThingSeed who = new ThingSeed(name,body,art);
        Things.Add(who.Name,who);
        foreach (Tags t in tags)
        {
            if(!Tags.ContainsKey(t)) Tags.Add(t,new List<string>());
            Tags[t].Add(who.Name);
        }
        return who;
    }

    public static ThingSeed AddChar(string name, string body, string art, int hp, params Tags[] tags)
    {
        List<Tags> t = new List<Tags>();
        foreach(Tags tag in tags) t.Add(tag);
        if(t.Count == 0) t.Add(global::Tags.NPC);
        return Add(name, body, art, t.ToArray())
            .Trait(Traits.Actor)
            .Trait(Traits.Health,God.E().Set(hp));
    }

    public static List<ThingSeed> GetTag(Tags tag)
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