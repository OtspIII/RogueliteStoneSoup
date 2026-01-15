using System;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    // public GameTags Type;
    public SpawnRequest ToSpawn;
    public bool AlwaysSpawn; //If this is true, it always spawns its thing and doesn't count for level totals

    public void Spawn(LevelBuilder b)
    {
        ToSpawn.Refine();
        ThingOption chosen = ToSpawn.FindThing(b);
        if (chosen == null)
        {
            Debug.Log(ToSpawn);
            return;
        }

        ThingInfo i = chosen.Create();
        i.Spawn(this);
        Destroy(gameObject);
    }

    public bool CanSpawn(ThingOption o,LevelBuilder b=null)
    {
        return ToSpawn.Judge(o,b) > 0;
    }
}
