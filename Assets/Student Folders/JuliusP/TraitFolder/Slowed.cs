using UnityEngine;

public class Slowed : Trait
{
    float timer;
    float originalSpeed;

    bool applied;
    bool hasOriginal;

    public Slowed()
    {
        Type = Traits.Slowed_JuliusP;

        AddListen(EventTypes.Setup);
        AddListen(EventTypes.Update);
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        ThingInfo thing = i.Who;
        if (thing == null || thing.Thing == null) return;

        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                applied = false;
                hasOriginal = false;
                timer = 0f;
                break;
            }

            case EventTypes.OnTouch:
            {
            
                if (applied) return;

                originalSpeed = thing.CurrentSpeed;
                hasOriginal = true;

                thing.CurrentSpeed = 0f;

                timer = 1.9f;
                applied = true;

                break;
            }

            case EventTypes.Update:
            {
                if (!applied) return;

                timer -= Time.deltaTime;

                if (timer <= 0f)
                {
                    if (hasOriginal)
                        thing.CurrentSpeed = originalSpeed;

                    applied = false;
                    hasOriginal = false;

                    thing.RemoveTrait(Traits.Slowed_JuliusP);
                }

                break;
            }
        }
    }
}