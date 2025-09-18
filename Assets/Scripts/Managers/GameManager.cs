using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform LevelHolder;
    public List<SpawnPointController> SpawnPoints;
    public TextAsset JSON;
    public TextMeshProUGUI HealthTxt;
    public List<RoomScript> Rooms;

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
        LevelHolder = new GameObject("Level").transform;
        for(float x = 0;x < 3;x++)
        for (float y = 0; y < 3; y++)
        {
            RoomScript rm = Instantiate(God.Library.GetRoom(), new Vector3(x * God.RoomSize.x, y * God.RoomSize.y, 0), Quaternion.identity);
            rm.Setup();
            Rooms.Add(rm);
        }
        RoomScript chosen = God.Random(Rooms);
        foreach (RoomScript rs in Rooms)
        {
            rs.Spawn(rs == chosen);
        }
    }

    public void AddSpawn(SpawnPointController s)
    {
        SpawnPoints.Add(s);
    }
}
