using UnityEngine;

public class AddTraitByScore : Trait
{
    public AddTraitByScore()
    {
        Type = Traits.AddTraitByScore_JuliusP;
        AddListen(EventTypes.AddScore);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.AddScore:
            {
                float score = Mathf.Floor(i.Change(e.GetN()));

                God.GM.SetUI("Score", "Score: " + score, 2);

                //FOR EVERY 10 SCORE, INCREASE HEALTH BY 1//
                if (score % 10 == 0)
                {
                 //   Debug.Log("Score reached 3 → +1 max HP and full heal");

                    // GET HEALTH TRAIT//
                    TraitInfo health = i.Who.Get(Traits.Health);

                    if (health != null)
                    {
                        // READ CURRENT VALUES
                        float current = health.GetN();
                        float max = health.Get(NumInfo.Max);

                        // INCREASE MAX HP//
                        health.Set(NumInfo.Max, max + 1);

                        // INCREASE CURRENT HP//
                        health.Set(current + 1);

                       // Debug.Log("HP upgraded to full 6/6");
                    }

                
                    EventInfo ph = i.Who.Ask(EventTypes.ShownHP);

                    God.GM.SetUI("Health", ph.GetInt() + "/" + ph.GetInt(NumInfo.Max), 1);
                }

                break;
            }
        }
    }
}