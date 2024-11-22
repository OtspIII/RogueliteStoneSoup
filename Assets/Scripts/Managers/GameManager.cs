using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<SpawnPointController> SpawnPoints;

    private void Awake()
    {
        God.GM = this;
    }

    void Start()
    {
        BuildLevel();
    }

    public virtual void BuildLevel()
    {
        foreach (SpawnPointController s in SpawnPoints)
        {
            s.Spawn();
        }
    }

    public void AddSpawn(SpawnPointController s)
    {
        SpawnPoints.Add(s);
    }
}
