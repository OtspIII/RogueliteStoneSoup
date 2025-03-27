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
    public Dictionary<StrInfo, string> Text = new Dictionary<StrInfo, string>();
    public Dictionary<EnumInfo, int> Enums = new Dictionary<EnumInfo, int>();
    public List<BoolInfo> Bools = new List<BoolInfo>();
    public Dictionary<ActorInfo, ThingController> Actor = new Dictionary<ActorInfo, ThingController>();
    public Dictionary<VectorInfo, Vector2> Vectors = new Dictionary<VectorInfo, Vector2>();
    public ActionScript Action;

    public EventInfo(EventTypes t)
    {
        Type = t;
    }
    
    //Numbers
    public EventInfo Set(NumInfo i, float f)
    {
        return SetFloat(i, f);
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
    public float GetFloat(NumInfo i)
    {
        if (Numbers.TryGetValue(i, out float r)) return r;
        return 0;
    }
    public int GetInt(NumInfo i)
    {
        if (Numbers.TryGetValue(i, out float r)) return (int)r;
        return 0;
    }
    public float Change(NumInfo i, float f)
    {
        float r = Get(i);
        r += f;
        Set(i, r);
        return r;
    }
    
    //Text
    public EventInfo Set(StrInfo i, string s)
    {
        return SetString(i, s);
    }
    public EventInfo SetString(StrInfo i, string s)
    {
        if (!Text.TryAdd(i,s)) Text[i]=s;
        return this;
    }
    public string Get(StrInfo i)
    {
        return GetString(i);
    }
    public string GetString(StrInfo i)
    {
        if (Text.TryGetValue(i, out string r)) return r;
        return "";
    }
    
    //Enums
    public EventInfo Set(EnumInfo i, int v)
    {
        return SetEnum(i, v);
    }
    public EventInfo SetEnum(EnumInfo i, int v)
    {
        if (!Enums.TryAdd(i,v)) Enums[i]=v;
        return this;
    }
    public T Get<T>(EnumInfo i)
    {
        return GetEnum<T>(i);
    }
    public T GetEnum<T>(EnumInfo i)
    {
        if (Enums.TryGetValue(i, out int r)) return (T)Enum.ToObject(typeof(T), r);
        return (T)Enum.ToObject(typeof(T), 0);
    }
    
    //Bools
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
    public bool GetBool(BoolInfo b)
    {
        return Bools.Contains(b);
    }
    
    //Actor
    public EventInfo Set(ActorInfo i, ThingController a)
    {
        return SetActor(i, a);
    }
    public EventInfo SetActor(ActorInfo i, ThingController a)
    {
        if (!Actor.TryAdd(i,a)) Actor[i]=a;
        return this;
    }
    public ThingController Get(ActorInfo i)
    {
        return GetActor(i);
    }
    public ThingController GetActor(ActorInfo i)
    {
        if (Actor.TryGetValue(i, out ThingController r)) return r;
        return null;
    }
    
    //Vector
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
    public Vector2 GetVector(VectorInfo i)
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
}
