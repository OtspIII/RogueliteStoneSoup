using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession
{
    public int Level = 1;
    public int MaxLevel = 10;
    public ThingInfo Player;
    public bool Victory = false;
    public bool Defeat = false;

    public virtual void Progress(EventInfo e)
    {
        Level++;
        if (Level >= MaxLevel) Victory = true;
        God.GM.PlayerWin();
    }

    public virtual void PlayerDeath(EventInfo e)
    {
        Defeat = true;
        God.GM.PlayerLose();
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
}
