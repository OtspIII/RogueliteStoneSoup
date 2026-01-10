using System.Collections.Generic;
using UnityEngine;

public class TraitInfo : EventInfo
{
    public Traits Trait;
    public ThingInfo Who { get { return Get(ActorInfo.Source); }}

    public TraitInfo(Traits t, ThingInfo who, EventInfo i)
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

    public override string ToString()
    {
        return "[TRAIT:" + Trait + "]("+BuildString()+")";
    }
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
        TraitDict.Add(Traits.Tool,new ToolTrait());
        TraitDict.Add(Traits.Pickupable,new PickupableTrait());
        TraitDict.Add(Traits.HealPack,new HealPackTrait());
        TraitDict.Add(Traits.Projectile,new ProjectileTrait());
        TraitDict.Add(Traits.Exit,new ExitTrait());
        TraitDict.Add(Traits.DamageZone,new DamageZoneTrait());
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
    public Dictionary<EventTypes,float> PreListen = new Dictionary<EventTypes,float>();
    public Dictionary<EventTypes,float> TakeListen = new Dictionary<EventTypes,float>();

    public void Init(TraitInfo i)
    {
        // Debug.Log("INIT TRAIT: " + Type + " / " + i.Who.Name);
        foreach (EventTypes e in PreListen.Keys)
            i.Who.AddListen(e,Type,true);
        foreach (EventTypes e in TakeListen.Keys)
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
    
    public EventInfo Ask(TraitInfo i,EventTypes e)
    {
        EventInfo r = God.E(e);
        TakeEvent(i,r);
        return r;
    }

    public void AddListen(EventTypes e, float prio = 3)
    {
        TakeListen.Add(e,prio);
    }
    public void AddPreListen(EventTypes e, float prio = 3)
    {
        PreListen.Add(e,prio);
    }
}


public enum Traits
{
    //Basic Traits:  0###
    None            =0000,
    Actor           =0001,
    Health          =0002,
    Player          =0100,
    Exit            =0200,
    DamageZone      =0201,
    Tool        =0300,
    Pickupable     =0301,
    HealPack        =0310,
    Projectile      =0400,
    //Misha Traits:  11##
}