using System;
using System.Collections.Generic;
using UnityEngine;


public class Infos
{
    public Dictionary<IntI, int> IInfo = new Dictionary<IntI, int>();
    public Dictionary<FloatI, float> FInfo = new Dictionary<FloatI, float>();
    public Dictionary<StringI, string> SInfo = new Dictionary<StringI, string>();

    public Infos()
    {
        
    }
    
    // public Infos(RawInfoJSON raw)
    // {
    //     if (raw.Amt.HasValue)
    //     {
    //         FInfo.Add(FloatI.Amt,raw.Amt.Value);
    //         IInfo.Add(IntI.Amt,Mathf.FloorToInt(raw.Amt.Value));
    //     }
    //     if (raw.Priority.HasValue) FInfo.Add(FloatI.Priority,raw.Priority.Value);
    //     if(string.IsNullOrEmpty(raw.Text)) SInfo.Add(StringI.Text,raw.Text);
    // }

    public Infos Add(IntI a, int b) { IInfo.Add(a,b); return this; }
    public Infos Add(FloatI a, float b) { FInfo.Add(a,b); return this; }
    public Infos Add(StringI a, string b) { SInfo.Add(a,b); return this; }
    
    public int Get(IntI i,int def=-5238)
    {
        if (!IInfo.ContainsKey(i))
        {
            if (def == -5238)
            {
                Debug.Log("UNFOUND INFO: " + i);
                return 0;
            }
            return def;
        }

        return IInfo[i];
    }
    public float Get(FloatI i,float def=-5238)
    {
        if (!FInfo.ContainsKey(i))
        {
            if (def == -5238)
            {
                Debug.Log("UNFOUND INFO: " + i);
                return 0;
            }
            return def;
        }

        return FInfo[i];
    }
    public string Get(StringI i,string def="FAKE STRING")
    {
        if (!SInfo.ContainsKey(i))
        {
            if (def == "FAKE STRING")
            {
                Debug.Log("UNFOUND INFO: " + i);
                return "";
            }
            return def;
        }

        return SInfo[i];
    }
    
}

public enum StringI
{
    None=0,
    Text=1,
}

public enum IntI
{
    None=0,
    Amt=1,
}

public enum FloatI
{
    None=0,
    Amt=1,
    Priority=2
}