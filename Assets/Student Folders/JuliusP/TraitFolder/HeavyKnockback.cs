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

               
                if (target == i.Who) return;

                // CAUSE KNOCKBACK AWAY FROM TARGET//
                target.Thing.TakeKnockback(i.Who, 17f);

                break;
            }
        }
    }
}