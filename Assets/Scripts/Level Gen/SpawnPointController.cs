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
            if(!ToSpawn.HasTag(GameTags.Debug))
                Debug.LogWarning("Tried to spawn but couldn't: " + ToSpawn);
            return;
        }

        ThingInfo i = chosen.Create();
        i.Spawn(this);
        Destroy(gameObject);
    }

    public bool CanSpawn(ThingOption o,LevelBuilder b=null,bool backup=false)
    {
        if (b == null) b = God.LB;
        return (b != null ? b : God.LB).JudgeThing(ToSpawn,o,backup) > 0;
    }
}
