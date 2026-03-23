using UnityEngine;

public class FreezeTrait : Trait
{
    public FreezeTrait() 
    {
        Type = Traits.Freeze;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) 
    {
        switch (e.Type) 
        {
            case EventTypes.OnTouch:
                {
                   

                }
                break;
        }
    }

}
