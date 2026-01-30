using System;
using System.Collections.Generic;
using UnityEngine;

//This Attribute makes it so that we can right click->Create->Scriptable Objects and make SO objects in our asset folder
[CreateAssetMenu(fileName = "ThingOption", menuName = "Scriptable Objects/ThingOption")]
public class ThingOption : GameOption //A generic class for anything that might spawn in a level
{
    public GameTeams Team = GameTeams.Neutral; //An enum that prevents characters on the same team from doing friendly fire
    public BodyController Body;                //A link to this thing's body prefab
    public Sprite Art;                         //If this isn't null, override the body's art with this sprite
    public Color Color = Color.white;        //Dye the body's spriterenderer with this color
    public float Size = 1;                     //Changes the size of the thing
    public List<Tag> Tags;             //The traits the thing has. Includes slots for customization
    public Vector2Int LevelRange = new Vector2Int(0,0); //Selects what levels the thing is valid to spawn in.                  //A list of tags this thing has. Used to know what's valid to spawn
    public List<TraitBuilder> Trait; 
    
    ///Called when the thing is first created. Makes its ThingInfo, but not its ThingController. That's done by ThingInfo.Spawn()
    public virtual ThingInfo Create()
    {
        //Make a new ThinInfo using this option as a seed
        ThingInfo r = new ThingInfo(this);
        //Copy over our info
        r.Name = Name;
        r.Team = Team;
        //Set up the traits. The TraitBuilder class holds all the info a trait needs to get set up, so plug all that info in
        foreach (TraitBuilder t in Trait)
        {
            //This is the was of info that will be given to the trait to let it set itself up
            EventInfo ts = new EventInfo();
            foreach(InfoNumber n in t.Numbers)
                ts.Numbers.Add(n.Type != NumInfo.None ? n.Type : NumInfo.Amount,n.Value);
            foreach(InfoOption n in t.Prefabs)
                ts.Options.Add(n.Type != OptionInfo.None ? n.Type : OptionInfo.Default,n.Value);
            foreach (InfoAction n in t.Acts)
                ts.Acts.Add(n.Type != ActionInfo.None ? n.Type : ActionInfo.Default,n.Act);
            ts.SpawnReq = t.SpawnReq;
            //Once the info is all transcribed, tell the Thing to add the Trait to itself
            r.AddTrait(t.Trait, ts);
        }
        //Return a link to the ThingInfo we just made
        return r;
    }

    //These functions just return specific variables.
    //They're mostly useful because they can be overridden by children if those children are doing something fancy
    
    /// Returns the body prefab of the ThingOption
    /// <param name="held">Some Things spawn a different body prefab when they're being held than on the ground (weapons)</param>
    public virtual BodyController GetBody(bool held = false)
    {
        return Body;
    }
    public virtual Sprite GetArt(bool held = false)
    {
        return Art;
    }

    ///Returns whether or not the option has a tag, and if so what that tag's weight is. Used for spawning objects
    public virtual bool HasTag(string tag, out float w)
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
                return true;
            }
        }
        //If we looked at every tag and none were a match, return false.
        w = 0;
        return false;
    }
}