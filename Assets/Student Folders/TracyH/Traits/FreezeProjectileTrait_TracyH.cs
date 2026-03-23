using UnityEngine;

//Note:
//Time : duration of freeze applied on hit
public class FreezeProjectileTrait_TracyH : Trait
{
    public FreezeProjectileTrait_TracyH()
    {
        Type = Traits.FreezeProjectile_TracyH;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //Apply freeze on hit
            case EventTypes.OnTouch:
                {
                    GameCollision col = e.Collision;
                    if (col == null) return;

                    if (col.HBOther.Type != HitboxTypes.Body && col.HBOther.Coll.isTrigger) return;

                    ThingInfo other = col.Other.Info;
                    if (other == null) return;

                    float freezeTime = i.GetFloat(NumInfo.Time, 2f);

                    other.AddTrait(
                        Traits.Freeze_TracyH,
                        God.E().Set(NumInfo.Time, freezeTime)
                    );

                    break;
                }
        }
    }
}