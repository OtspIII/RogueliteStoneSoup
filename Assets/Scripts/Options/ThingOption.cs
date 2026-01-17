using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThingOption", menuName = "Scriptable Objects/ThingOption")]
public class ThingOption : ScriptableObject
{
    public string Name;
    public GameTeams Team = GameTeams.Neutral;
    public BodyController Body;
    public Sprite Art;
    public Color Color = Color.white;
    public List<GameTags> Tags;
    public List<TraitBuilder> Trait;
    public float Size = 1;
    
    
    public virtual ThingInfo Create()
    {
        ThingInfo r = new ThingInfo(this);
        r.Name = Name;
        r.Team = Team;
        foreach (TraitBuilder t in Trait)
        {
            EventInfo ts = new EventInfo();
            foreach(InfoNumber n in t.Numbers)
                ts.Numbers.Add(n.Type != NumInfo.None ? n.Type : NumInfo.Amount,n.Value);
            foreach(InfoOption n in t.Prefabs)
                ts.Options.Add(n.Type != OptionInfo.None ? n.Type : OptionInfo.Default,n.Value);
            foreach (InfoAction n in t.Acts)
                ts.Acts.Add(n.Type != ActionInfo.None ? n.Type : ActionInfo.Default,n.Act);
            ts.SpawnReq = t.SpawnReq;
            r.AddTrait(t.Trait, ts);
            // Debug.Log("ADD TRAIT: " + t.Trait + " / " + Name);
        }
        return r;
    }

    public virtual BodyController GetBody(bool held = false)
    {
        return Body;
    }
    
    public virtual Sprite GetArt(bool held = false)
    {
        return Art;
    }
   
}

[System.Serializable]
public class TraitBuilder
{
    public Traits Trait;
    public List<InfoNumber> Numbers;
    public List<InfoOption> Prefabs;
    public List<InfoAction> Acts;
    public SpawnRequest SpawnReq;
}

[System.Serializable]
public class InfoNumber
{
    public NumInfo Type=NumInfo.Amount;
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