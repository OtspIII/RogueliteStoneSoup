using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<SpawnPointController> SpawnPoints;
    public TextAsset JSON;

    private void Awake()
    {
        God.GM = this;
        God.JSON = JSONReader.ParseJSON(JSON.text);
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
