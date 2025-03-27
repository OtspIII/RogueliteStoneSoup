using System.Collections.Generic;
using UnityEngine;

public class TraitInfo
{
    public Traits Type;
    public EventInfo Info;
    public ThingController Who { get { return Get(ActorInfo.Source); }}

    public TraitInfo(Traits t, ThingController who, EventInfo i)
    {
        Type = t;
        Info = i != null ? i : new EventInfo(EventTypes.TraitInfo);
        Info.Set(ActorInfo.Source, who);
    }

    public void Init() { TraitManager.Get(Type).Init(this); }
    public void ReUp(EventInfo i) { TraitManager.Get(Type).ReUp(this,i); }
    public void PreEvent(EventInfo e) { TraitManager.Get(Type).PreEvent(this,e); }
    public void TakeEvent(EventInfo e) { TraitManager.Get(Type).TakeEvent(this,e); }

    public float Get(NumInfo n){ return Info.Get(n);}
    public string Get(StrInfo n){ return Info.Get(n);}
    public T Get<T>(EnumInfo n){ return Info.Get<T>(n);}
    public bool Get(BoolInfo n){ return Info.Get(n);}
    public ThingController Get(ActorInfo n){ return Info.Get(n);}
    public Vector2 Get(VectorInfo n){ return Info.Get(n);}
    
    public TraitInfo Set(NumInfo n, float v){ Info.Set(n,v);return this;}
    public TraitInfo Set(StrInfo n, string v){ Info.Set(n,v);return this;}
    public TraitInfo Set(EnumInfo n, int v){ Info.Set(n,v);return this;}
    public TraitInfo Set(BoolInfo n, bool v){ Info.Set(n,v);return this;}
    public TraitInfo Set(ActorInfo n, ThingController v){ Info.Set(n,v);return this;}
    public TraitInfo Set(VectorInfo n, Vector2 v){ Info.Set(n,v);return this;}
    
    public float Change(NumInfo n, float v){ return Info.Change(n,v);}
}

public static class TraitManager
{
    public static bool Setup = false;
    public static Dictionary<Traits, Trait> TraitDict = new Dictionary<Traits, Trait>();

    public static void Init()
    {
        if (Setup) return;
        Setup = true;
        TraitDict.Add(Traits.Health,new HealthTrait());
    }
    
    public static Trait Get(Traits t)
    {
        if (TraitDict.TryGetValue(t, out Trait r)) return r;
        Debug.Log("ERROR MISSING TRAIT: " + t+"\nMust add to TraitManager");
        return null;
    }
}

public class Trait
{
    public Traits Type;
    public List<EventTypes> PreListen = new List<EventTypes>();
    public List<EventTypes> TakeListen = new List<EventTypes>();

    public void Init(TraitInfo i)
    {
        foreach (EventTypes e in PreListen)
            i.Who.AddListen(e,Type,true);
        foreach (EventTypes e in TakeListen)
            i.Who.AddListen(e,Type,false);
        Setup(i);
    }
    
    protected virtual void Setup(TraitInfo i)
    {
        //Called when a trait first gets added to an actor
    }

    public virtual void ReUp(TraitInfo old,EventInfo n)
    {
        //Called when you gain a trait when you already had it
    }
    
    public virtual void PreEvent(TraitInfo i, EventInfo e)
    {
        //Called when an event hits an actor, runs before TakeEvent
        //Use this for things like elemental vulnerabilities--things that
        // mod the result, but don't do any effects just yet
    }
    
    public virtual void TakeEvent(TraitInfo i, EventInfo e)
    {
        
    }
}


public enum Traits
{
    None=0,
    Health=1,
}