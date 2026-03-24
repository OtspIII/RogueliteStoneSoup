using UnityEngine;

public class InvertControlsTrait : Trait
{
    public InvertControlsTrait() 
    {
        Type = Traits.InvertControls;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) 
    {
        switch (e.Type) 
        {
            case EventTypes.OnTouch:
                {
                   
                    // Vector2 vel = Vector2.zero;
                    // i.vel.x = -1;
                    // i.vel.x = 1;
                    // i.vel.y = -1;
                    // i.vel.y = 1;

                    //ThingInfo.DesiredMove(0, 0);
                }
                break;
        }
    }

}
