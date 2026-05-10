using UnityEngine;

public class HeavyKnockback : Trait
{
    public HeavyKnockback()
    {
        Type = Traits.ApplyHeavyKnockBack_JuliusP;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnTouch:
            {
                GameCollision col = e.Collision;
                if (col == null) return;

                ThingInfo target = col.Other.Info;
                if (target == null) return;

                ThingInfo projectile = i.Who;

                // 🔥 GET THE REAL SOURCE (PLAYER WHO FIRED)
                ThingInfo owner = projectile.GetOwner();

                // PUSH TARGET AWAY FROM OWNER
                target.Thing.TakeKnockback(owner, 30f);

                break;
            }
        }
    }
}