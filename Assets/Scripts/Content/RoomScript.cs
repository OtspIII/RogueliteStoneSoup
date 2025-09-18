using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public Transform SpawnerHolder;
    public List<SpawnPointController> Spawners;
    public SpawnPointController PlayerSpawn;
    
    public void Setup()
    {
        transform.parent = God.GM.LevelHolder;
        if (SpawnerHolder == null) SpawnerHolder = transform;
        for(int n = 0;n < SpawnerHolder.childCount;n++)
        {
            SpawnPointController s = SpawnerHolder.GetChild(n).GetComponent<SpawnPointController>();
            if (s != null)
            {
                God.GM.AddSpawn(s);
                if (s.Type == Tags.Player)
                    PlayerSpawn = s;
                else
                    Spawners.Add(s);
            }
        }
    }

    public void Spawn(bool playerRoom=false)
    {
        if (playerRoom)
        {
            PlayerSpawn.Spawn();
            return;
        }
        foreach(SpawnPointController s in Spawners)
            s.Spawn();
    }
}
