using Unity.VisualScripting;
using UnityEngine;

public class BarrierTrait_ElioR : Trait
{
    
    public BarrierTrait_ElioR()
    {
        Type = Traits.Barrier;
        AddPreListen(EventTypes.Damage);
        AddListen(EventTypes.Damage);
        AddListen(EventTypes.Setup);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch(e.Type)
        {
            case EventTypes.Setup:
                {
                    Debug.Log("SETUP BARRIER");
                    break;
                }
           case EventTypes.Damage:
                {
                    Debug.Log("XXX");
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
                Debug.Log("Success");
                e.Abort = true;
                i.Who.RemoveTrait(i.Trait);
                
                break;
            }
        }
    }
}
