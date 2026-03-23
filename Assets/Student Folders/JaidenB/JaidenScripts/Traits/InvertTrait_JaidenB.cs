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
            case EventTypes.Update:
                {
                    Vector2 vel = Vector2.zero;
                    if (Input.GetKey(KeyCode.D)) vel.x = -1;
                    if (Input.GetKey(KeyCode.A)) vel.x = 1;
                    if (Input.GetKey(KeyCode.W)) vel.y = -1;
                    if (Input.GetKey(KeyCode.S)) vel.y = 1;
                    i.Who.DesiredMove = vel;

                    //ThingInfo.DesiredMove(0, 0);
                }
                break;
        }
    }

}
