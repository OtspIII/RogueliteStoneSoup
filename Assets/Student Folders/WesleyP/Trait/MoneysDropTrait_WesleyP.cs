using UnityEngine;

public class MoneysDrop_WesleyP: Trait
{
    public MoneysDrop_WesleyP() 
    {
        Type = Traits.MoneysDrop_WesleyP1;
        AddListen(EventTypes.Death);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) 
    {
        switch (e.Type) 
        {
            case EventTypes.Death:
                {

                    Debug.Log("test");
                }
                break;
        }
    }

}
