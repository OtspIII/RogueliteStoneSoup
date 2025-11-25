using System;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class EventInfo
{
    public EventTypes Type;
    public bool Abort = false;
    public Dictionary<NumInfo, float> Numbers = new Dictionary<NumInfo, float>();
    public Dictionary<StrInfo, string> Texts = new Dictionary<StrInfo, string>();
    public Dictionary<EnumInfo, int> Enums = new Dictionary<EnumInfo, int>();
    public List<BoolInfo> Bools = new List<BoolInfo>();
    public Dictionary<ActorInfo, ThingController> Actors = new Dictionary<ActorInfo, ThingController>();
    public Dictionary<VectorInfo, Vector2> Vectors = new Dictionary<VectorInfo, Vector2>();
    public ActionScript Action;
    public ThingSeed Seed;

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
        foreach(EnumInfo n in i.Enums.Keys) Enums.Add(n,i.Enums[n]);
        foreach(BoolInfo n in i.Bools) Bools.Add(n);
        foreach(ActorInfo n in i.Actors.Keys) Actors.Add(n,i.Actors[n]);
        foreach(VectorInfo n in i.Vectors.Keys) Vectors.Add(n,i.Vectors[n]);
        Action = i.Action;
        Seed = i.Seed;
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
    public float Get(NumInfo i)
    {
        return GetFloat(i);
    }
    public float GetN(NumInfo i=NumInfo.Amount)
    {
        return GetFloat(i);
    }
    public float GetFloat(NumInfo i=NumInfo.Amount)
    {
        if (Numbers.TryGetValue(i, out float r)) return r;
        return 0;
    }
    public int GetInt(NumInfo i=NumInfo.Amount)
    {
        if (Numbers.TryGetValue(i, out float r)) return (int)r;
        return 0;
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
    
    //Enums
    public EventInfo Set(EnumInfo i, int v)
    {
        return SetEnum(i, v);
    }
    public EventInfo SetEnum(int v)
    {
        if (!Enums.TryAdd(EnumInfo.Default,v)) Enums[EnumInfo.Default]=v;
        return this;
    }
    public EventInfo SetEnum(EnumInfo i, int v)
    {
        if (!Enums.TryAdd(i,v)) Enums[i]=v;
        return this;
    }
    public T Get<T>(EnumInfo i=EnumInfo.Default)
    {
        return GetEnum<T>(i);
    }
    public T GetEnum<T>(EnumInfo i=EnumInfo.Default)
    {
        if (Enums.TryGetValue(i, out int r)) return (T)Enum.ToObject(typeof(T), r);
        return (T)Enum.ToObject(typeof(T), 0);
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
        return SetActor(ActorInfo.Target, a);
    }
    public EventInfo Set(ActorInfo i, ThingController a)
    {
        return SetActor(i, a);
    }
    public EventInfo SetActor(ActorInfo i, ThingController a)
    {
        if (!Actors.TryAdd(i,a)) Actors[i]=a;
        return this;
    }
    public ThingController Get(ActorInfo i)
    {
        return GetActor(i);
    }
    public ThingController GetActor(ActorInfo i=ActorInfo.Target)
    {
        if (Actors.TryGetValue(i, out ThingController r)) return r;
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
        Action = a;
        return this;
    }
    public EventInfo Set(ThingSeed a)
    {
        Seed = a;
        return this;
    }
    
}
