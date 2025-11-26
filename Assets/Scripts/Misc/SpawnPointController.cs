using System;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public GameTags Type;
    public SpawnRequest ToSpawn;

    public void Spawn()
    {
        ThingOption chosen = God.Library.GetThing(ToSpawn);
        if (chosen == null)
        {
            Debug.Log(Type);
            return;
        }
        chosen.Spawn(this);
        Destroy(gameObject);
    }
}
