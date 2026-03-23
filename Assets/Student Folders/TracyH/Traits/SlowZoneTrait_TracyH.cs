using UnityEngine;

//Note:
//Time : duration of slowness applied on touch
//Default : speed multiplier applied to target
public class SlowZoneTrait_TracyH : Trait
{
    public SlowZoneTrait_TracyH()
    {
        Type = Traits.SlowZone_TracyH;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //Apply slow on touch
            case EventTypes.OnTouch:
                {
                    GameCollision col = e.Collision;
                    if (col == null) return;

                    ThingInfo other = col.Other.Info;
                    if (other == null) return;

                    if (other.ActorTrait == null) return;

                    float duration = i.GetFloat(NumInfo.Time, 2f);
                    float mult = i.GetFloat(NumInfo.Default, 0.5f);

                    other.AddTrait(
                        Traits.Slow_TracyH,
                        God.E()
                            .Set(NumInfo.Time, duration)
                            .Set(NumInfo.Default, mult)
                    );
                    break;
                }
        }
    }
}
