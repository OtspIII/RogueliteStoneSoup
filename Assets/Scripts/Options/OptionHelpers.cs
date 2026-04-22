using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

//This and the below classes just exist so the player can input data in the Unity Inspector when customizing Options
[System.Serializable]
public class TraitBuilder
{
    public Traits Trait;
    [HideInInspector]public List<InfoNumber> Numbers;
    [HideInInspector]public List<InfoOption> Prefabs;
    [HideInInspector]public List<InfoAction> Acts;
    [HideInInspector]public Traits OtherTrait;
    [HideInInspector]public SpawnRequest SpawnReq;
    public List<TraitPicker> Builder;
}

[System.Serializable]
public class InfoNumber
{
    public NumInfo Type=NumInfo.Default;
    public float Value;
}

[System.Serializable]
public class InfoOption
{
    public OptionInfo Type=OptionInfo.Default;
    public ThingOption Value;
}

[System.Serializable]
public class InfoAction
{
    public ActionInfo Type=ActionInfo.None;
    public Actions Act;
}


[System.Serializable]
public class TraitPicker
{
    public InfoTypes Type;
    public NumInfo NumType;
    public float Num;
    public StrInfo StrType;
    public string Str;
    public ActionInfo ActType;
    public Actions Act;
    public BoolInfo BoolType;
    public bool Bool;
    public OptionInfo OptionType;
    public ThingOption Option;
    public VectorInfo VectorType;
    public Vector2 Vector;
    public ThingOption Fixed;
    public string Mandatory;
    public float MandatoryW;
    public string Any;
    public float AnyW;
    public Traits Trait;

    public TraitPicker(NumInfo i,float v)
    {
        Type = InfoTypes.Number;
        NumType = i;
        Num = v;
    }
    public TraitPicker(StrInfo i,string v)
    {
        Type = InfoTypes.String;
        StrType = i;
        Str = v;
    }
    public TraitPicker(ActionInfo i,Actions v)
    {
        Type = InfoTypes.Action;
        ActType = i;
        Act = v;
    }
    public TraitPicker(BoolInfo i,bool v)
    {
        Type = InfoTypes.Bool;
        BoolType = i;
        Bool = v;
    }
    public TraitPicker(OptionInfo i,ThingOption v)
    {
        Type = InfoTypes.Option;
        OptionType = i;
        Option = v;
    }
    public TraitPicker(VectorInfo i,Vector2 v)
    {
        Type = InfoTypes.Vector;
        VectorType = i;
        Vector = v;
    }
    public TraitPicker(SpawnRequest v)
    {
        Type = InfoTypes.SpawnRequest;
        Fixed = v.Fixed;
        Mandatory = v.Mandatory.Count > 0 ? v.Mandatory[0].Value : "";
        Any = v.Any.Count > 0 ? v.Any[0].Value : "";
    }
    public TraitPicker(Traits v)
    {
        Type = InfoTypes.Trait;
        Trait = v;
    }

    public void Apply(EventInfo e)
    {
        switch (Type)
        {
            case InfoTypes.Number:
            {
                NumInfo i = NumType == NumInfo.None ? NumInfo.Default : NumType;
                e.Set(i, Num);
                break;
            }
            case InfoTypes.String:
            {
                StrInfo i = StrType == StrInfo.None ? StrInfo.Default : StrType;
                e.Set(i, Str);
                break;
            }
            case InfoTypes.Action:
            {
                ActionInfo i = ActType == ActionInfo.None ? ActionInfo.Default : ActType;
                e.Set(i, Act);
                break;
            }
            case InfoTypes.Bool:
            {
                BoolInfo i = BoolType == BoolInfo.None ? BoolInfo.Default : BoolType;
                e.Set(i, Bool);
                break;
            }
            case InfoTypes.Option:
            {
                OptionInfo i = OptionType == OptionInfo.None ? OptionInfo.Default : OptionType;
                e.Set(i, Option);
                break;
            }
            case InfoTypes.Vector:
            {
                VectorInfo i = VectorType == VectorInfo.None ? VectorInfo.Amount : VectorType;
                e.Set(i, Vector);
                break;
            }
            case InfoTypes.SpawnRequest:
            {
                SpawnRequest sr = new SpawnRequest(Fixed);
                sr.Mandatory = new List<Tag>(){};
                if(Mandatory != "") sr.Mandatory.Add(new Tag(Mandatory,MandatoryW));
                sr.Any = new List<Tag>(){};
                if(Any != "") sr.Any.Add(new Tag(Any,AnyW));
                e.Set(sr);
                break;
            }
            case InfoTypes.Trait:
            {
                e.Set(Trait);
                break;
            }
        }
    }
}

public enum InfoTypes
{
    Number=0,
    String=1,
    Action=2,
    Bool=3,
    Option=4,
    Vector=5,
    SpawnRequest=6,
    Trait=7,
}

[CustomPropertyDrawer(typeof(TraitPicker), true)]
public class TraitDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("Type"));
        InfoTypes t = (InfoTypes)property.FindPropertyRelative("Type").enumValueFlag;
        switch (t)
        {
            case InfoTypes.Number:
            {
                AddLine(ref position,property,"NumType");
                AddLine(ref position,property,"Num");
                break;
            }
            case InfoTypes.String:
            {
                AddLine(ref position,property,"StrType");
                AddLine(ref position,property,"Str"); 
                break;
            }
            case InfoTypes.Action:
            {
                AddLine(ref position,property,"ActType");
                AddLine(ref position,property,"Act"); 
                break;
            }
            case InfoTypes.Bool:
            {
                AddLine(ref position,property,"BoolType");
                AddLine(ref position,property,"Bool"); 
                break;
            }
            case InfoTypes.Option:
            {
                AddLine(ref position,property,"OptionType");
                AddLine(ref position,property,"Option"); 
                break;
            }
            case InfoTypes.Vector:
            {
                AddLine(ref position,property,"VectorType");
                AddLine(ref position,property,"Vector"); 
                break;
            }
            case InfoTypes.SpawnRequest:
            {
                AddLine(ref position,property,"Fixed");
                AddLine(ref position,property,"Mandatory");
                AddLine(ref position,property,"MandatoryW");
                AddLine(ref position,property,"Any");
                AddLine(ref position,property,"AnyW");
                break;
            }
            case InfoTypes.Trait:
            {
                AddLine(ref position,property,"Trait");
                break;
            }
            default: EditorGUI.LabelField(position, "MISC!"); break;
        }
    }

    public void AddLine(ref Rect position,SerializedProperty property,string prop)
    {
        position.y += EditorGUIUtility.singleLineHeight;
        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight+EditorGUIUtility.standardVerticalSpacing);
        EditorGUI.PropertyField(position, property.FindPropertyRelative(prop));
    }
}