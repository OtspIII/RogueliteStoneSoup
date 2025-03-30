using System.Collections.Generic;
using UnityEngine;

public class TraitInfo : EventInfo
{
    public Traits Trait;
    public ThingController Who { get { return Get(ActorInfo.Source); }}

    public TraitInfo(Traits t, ThingController who, EventInfo i)
    {
        Clone(i);
        Trait = t;
        Type = EventTypes.TraitInfo;
        Set(ActorInfo.Source, who);
    }

    public void Init() { TraitManager.Get(Trait).Init(this); }
    public void ReUp(EventInfo i) { TraitManager.Get(Trait).ReUp(this,i); }
    public void PreEvent(EventInfo e) { TraitManager.Get(Trait).PreEvent(this,e); }
    public void TakeEvent(EventInfo e) { TraitManager.Get(Trait).TakeEvent(this,e); }
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
        TraitDict.Add(Traits.Actor,new ActorTrait());
        TraitDict.Add(Traits.Player,new PlayerTrait());
        TraitDict.Add(Traits.Fighter,new FighterTrait());
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
    Actor=2,
    Player=3,
    Fighter=4,
}