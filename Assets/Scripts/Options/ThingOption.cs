using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorOption", menuName = "Scriptable Objects/ActorOption")]
public class ThingOption : ScriptableObject
{
    public string Name;
    public BodyController Body;
    public Sprite Art;
    public List<GameTags> Tags;
    public List<TraitBuilder> Trait;
    
    
    // public bool Valid(SpawnPointController s, LevelBuilder b)
    // {
    //     return Tags.Contains(s.Type);
    // }

    
    public virtual ThingInfo Create()
    {
        ThingInfo r = new ThingInfo(this);
        r.Name = Name;
        foreach (TraitBuilder t in Trait)
        {
            EventInfo ts = new EventInfo();
            foreach(InfoNumber n in t.Numbers)
                ts.Numbers.Add(n.Type,n.Value);
            r.AddTrait(t.Trait, ts);
            // Debug.Log("ADD TRAIT: " + t.Trait + " / " + Name);
        }

        return r;
    }
    
    //
    // public virtual void Imprint(ThingInfo r)
    // {
    //     r.Name = Name;
    //     foreach (TraitBuilder t in Trait)
    //     {
    //         EventInfo ts = new EventInfo();
    //         foreach(InfoNumber n in t.Numbers)
    //             ts.Numbers.Add(n.Type,n.Value);
    //         r.AddTrait(t.Trait, ts);
    //     }
    // }
}

[System.Serializable]
public class TraitBuilder
{
    public Traits Trait;
    public List<InfoNumber> Numbers;
}

[System.Serializable]
public class InfoNumber
{
    public NumInfo Type;
    public float Value;
}