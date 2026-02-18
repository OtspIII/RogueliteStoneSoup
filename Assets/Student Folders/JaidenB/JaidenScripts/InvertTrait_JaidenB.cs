using UnityEngine;

public class InvertControlsTrait : Trait
{
    public InvertControlsTrait() 
    {
        Type = Traits.InvertControls;
        AddListen(EventTypes.OnPickup);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) 
    {
        switch (e.Type) 
        {
            case EventTypes.OnPickup:
                {
                    Debug.Log("Invert test");
                }
        }
    }

}
