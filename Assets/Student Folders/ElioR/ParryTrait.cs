using UnityEngine;

public class ParryTrait: Trait
{
    public bool CanParry = true;
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
            if(CanParry && Input.GetKeyDown(KeyCode.P))
                {
                    ThingController.DoAction(Actions.ParryAction_ElioR, e);
                }
                break;
        }
    }
}
