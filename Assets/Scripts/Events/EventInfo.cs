using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class EventInfo
{
    public EventTypes Type;
    public bool Abort { get { return GetBool(BoolInfo.Abort); } set { SetBool(BoolInfo.Abort, value);} }
    public Dictionary<NumInfo, float> Numbers = new Dictionary<NumInfo, float>();
    public Dictionary<StrInfo, string> Texts = new Dictionary<StrInfo, string>();
    public Dictionary<ActionInfo, Actions> Acts = new Dictionary<ActionInfo, Actions>();
    // public Dictionary<EnumInfo, int> Enums = new Dictionary<EnumInfo, int>();
    public List<BoolInfo> Bools = new List<BoolInfo>();
    public Dictionary<ActorInfo, ThingInfo> Actors = new Dictionary<ActorInfo, ThingInfo>();
    public Dictionary<OptionInfo, ThingOption> Options = new Dictionary<OptionInfo, ThingOption>();
    public Dictionary<VectorInfo, Vector2> Vectors = new Dictionary<VectorInfo, Vector2>();
    public ActionScript ActScript;
    // public ThingSeed Seed;
    public Dictionary<HitboxInfo, HitboxController> Hitboxes = new Dictionary<HitboxInfo, HitboxController>();
    public GameCollision Collision;
    public SpawnRequest SpawnReq;

    public EventInfo(){ }
    
    public EventInfo(EventTypes t)
    {
        Type = t;
    }

    public EventInfo(float n)
    {
        Numbers.Add(NumInfo.Amount,n);
    }
    
    public EventInfo(EventInfo i){ Clone(i); }

    public virtual void Clone(EventInfo i)
    {
        if (i == null) return;
        Type = i.Type;
        foreach(NumInfo n in i.Numbers.Keys) Numbers.Add(n,i.Numbers[n]);
        foreach(StrInfo n in i.Texts.Keys) Texts.Add(n,i.Texts[n]);
        foreach(ActionInfo n in i.Acts.Keys) Acts.Add(n,i.Acts[n]);
        foreach(BoolInfo n in i.Bools) Bools.Add(n);
        foreach(ActorInfo n in i.Actors.Keys) Actors.Add(n,i.Actors[n]);
        foreach(OptionInfo n in i.Options.Keys) Options.Add(n,i.Options[n]);
        foreach(VectorInfo n in i.Vectors.Keys) Vectors.Add(n,i.Vectors[n]);
        foreach(HitboxInfo n in i.Hitboxes.Keys) Hitboxes.Add(n,i.Hitboxes[n]);
        ActScript = i.ActScript;
        // Seed = i.Seed;
        Collision = i.Collision;
        SpawnReq = i.SpawnReq;
    }
    
    //Numbers
    public EventInfo Set(NumInfo i, float f)
    {
        return SetFloat(i, f);
    }
    public EventInfo Set(float f)
    {
        return SetFloat(NumInfo.Amount, f);
    }
    public EventInfo SetFloat(NumInfo i, float f)
    {
        if (!Numbers.TryAdd(i,f)) Numbers[i]=f;
        return this;
    }
    public EventInfo SetInt(NumInfo i, int f)
    {
        if (!Numbers.TryAdd(i,f)) Numbers[i]=f;
        return this;
    }
    public float Get(NumInfo i,float def=0)
    {
        return GetFloat(i,def);
    }
    public float GetN(NumInfo i=NumInfo.Amount,float def=0)
    {
        return GetFloat(i,def);
    }
    public float GetFloat(NumInfo i=NumInfo.Amount,float def=0)
    {
        if (Numbers.TryGetValue(i, out float r)) return r;
        return def;
    }
    public int GetInt(NumInfo i=NumInfo.Amount,int def=0)
    {
        if (Numbers.TryGetValue(i, out float r)) return (int)r;
        return def;
    }
    public float Change(float f)
    {
        return Change(NumInfo.Amount, f);
    }
    public float Change(NumInfo i, float f)
    {
        float r = Get(i);
        r += f;
        Set(i, r);
        return r;
    }
    
    //Text
    public EventInfo Set(string s)
    {
        return SetString(StrInfo.Text, s);
    }
    public EventInfo Set(StrInfo i, string s)
    {
        return SetString(i, s);
    }
    public EventInfo SetString(StrInfo i, string s)
    {
        if (!Texts.TryAdd(i,s)) Texts[i]=s;
        return this;
    }
    public string Get(StrInfo i)
    {
        return GetString(i);
    }
    public string GetString(StrInfo i=StrInfo.Text)
    {
        if (Texts.TryGetValue(i, out string r)) return r;
        return "";
    }
    
    //Actions
    public EventInfo Set(ActionInfo i, Actions v)
    {
        return SetAction(i, v);
    }
    public EventInfo SetAction(Actions v)
    {
        if (!Acts.TryAdd(ActionInfo.Default,v)) Acts[ActionInfo.Default]=v;
        return this;
    }
    public EventInfo SetAction(ActionInfo i, Actions v)
    {
        if (!Acts.TryAdd(i,v)) Acts[i]=v;
        return this;
    }
    public Actions Get(ActionInfo i=ActionInfo.Default)
    {
        return GetAction(i);
    }
    public Actions GetAction(ActionInfo i=ActionInfo.Default)
    {
        if (Acts.TryGetValue(i, out Actions r)) return r;
        return Actions.None;
    }
    
    
    //Bools
    public EventInfo Set(bool v=true)
    {
        return SetBool(BoolInfo.Default, v);
    }
    public EventInfo Set(BoolInfo i, bool v=true)
    {
        return SetBool(i, v);
    }
    public EventInfo SetBool(BoolInfo i, bool v=true)
    {
        bool has = Bools.Contains(i); 
        if(v && !has) Bools.Add(i);
        else if (!v && has) Bools.Remove(i);
        return this;
    }
    public bool Get(BoolInfo b)
    {
        return GetBool(b);
    }
    public bool GetBool(BoolInfo b=BoolInfo.Default)
    {
        return Bools.Contains(b);
    }
    
    //Actor
    public EventInfo Set(ThingController a)
    {
        return SetActor(ActorInfo.Target, a != null ? a.Info : null);
    }
    public EventInfo Set(ThingInfo a)
    {
        return SetActor(ActorInfo.Target, a);
    }
    public EventInfo Set(ActorInfo i, ThingInfo a)
    {
        return SetActor(i, a);
    }
    public EventInfo SetActor(ActorInfo i, ThingInfo a)
    {
        if (!Actors.TryAdd(i,a)) Actors[i]=a;
        return this;
    }
    public ThingInfo Get(ActorInfo i)
    {
        return GetActor(i);
    }
    public ThingInfo GetActor(ActorInfo i=ActorInfo.Target)
    {
        if (Actors.TryGetValue(i, out ThingInfo r)) return r;
        // if (Actors.TryGetValue(i, out ThingInfo r)) return r;
        return null;
    }
    
    //Option
    public EventInfo Set(ThingOption a)
    {
        return SetOption(OptionInfo.Default, a);
    }
    public EventInfo Set(OptionInfo i, ThingOption a)
    {
        return SetOption(i, a);
    }
    public EventInfo SetOption(OptionInfo i, ThingOption a)
    {
        if (!Options.TryAdd(i,a)) Options[i]=a;
        return this;
    }
    public ThingOption Get(OptionInfo i)
    {
        return GetOption(i);
    }
    public ThingOption GetOption(OptionInfo i=OptionInfo.Default)
    {
        if (Options.TryGetValue(i, out ThingOption r)) return r;
        return null;
    }
    
    //Vector
    public EventInfo Set(Vector2 a)
    {
        return SetVector(VectorInfo.Amount, a);
    }
    public EventInfo Set(VectorInfo i, Vector2 a)
    {
        return SetVector(i, a);
    }
    public EventInfo SetVector(VectorInfo i, Vector2 a)
    {
        if (!Vectors.TryAdd(i,a)) Vectors[i]=a;
        return this;
    }
    public Vector2 Get(VectorInfo i)
    {
        return GetVector(i);
    }
    public Vector2 GetVector(VectorInfo i=VectorInfo.Amount)
    {
        if (Vectors.TryGetValue(i, out Vector2 r)) return r;
        return Vector2.zero;
    }
    
    //Action
    public EventInfo Set(ActionScript a)
    {
        ActScript = a;
        return this;
    }
    // public EventInfo Set(ThingSeed a)
    // {
    //     Seed = a;
    //     return this;
    // }
    public EventInfo Set(HitboxController a)
    {
        SetHitbox(HitboxInfo.Theirs, a);
        return this;
    }
    public EventInfo Set(HitboxInfo i,HitboxController a)
    {
        SetHitbox(i, a);
        return this;
    }
    public EventInfo SetHitbox(HitboxInfo i, HitboxController a)
    {
        if (!Hitboxes.TryAdd(i,a)) Hitboxes[i]=a;
        return this;
    }
    public HitboxController Get(HitboxInfo i)
    {
        return GetHitbox(i);
    }
    public HitboxController GetHitbox(HitboxInfo i=HitboxInfo.Theirs)
    {
        if (Hitboxes.TryGetValue(i, out HitboxController r)) return r;
        return null;
    }
    
    public EventInfo Set(GameCollision a)
    {
        Collision = a;
        return this;
    }
    public EventInfo Set(SpawnRequest sr)
    {
        SpawnReq = sr;
        return this;
    }
    
    public override string ToString()
    {
        return "[EVENT:" + Type + "]("+BuildString()+")";
    }

    public virtual string BuildString()
    {
        string r = "";
        if (Abort) r += "##ABORT##";
        foreach (NumInfo l in Numbers.Keys) r = God.AddList(r, l+"<"+Numbers[l].ToString()+">");
        foreach (StrInfo l in Texts.Keys) r = God.AddList(r, l+"<"+Texts[l].ToString()+">");
        foreach (ActionInfo l in Acts.Keys) r = God.AddList(r, l+"<"+Acts[l].ToString()+">");
        foreach (BoolInfo l in Bools) r = God.AddList(r, l.ToString());
        foreach (ActorInfo l in Actors.Keys) r = God.AddList(r, l+"<"+Actors[l].ToString()+">");
        foreach (OptionInfo l in Options.Keys) r = God.AddList(r,l+"<"+Options[l].ToString()+">");
        foreach (VectorInfo l in Vectors.Keys) r = God.AddList(r, l+"<"+Vectors[l].ToString()+">");
        return r;
    }
    
    
}
