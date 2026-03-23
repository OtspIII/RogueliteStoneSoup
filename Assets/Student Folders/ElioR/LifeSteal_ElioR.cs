using UnityEngine;

public class LifeSteal_ElioR : Trait
{
    
    public LifeSteal_ElioR()
    {
        Type = Traits.LifeSteal_ElioR;
        AddListen(EventTypes.OnKill);
        AddListen(EventTypes.Setup);
        
    
    }

        public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch(e.Type)
        {
            case EventTypes.Setup:
                {
                    TraitInfo health = i.Who.Get(Traits.Health);
                    break;
                }
            case EventTypes.OnKill:
            {
                i.Who.TakeEvent(God.E(EventTypes.Healing).Set(1));
            break;
            }
        }
    }
}
