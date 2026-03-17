using UnityEngine;

public class Slowed : Trait
{

    //TIMER FOR SLOW EFFECT//
    float timer = 5f;

    //SAVE ORIGINAL SPEED//
    float originalSpeed;
    

    //BOOL TO TRACK IF ORIGINAL SPEED WAS SAVED//
    bool OgSpeedSaved = false;

    public Slowed()
    {
        Type = Traits.Slowed_JuliusP;

        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                //GET THINGINFO OF THE THING THAT OWNS THE TRAIT//
                ThingInfo thing = i.Who;

                // STORE ORIGINAL SPEED ONCE
                if (!OgSpeedSaved)
                {
                    originalSpeed = thing.CurrentSpeed;
                    OgSpeedSaved = true;
                }

                // SLOW THE THING DOWN
                thing.CurrentSpeed = 1.5f;

                // COUNT DOWN TIMER
                timer -= Time.deltaTime;

                // WHEN DONE
                if (timer <= 0)
                {
                    // RESTORE SPEED
                    thing.CurrentSpeed = originalSpeed;

                    Debug.Log("Back to normal");

                    // REMOVE THE SLOW TRAIT
                    thing.RemoveTrait(Traits.Slowed_JuliusP);
                }

                break;
            }
        }
    }
}