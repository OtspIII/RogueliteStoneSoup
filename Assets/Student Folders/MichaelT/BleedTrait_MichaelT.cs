using UnityEngine;

public class BleedTrait_MichaelT : Trait
{
    private float timer;
    public BleedTrait_MichaelT()
    {
        Type = Traits.Bleed_MichaelT;

        AddListen(EventTypes.Setup);
        AddListen(EventTypes.Update);
        
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                //Intialize when timer starts 
                timer = 1.0f;
                break; 
            }
           
            case EventTypes.Update:
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    //Damage the player and reset the timer
                    i.Who.TakeEvent(God.E(EventTypes.Damage).Set(1));
                    timer = 1.0f;
                }
                break;
            }
        }
    }

}
