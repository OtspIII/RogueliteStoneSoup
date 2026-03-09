using UnityEngine;

public class GrappleHook : Trait
{
    public GrappleHook()
    {
       // Type = Traits.GrappleHook;
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.OnDestroy);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
            {
                ThingInfo grapplerThingInfo = e.GetThing(ThingEInfo.Source);
                i.SetThing(ThingEInfo.Source, grapplerThingInfo); // Store the grappler source thing
                break;
            }
            case EventTypes.OnDestroy:
            {
                ThingInfo grapplerThingInfo = i.GetThing(ThingEInfo.Source);

                EventInfo changeActionEvent = God.E(EventTypes.StartAction);
               // changeActionEvent.SetAction(ActionInfo.Action, Actions.Grapple);
                changeActionEvent.SetInt(NumInfo.Priority, 100); // High priority so thing is forced into action
                changeActionEvent.SetFloat(NumInfo.Time, i.GetFloat(NumInfo.Time)); // Set grapple duration
                changeActionEvent.SetVector(VectorInfo.Position, e.GetVector()); // Set target hook position
                grapplerThingInfo.TakeEvent(changeActionEvent);
                break;
            }
        }
    }
}