using UnityEngine;

//Note:
//Time : how long slowness lasts
//Default : speed multiplier applied while active
public class SlowTrait_TracyH : Trait
{
    public SlowTrait_TracyH()
    {
        Type = Traits.Slow_TracyH;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //Slow effect
            case EventTypes.Update:
                {
                    float baseSpeed = 8f;

                    if (i.Who.ActorTrait != null)
                        baseSpeed = i.Who.ActorTrait.GetFloat(NumInfo.Speed, 8f);

                    float mult = i.GetFloat(NumInfo.Default, 0.5f);
                    i.Who.CurrentSpeed = baseSpeed * mult;

                    float timeLeft = i.GetFloat(NumInfo.Time, 0f);
                    timeLeft -= Time.deltaTime;
                    i.Set(NumInfo.Time, timeLeft);

                    if (timeLeft <= 0f)
                    {
                        i.Who.RemoveTrait(Type);
                    }
                    break;
                }
        }
    }

    protected override void OnRemove(TraitInfo i, EventInfo e)
    {
        float baseSpeed = 8f;

        if (i.Who.ActorTrait != null)
            baseSpeed = i.Who.ActorTrait.GetFloat(NumInfo.Speed, 8f);

        i.Who.CurrentSpeed = baseSpeed;
    }

    public override void ReUp(TraitInfo old, EventInfo n)
    {
        if (n == null) return;

        old.Set(NumInfo.Time, n.GetFloat(NumInfo.Time, old.GetFloat(NumInfo.Time, 0f)));
        old.Set(NumInfo.Default, n.GetFloat(NumInfo.Default, old.GetFloat(NumInfo.Default, 0.5f)));
    }
}