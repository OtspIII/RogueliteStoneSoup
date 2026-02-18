using UnityEngine;

public class BarrierTrait_ElioR : Trait
{
    
    public BarrierTrait_ElioR()
    {
        Type = Traits.Barrier;
        AddPreListen(EventTypes.Damage);
        AddListen(EventTypes.Damage);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch(e.Type)
        {
           case EventTypes.Damage:
                {
                    break;
                }
        }
    }

    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        switch(e.Type)
        {
            case EventTypes.Damage:
            {
                e.Abort = true;
                
                break;
            }
        }
    }
}
