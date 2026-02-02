using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

//This is the script that kicks off the whole game!
//It's also in charge of any UI elements the game has
public class GameManager : MonoBehaviour
{
    [Header("Debug Elements")]
    //Whose game are we going to play? Picks randomly in GameSession if left blank.
    public Authors CurrentAuthor;
    //If you put an Option in here, two of them will spawn in the player's starting room. For testing.
    public ThingOption DebugSpawn;
    //What level does the game start on? If not 0, skips straight to the selected level.
    public int LevelOverride = 0;
    
    [Header("UI Elements")]
    //For fading in and out of blackness at level start and end
    public Image Fader;
    //Left-side text display. Shows health and score, but can show more.
    public TextMeshProUGUI InfoTxt;
    //Right-side text display. Shows what inventory items the player has.
    public TextMeshProUGUI InvTxt;
    
    //We're going to spawn all the rooms as children of this empty GameObject
    [HideInInspector] public Transform LevelHolder;
    //List of all the rooms/things/etc we've spawned, just in case we need it
    [HideInInspector] public List<RoomScript> Rooms;
    [HideInInspector] public List<ThingController> Things;
    [HideInInspector] public List<SfXGnome> Gnomes;
    //Keeps track of the text that InfoTxt needs to display. See SetUI().
    private Dictionary<string, UIText> UIInfo = new Dictionary<string, UIText>();

    //All the code that needs to run before the level build process starts
    private void Awake()
    {
        //Register our static variable so we're easy to find
        God.GM = this;
        //Parser is the script that connects our enums to our custom classes. Tell it to wake up and get set up.
        Parser.Init();
    }

    //Actually build the level
    void Start()
    {
        //If this is the first level, create a new GameSession to track our progress
        if(God.Session == null) God.Session = new GameSession(CurrentAuthor);
        //Spawn an empty game object to hold all our rooms.
        LevelHolder = new GameObject("Level").transform;
        //Create a new LevelBuilder and tell it to build a level for us
        God.LB = Parser.GetLB(God.Session.Author);
        God.LB.Build();
    }
    
    ///Fades the screen in or out. Just a wrapper for a coroutine
    public Coroutine Fade(bool fadeOut=true)
    {
        return StartCoroutine(fade(fadeOut));
    }
    private IEnumerator fade(bool fadeOut=true)
    {
        yield return God.Fade(Fader,fadeOut);
    }
    
    ///Redraws the player inventory UI text. Things get added/removed via the PickupableTrait and GameSession script
    public void UpdateInvText()
    {
        //Make a string that we'll feed into the inventory UI TextMesh
        string txt = "";
        //Each inventory slot has a number, starting with 1
        int n = 1;
        //Loop through each inventory item
        foreach (ThingInfo i in God.Session.PlayerInventory)
        {
            //This line of text is going to be the slot number followed by the item's name
            string l = n + ": " + i.GetName();
            //If this is the item currently held by the player, bold it
            if (n == God.Session.InventoryIndex) l = "<b>" + l + "</b>";
            //If this isn't the first line of text, add a line break (that's what \n does)
            if (txt != "") txt += "\n";
            //Add this line to the text
            txt += l;
            //Annoyingly, the keyboard starts at 1 and ends with 0. If this was #0, we're done--no slots left. End the loop.
            if (n == 0) break;
            //Make the slot number go up
            n++;
            //And if we're at slot 10, change it to be 0 (because that's how keyboards are set up)
            if (n > 9) n = 0;
        }
        //Plug the text we wrote into the TextMesh
        InvTxt.text = txt;
    }

    ///Call this to update the player's left-screen UI text. By default, this is just Health and Score
    ///Type is the UI's key--if you call this again with the same type it'll override the old value
    /// Priority is how high up it goes on the screen. Lower is higher on the screen
    public void SetUI(string key, string text, int priority = 3)
    {
        SetUI(new UIText(key,text,priority));
    }
    public void SetUI(UIText t)
    {
        //If we ever submit a blank text, just delete what was there before
        if (t.Text == "")
        {
            UIInfo.Remove(t.Key);
            return;
        }
        //Add it to the dictionary and redraw the UI
        if (!UIInfo.TryAdd(t.Key, t)) UIInfo[t.Key] = t;
        DrawUI();
    }
    

    public void DrawUI()
    {
        //Make a list of all the UI elements
        List<UIText> uis = UIInfo.Values.ToList();
        //Sort them in order of priority.
        //Sort is a fun function where you can write a mini-function inside its parameters that dictates exactly how it will sort
        uis.Sort((UIText a, UIText b)=>a.Priority > b.Priority ? 1 : -1);
        //Make a string that'll be what we plug into the TextMesh
        string r = "";
        //Loop through the entries and add them to the string 
        foreach (UIText s in uis)
        {
            if (r != "") r += "\n"; //If this isn't the first one, add a line break
            r += s.Text;
        }
        //Plug it in to the TextMesh
        InfoTxt.text = r;
    }
}

///A little custom data-class for storing UI entries like the player's health or score
public class UIText
{
    public string Key;
    public string Text;
    public float Priority;

    public UIText(string key, string txt, float priority = 3)
    {
        Key = key;
        Text = txt;
        Priority = priority;
    }
}