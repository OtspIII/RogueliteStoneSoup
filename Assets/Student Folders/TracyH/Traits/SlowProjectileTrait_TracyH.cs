using UnityEngine;

//Note:
//Time : duration of slowness applied on hit
//Default : speed multiplier applied to target
public class SlowProjectileTrait_TracyH : Trait
{
    public SlowProjectileTrait_TracyH()
    {
        Type = Traits.SlowProjectile_TracyH;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //Apply slow on hit
            case EventTypes.OnTouch:
                {
                    GameCollision col = e.Collision;
                    if (col == null) return;

                    if (col.HBOther.Type != HitboxTypes.Body && col.HBOther.Coll.isTrigger) return;

                    ThingInfo other = col.Other.Info;
                    if (other == null) return;

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