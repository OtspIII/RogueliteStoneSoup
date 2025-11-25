using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnRequest
{
    public List<GameTags> Mandatory = new List<GameTags>();
    public List<GameTags> Any = new List<GameTags>();

    public float Judge(ThingOption o, LevelBuilder b=null)
    {
        float w = 1; //Should actually be calculated eventually
        foreach(GameTags t in Mandatory)
            if (!o.Tags.Contains(t))
                return 0;
        if (Any.Count > 0)
        {
            bool any = false;
            foreach(GameTags t in Any)
                if (!o.Tags.Contains(t))
                {
                    any = true;
                    break;
                }
            if(!any)
                return 0;
        }
        return w;
    }
}
