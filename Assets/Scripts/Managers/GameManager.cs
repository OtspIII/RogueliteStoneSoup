using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<SpawnPointController> SpawnPoints;
    public TextAsset JSON;
    public TextMeshProUGUI HealthTxt;

    private void Awake()
    {
        God.GM = this;
        TraitManager.Init();
        ThingBuilder.Init();
        God.JSON = JSONReader.ParseJSON(JSON.text);
    }

    void Start()
    {
        God.Library.Setup();
        BuildLevel();
    }

    void Update()
    {
        EventInfo e = God.E(EventTypes.ShownHP);
        God.Player.TakeEvent(e);
        if (God.Player != null) HealthTxt.text = "Health: " + e.Get(NumInfo.Amount);
        else HealthTxt.text = "GAME OVER";
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
