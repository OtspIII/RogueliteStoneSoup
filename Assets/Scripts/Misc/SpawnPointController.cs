using System;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public SpawnTypes Type;

    public PlayerController PlayerPrefab;
    public ActorController MonsterPrefab;

    private void Awake()
    {
        God.GM.AddSpawn(this);
    }

    public void Spawn()
    {
        if (Type == SpawnTypes.Player)
        {
            Instantiate(PlayerPrefab, transform.position, transform.rotation);
        }
        else if (Type == SpawnTypes.Monster)
        {
            Instantiate(PlayerPrefab, transform.position, transform.rotation);
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