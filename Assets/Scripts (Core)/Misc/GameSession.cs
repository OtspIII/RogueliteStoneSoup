using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession
{
    public Authors Author;
    public int Level = 1;
    public int MaxLevel = 10;
    public ThingInfo Player;
    public bool Victory = false;
    public bool Defeat = false;
    public List<ThingInfo> PlayerInventory = new List<ThingInfo>();
    public int InventoryIndex = 1;

    public GameSession(Authors a)
    {
        //If the author isn't set, auto-set it. This won't work unless you go to Parser and add the code for it
        if (a == Authors.None) 
            a = Parser.FindAuthor();
        //If that didn't work or if I want it random, make it a random one 
        if (a == Authors.None || a == Authors.Random)
        {
            a = Parser.AllAuthors[Random.Range(0,Parser.AllAuthors.Count)];
        }
        Author = a;
        if (God.GM != null) 
        {
            God.GM.CurrentAuthor = a;
            if(God.GM.LevelOverride > 0) Level = God.GM.LevelOverride;
        }
    }
    
    public virtual void Progress(EventInfo e)
    {
        Level++;
        if (Level >= MaxLevel) Victory = true;
        God.C(BeatLevel());
    }

    public virtual void PlayerDeath(EventInfo e)
    {
        Defeat = true;
        God.C(LoseLevel());
    }

    public virtual IEnumerator BeatLevel()
    {
        yield return (God.GM.Fade());
        SceneManager.LoadScene(Victory ? "YouWin" : "Gameplay");
    }
    
    public virtual IEnumerator LoseLevel()
    {
        yield return (God.GM.Fade());
        SceneManager.LoadScene("YouLose");
    }
    
    public void AddInventory(ThingInfo i)
    {
        if (PlayerInventory.Contains(i)) return;
        PlayerInventory.Add(i);
        God.GM?.UpdateInvText();
    }

    public void RemoveInventory(ThingInfo i)
    {
        int n = PlayerInventory.IndexOf(i);
        PlayerInventory.Remove(i);
        if (InventoryIndex - 1 == n) InventoryIndex--;
        God.GM?.UpdateInvText();
    }
}
