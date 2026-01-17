using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnRequest
{
    bool Refined = false;
    public ThingOption Fixed;
    public List<Tag> Mandatory = new List<Tag>();
    public List<Tag> Any = new List<Tag>();

    public SpawnRequest(params GameTags[] tags)
    {
        // Debug.Log("SR1: " + tags);
        foreach (GameTags t in tags)
        {
            // Debug.Log("SR2: " + t);
            Mandatory.Add(new Tag(t));
        }
    }
    
    public SpawnRequest(params string[] tags)
    {
        foreach (string t in tags)
        {
            Mandatory.Add(new Tag(t));
        }
    }
    
    public SpawnRequest(params Tag[] tags)
    {
        foreach (Tag t in tags)
        {
            Mandatory.Add(t);
        }
    }

    public bool HasTag(GameTags tag) { return HasTag(tag, out float w); }
    public bool HasTag(string tag) { return HasTag(tag, out float w); }
    public bool HasTag(GameTags tag, out float w) { return HasTag(tag.ToString(),out w); }
    public bool HasTag(string tag,out float w)
    {
        bool r = false;
        foreach (Tag t in Mandatory)
            if (t.Value == tag)
            {
                w = t.W != 0 ? t.W : 1;
                return true;
            }
        foreach (Tag t in Any)
            if (t.Value == tag)
            {
                w = t.W != 0 ? t.W : 1;
                return true;
            }
        w = 0;
        return false;
    }

    public ThingOption FindThing()
    {
        // if (!Refined)
        // {
        //     Refine();
        //     Refined = true;
        // }
        if (Fixed != null) return Fixed;
        return God.Library.GetThing(this);
    }

   
    
    public float Judge(ThingOption o)
    {
        float w = 1;
        foreach(Tag t in Mandatory)
            if (o.HasTag(t.Value, out float tw))
            {
                w = God.MergeWeight(w,tw);
            }
            else return 0;
        if (Any.Count > 0)
        {
            bool any = false;
            foreach (Tag t in Any)
            {
                if (o.HasTag(t.Value, out float tw))
                {
                    w = God.MergeWeight(w, tw);
                    any = true;
                }
            }
            if(!any)
                return 0;
        }
        return w;
    }

    // public void Refine()
    // {
    //     if (God.GM.DebugSpawn != null) return;
    //     for (int n = 0; n < Mandatory.Count; n++)
    //     {
    //         if (Mandatory[n].Value == GameTags.Something.ToString())
    //         {
    //             Mandatory[n].Custom = God.CoinFlip() ? GameTags.NPC.ToString() : GameTags.Pickup.ToString();
    //         }
    //     }
    //
    //     // if (Any.Contains(GameTags.Something))
    //     // {
    //     //     Any.Add(GameTags.NPC);
    //     //     Any.Add(GameTags.Pickup);
    //     // }
    // }

    public override string ToString()
    {
        string r = "";
        foreach (Tag t in Mandatory) r += t.ToString().ToUpper()+",";
        foreach (Tag t in Any) r += t.ToString().ToLower()+",";
        return "SPAWN REQUEST[" + r + "]";
    }
}


[System.Serializable]
public class Tag
{
    public GameTags Common = GameTags.Custom;
    public string Custom="";
    public string Value { get { return Custom != "" ? Custom : Common.ToString() ; } }
    public float W = 1;

    public Tag()
    {
        W = 1;
    }
    
    public Tag(GameTags t, float w=1)
    {
        Common = t;
        W = w;
    }

    public Tag(string t, float w=1)
    {
        Custom = t;
        W = w;
    }

    public override string ToString()
    {
        return Value+"("+W+")";
    }
}