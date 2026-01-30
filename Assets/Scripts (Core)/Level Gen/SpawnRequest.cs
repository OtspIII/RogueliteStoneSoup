using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SpawnRequest
{
    // bool Refined = false;
    public ThingOption Fixed;
    public List<Tag> Mandatory = new List<Tag>();
    public List<Tag> Any = new List<Tag>();
    public int Level = 0;
    public Authors Author;

    public SpawnRequest(ThingOption o)
    {
        if(God.Session != null)
            Author = God.Session.Author;
        Fixed = o;
    }
    
    public SpawnRequest(params GameTags[] tags)
    {
        Author = God.Session.Author;
        // Debug.Log("SR1: " + tags);
        foreach (GameTags t in tags)
        {
            // Debug.Log("SR2: " + t);
            Mandatory.Add(new Tag(t));
        }
    }
    
    public SpawnRequest(params string[] tags)
    {
        Author = God.Session.Author;
        foreach (string t in tags)
        {
            Mandatory.Add(new Tag(t));
        }
    }
    
    public SpawnRequest(params Tag[] tags)
    {
        Author = God.Session.Author;
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
        if (Fixed != null) return Fixed;
        if (Mandatory.Count == 0 && Any.Count == 0)
        {
            Debug.LogWarning("Spawn request totally blank: could spawn anything.");
        }
        return God.Library.GetThing(this);
    }

    public SpawnRequest SetAuthor(Authors a)
    {
        Author = a;
        return this;
    }
    
    

    // public bool JudgeLevel(ThingOption o)
    // {
    //     Debug.Log("AUTHORS: " + Author + " / " + o.Author);
    //     //Make sure it's the right author. If either the option or the game is universal, it's okay
    //     if (Author != Authors.Universal && o.Author != Authors.Universal && o.Author != Author) return false;
    //     //If the level is set to -1 then anything is okay
    //     if (Level < 0) return true;
    //     //If the Option's level range isn't set up, it's okay
    //     if (o.LevelRange == Vector2Int.zero) return true;
    //     //If we didn't set the level manually just use the game session's current level
    //     int l = Level != 0 ? Level : God.Session.Level;
    //     //If we're too high or low level, it's not okay
    //     if (l < o.LevelRange.x && o.LevelRange.x > 0) return false;
    //     if (l > o.LevelRange.y && o.LevelRange.y > 0) return false;
    //     //If nothing went wrong, we're good
    //     return true;
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