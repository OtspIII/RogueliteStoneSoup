using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelBuilder Level;
    public Image Fader;
    [HideInInspector] public Transform LevelHolder;
    public List<SpawnPointController> SpawnPoints;
    public TextMeshProUGUI HealthTxt;
    public TextMeshProUGUI InvTxt;
    public List<RoomScript> Rooms;
    
    private Dictionary<string, string> UIThings = new Dictionary<string, string>();
    public ThingOption DebugSpawn;
    public int LevelOverride = 0;

    private void Awake()
    {
        God.GM = this;
        Parser.Init();
        // ThingBuilder.Init();
    }

    void Start()
    {
        God.Library.Setup();
        LevelHolder = new GameObject("Level").transform;
        Level = new LevelBuilder();
        Level.Build();
    }

    public void AddSpawn(SpawnPointController s)
    {
        SpawnPoints.Add(s);
    }

    

    public void UpdateInvText()
    {
        string txt = "";
        int n = 1;
        foreach (ThingInfo i in God.Session.PlayerInventory)
        {
            if (txt != "") txt += "\n";
            string l = n + ": " + i.GetName();
            if (n == God.Session.InventoryIndex) l = "<b>" + l + "</b>";
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

    public void PlayerWin()
    {
        StartCoroutine(God.Session.BeatLevel());
    }
    
    public void PlayerLose()
    {
        StartCoroutine(God.Session.LoseLevel());
    }

    public Coroutine Fade(bool fadeOut=true)
    {
        return StartCoroutine(fade(fadeOut));
    }

    private IEnumerator fade(bool fadeOut=true)
    {
        yield return God.Fade(Fader);
    }
}
