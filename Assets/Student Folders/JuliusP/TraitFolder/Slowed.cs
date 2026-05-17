using UnityEngine;

public class Slowed : Trait
{
    float timer;
    bool applied;

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
                timer = 0f;
                break;
            }

            case EventTypes.OnTouch:
            {
                applied = true;
                timer = 1.9f;
                break;
            }

            case EventTypes.Update:
            {
                if (!applied) return;

                timer -= Time.deltaTime;

                // 🔥 SAFE SLOW: only modify behavior, not core speed
                thing.DesiredMove = Vector2.zero;

                if (timer <= 0f)
                {
                    applied = false;
                    thing.RemoveTrait(Traits.Slowed_JuliusP);
                }

                break;
            }
        }
    }
}