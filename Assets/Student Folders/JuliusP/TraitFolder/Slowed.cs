using UnityEngine;

public class Slowed : Trait
{
    float timer = 5f;
    float originalSpeed;
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
                ThingInfo thing = i.Who;
                if (thing == null || thing.Thing == null) return;

                // SAVE ORIGINAL SPEED IMMEDIATELY
                if (!OgSpeedSaved)
                {
                    originalSpeed = thing.CurrentSpeed; // save original speed before slowing
                    OgSpeedSaved = true;
                }

                // APPLY SLOW
                thing.CurrentSpeed = 0.5f;

                // COUNTDOWN
                timer -= Time.deltaTime;

                // RESTORE SPEED WHEN DONE
                if (timer <= 0)
                {
                    thing.CurrentSpeed = originalSpeed;
                    Debug.Log("Back to normal");

                    // REMOVE THIS TRAIT
                    thing.RemoveTrait(Traits.Slowed_JuliusP);
                }

                break;
            }
        }
    }
}
    
