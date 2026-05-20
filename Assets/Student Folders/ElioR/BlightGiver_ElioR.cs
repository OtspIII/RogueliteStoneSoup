using UnityEngine;

public class BlightGiver_ElioR:Trait
{
    public float cd = 5;
    public BlightGiver_ElioR()
    {
        Type = Traits.BlightGiver_ElioR;
        AddListen(EventTypes.OnTouch);
        AddListen(EventTypes.Update);
        AddListen(EventTypes.OnKill);
         
        
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
       switch(e.Type)
        {
            case EventTypes.OnTouch:
                {
                    //ThingInfo targ = e.GetThing();
                   // targ.AddTrait(Traits.Blight_ElioR);
                    break;
                }
                case EventTypes.Update:
                {
                    cd -= Time.deltaTime;
                    if(cd <= 0.01f){
                    i.Who.TakeEvent(God.E(EventTypes.Damage).Set(2f));
                    cd = 5;
                    } 
                   // Debug.Log(cd);
                    break;
                }

                case EventTypes.OnKill:
            {
                i.Who.TakeEvent(God.E(EventTypes.Healing).Set(20));
            break;
            }
        }
    }

    
}
