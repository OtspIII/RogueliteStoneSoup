using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelBuilder Level;
    [HideInInspector] public Transform LevelHolder;
    public List<SpawnPointController> SpawnPoints;
    // public TextAsset JSON;
    public TextMeshProUGUI HealthTxt;
    public TextMeshProUGUI InvTxt;
    public List<RoomScript> Rooms;
    public List<ThingInfo> PlayerInventory;
    public int InventoryIndex = 1;

    private void Awake()
    {
        God.GM = this;
        TraitManager.Init();
        ThingBuilder.Init();
        // God.JSON = JSONReader.ParseJSON(JSON.text);
    }

    void Start()
    {
        God.Library.Setup();
        BuildLevel();
    }

    void Update()
    {
        if (God.Player != null)
        {
            EventInfo e = God.Player.Ask(EventTypes.ShownHP);//God.E(EventTypes.ShownHP);
            HealthTxt.text = "Health: " + e.Get(NumInfo.Amount);
        }
        else HealthTxt.text = "GAME OVER";
    }

    public virtual void BuildLevel()
    {
        LevelHolder = new GameObject("Level").transform;
        Level = new LevelBuilder();
        foreach (GeoTile g in Level.AllGeo)
        {
            RoomOption rm = God.Library.GetRoom(g, Level);
            RoomScript rs = rm.Build(g,Level);
            if(rs != null)
                Rooms.Add(rs);
        }
        // for(float x = 0;x < 3;x++)
        // for (float y = 0; y < 3; y++)
        // {
        //     RoomScript rm = Instantiate(God.Library.GetRoom(), new Vector3(x * God.RoomSize.x, y * God.RoomSize.y, 0), Quaternion.identity);
        //     rm.Setup();
        //     Rooms.Add(rm);
        // }
        // RoomScript chosen = God.Random(Rooms);
        foreach (RoomScript rs in Rooms)
        {
            rs.Spawn();
        }
    }

    public void AddSpawn(SpawnPointController s)
    {
        SpawnPoints.Add(s);
    }

    public void AddInventory(ThingInfo i)
    {
        if (PlayerInventory.Contains(i)) return;
        PlayerInventory.Add(i);
        UpdateInvText();
    }

    public void RemoveInventory(ThingInfo i)
    {
        int n = PlayerInventory.IndexOf(i);
        PlayerInventory.Remove(i);
        if (InventoryIndex - 1 == n) InventoryIndex--;
        UpdateInvText();
    }

    public void UpdateInvText()
    {
        string txt = "";
        int n = 1;
        foreach (ThingInfo i in PlayerInventory)
        {
            if (txt != "") txt += "\n";
            string l = n + ": " + i.Name;
            if (n == InventoryIndex) l = "<b>" + l + "</b>";
            txt += l;
            if (n == 0) break;
            n++;
            if (n > 9) n = 0;
        }
        InvTxt.text = txt;
    }
}
