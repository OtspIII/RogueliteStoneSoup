using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletHell_qixiangdong : Trait
{
    public float SurvivalTime = 20f;
    public BulletHell_qixiangdong()
    {
        Type = Traits.BulletHell_qixiangdong;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.Update);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
                i.Set(NumInfo.Max, SurvivalTime);
                i.Set(false);
                break;
            case EventTypes.OnSpawn:
                int level = God.Session.Level;
                float survivalTime = SurvivalTime + level * 5f;
                i.Set(NumInfo.Max, survivalTime);
                break;
            case EventTypes.Update:
                if (i.GetBool()) return;
                float remaining = i.Get(NumInfo.Max) - Time.deltaTime;
                i.Set(NumInfo.Max, remaining);
                God.GM.SetUI("bullethell_timer",
                    "Survive for " + Mathf.CeilToInt(remaining) + "s", 1);
                if (remaining <= 0)
                {
                    i.Set(true);
                    God.GM.SetUI("bullethell_timer", "", 1);
                    
                    GoToNextLevel();
                }
                break;
        }
    }
    
    void GoToNextLevel()
    {
        God.Session.Level++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
