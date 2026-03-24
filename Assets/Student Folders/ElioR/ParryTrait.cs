using UnityEngine;

public class ParryTrait: Trait
{
    
    public float Speed = 0;
    public ParryTrait()
    {
        Type = Traits.ParryTrait_ElioR;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch(e.Type)
        {
            case EventTypes.Update:
            if(Input.GetKeyDown(KeyCode.P))
                {
                    i.Who.DoAction(Actions.ParryAction_ElioR);
                }
                break;
        }
    }
}
