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

             ThingController target = col.Other;
            
             if (target == null) return;

             if (target.Info == i.Who) return;

              target.TakeKnockback(i.Who, 17f);
                
                break;
            }
        }
    }
}