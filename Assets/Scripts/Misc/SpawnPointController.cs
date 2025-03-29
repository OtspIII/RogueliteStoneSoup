using System;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public SpawnTypes Type;

    private void Awake()
    {
        God.GM.AddSpawn(this);
    }

    public void Spawn()
    {
        if (Type == SpawnTypes.Player)
        {
            Instantiate(God.Library.ActorPrefab, transform.position, transform.rotation).Imprint(ThingBuilder.Player);
        }
        else if (Type == SpawnTypes.Monster)
        {
            ThingSeed who = ThingBuilder.GetTag("NPC").Random();
            Instantiate(God.Library.ActorPrefab, transform.position, transform.rotation).Imprint(who);
        }
        Destroy(gameObject);
    }
}

public enum SpawnTypes
{
    None=0,
    Player=1,
    Monster=2
}