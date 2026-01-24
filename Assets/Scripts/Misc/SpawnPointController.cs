using System;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    // public GameTags Type;
    public SpawnRequest ToSpawn;
    public bool AlwaysSpawn; //If this is true, it always spawns its thing and doesn't count for level totals

    public void Spawn()
    {
        // ToSpawn.Refine();
        ThingOption chosen = ToSpawn.FindThing();
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
        return (b != null ? b : God.LB).JudgeThing(ToSpawn,o) > 0;
    }
}
