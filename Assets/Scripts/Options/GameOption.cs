using System.Collections.Generic;
using UnityEngine;


///The parent class of all Options--all Options share a name and an author.
public class GameOption : ScriptableObject
{
    public string Name;
    public Authors Author;
    public List<Tag> Tags;             //The traits the thing has. Includes slots for customization

    public virtual bool HasTag(string tag)
    {
        return HasTag(tag, out float w, out float c);
    }
    
    public virtual bool HasTag(string tag, out float w)
    {
        return HasTag(tag, out w, out float c);
    }
    
    ///Returns whether or not the option has a tag, and if so what that tag's weight is. Used for spawning objects
    public virtual bool HasTag(string tag, out float w, out float cost)
    {
        //The 'something' tag is special in that it just refers to any object that might spawn in a random spot
        //So if the option is a npc or item, it returns true even if it doesn't have the 'something' tag itself
        bool something = tag ==  GameTags.Something.ToString();
        //Look at each tag on the option. If it's the tag the function is looking for, return 'true' and set w to be its weight 
        foreach (Tag t in Tags)
        {
            if (t.Value == tag || (something && God.LB.Somethings.Contains(t.Value)))
            {
                w = t.W != 0 ? t.W : 1;
                cost = t.Cost != 0 ? t.Cost : 1;
                if (w >= 10)
                {
                    God.LogWarning("Super High Tag Weight: " + Name + " / " + t.Value
                                   + " / " + t.W + " / " + Author);
                }
                return true;
            }
        }
        //If we looked at every tag and none were a match, return false.
        w = 0;
        cost = 1;
        return false;
    }

    public Tag GetTag(GameTags t)
    {
        return GetTag(t.ToString());
    }
    public Tag GetTag(string t)
    {
        foreach (Tag g in Tags)
        {
            if (g.Value == t) return g;
        }

        return null;
    }
}
