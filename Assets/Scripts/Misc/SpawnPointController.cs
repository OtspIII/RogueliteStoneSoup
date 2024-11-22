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
            Instantiate(God.Library.ActorPrefab, transform.position, transform.rotation).Imprint(God.JSON.Player,true);
        }
        else if (Type == SpawnTypes.Monster)
        {
            CharacterStats who = God.JSON.NPCs.Random();
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