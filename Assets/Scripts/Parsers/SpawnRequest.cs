using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnRequest
{
    public List<GameTags> Mandatory = new List<GameTags>();
    public List<GameTags> Any = new List<GameTags>();

    public SpawnRequest(params GameTags[] tags)
    {
        foreach (GameTags t in tags)
        {
            Mandatory.Add(t);
        }
    }
    
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

    public void Refine()
    {
        if (God.GM.DebugSpawn != null) return;
        for (int n = 0; n < Mandatory.Count; n++)
        {
            if (Mandatory[n] == GameTags.Something)
            {
                Mandatory[n] = God.CoinFlip() ? GameTags.NPC : GameTags.Pickup;
            }
        }

        if (Any.Contains(GameTags.Something))
        {
            Any.Add(GameTags.NPC);
            Any.Add(GameTags.Pickup);
        }
    }

    public override string ToString()
    {
        string r = "";
        foreach (GameTags t in Mandatory) r += t.ToString().ToUpper()+",";
        foreach (GameTags t in Any) r += t.ToString().ToLower()+",";
        return "SPAWN REQUEST[" + r + "]";
    }
}
