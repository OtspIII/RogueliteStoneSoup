using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelBuilder Level;
    [HideInInspector] public Transform LevelHolder;
    public List<SpawnPointController> SpawnPoints;
    public TextMeshProUGUI HealthTxt;
    public TextMeshProUGUI InvTxt;
    public List<RoomScript> Rooms;
    [HideInInspector] public List<ThingInfo> PlayerInventory;
    [HideInInspector] public int InventoryIndex = 1;
    private Dictionary<string, string> UIThings = new Dictionary<string, string>();

    private void Awake()
    {
        God.GM = this;
        TraitManager.Init();
        ThingBuilder.Init();
    }

    void Start()
    {
        God.Library.Setup();
        BuildLevel();
    }

    void Update()
    {
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

    public void SetUI(string type,string text,int priority=3)
    {
        if (priority > 9 || priority < 1)
        {
            Debug.Log("WARNING: Added UI with a non-single-digit priority. May cause visual bugs.\n"+text);
        }
        string t = priority + text;
        if (text == "")
        {
            UIThings.Remove(type);
            return;
        }
        if (!UIThings.TryAdd(type, t)) UIThings[type] = t;
        DrawUI();
    }

    public void DrawUI()
    {
        List<string> uis = UIThings.Values.ToList();
        uis.Sort();
        string r = "";
        foreach (string s in uis)
        {
            if (r != "") r += "\n";
            r += s.Substring(1);
        }
        HealthTxt.text = r;
    }
}
