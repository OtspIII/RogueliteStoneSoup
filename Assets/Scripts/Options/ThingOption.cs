using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActorOption", menuName = "Scriptable Objects/ActorOption")]
public class ThingOption : ScriptableObject
{
    public string Name;
    public BodyController Body;
    public string Art;
    public List<GameTags> Tags;
    public List<TraitBuilder> Trait;
    
    public bool Valid(SpawnPointController s, LevelBuilder b)
    {
        return Tags.Contains(s.Type);
    }

    public ThingController Spawn(SpawnPointController where)
    {
        return Spawn(where.transform.position, where.transform.rotation.eulerAngles.z);
    }
    
    public ThingController Spawn(Vector3 pos)
    {
        return Spawn(pos, 0);
    }
    
    public virtual ThingController Spawn(Vector3 pos, float rot)
    {
        ThingController r = Instantiate(God.Library.ActorPrefab, pos, Quaternion.Euler(0, 0, rot));
        Imprint(r);
        r.TakeEvent(EventTypes.Setup);
        r.Body = Instantiate(Body, r.transform);
        r.Body.Setup(r,Art);
        if(r.Body.Anim != null)
            r.Body.Anim.Rebind();
        return r;
    }

    public virtual void Imprint(ThingController r)
    {
        r.name = Name;
        r.Name = Name;
        //Placeholder
        // r.Stats = new CharacterStats();
        foreach (TraitBuilder t in Trait)
        {
            EventInfo ts = new EventInfo();
            foreach(InfoNumber n in t.Numbers)
                ts.Numbers.Add(n.Type,n.Value);
            r.AddTrait(t.Trait, ts);
        }
    }
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