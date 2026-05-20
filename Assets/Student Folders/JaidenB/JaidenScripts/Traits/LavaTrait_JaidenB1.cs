using UnityEngine;

public class LavaTrait : Trait
{
    public LavaTrait() 
    {
        Type = Traits.Freeze;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) 
    {
        bool LavaFull = false;

        switch (e.Type) 
        {
            case EventTypes.Update:
                {
                   if (LavaFull == false)
                    {
                        Debug.Log("Hello lava is rising");
                    }
                    Debug.Log("ing");
                }
                break;
        }
    }

}
