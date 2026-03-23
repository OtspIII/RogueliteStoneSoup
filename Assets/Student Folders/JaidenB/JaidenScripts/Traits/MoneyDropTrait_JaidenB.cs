using UnityEngine;

public class MoneyDropTrait : Trait
{
    public MoneyDropTrait() 
    {
        Type = Traits.MoneyDrop;
        AddListen(EventTypes.Death);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) 
    {
        switch (e.Type) 
        {
            case EventTypes.Death:
                {

                    Debug.Log("idk anymore man");
                }
                break;
        }
    }

}
