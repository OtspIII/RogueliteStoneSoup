using UnityEngine;

public class GrappleHookTrait : Trait
{
    public GrappleHookTrait()
    {
        Type = Traits.GrappleHook;
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
                if (grapplerThingInfo == null)
                {
                    Debug.LogWarning("Grapple fail, shooter is null");
                    return;
                }
                i.SetThing(ThingEInfo.Source, grapplerThingInfo); // Store the grappler source thing
                break;
            }
            case EventTypes.OnDestroy:
            {
                ThingInfo grapplerThingInfo = i.GetThing(ThingEInfo.Source);
                if (grapplerThingInfo == null)
                {
                    Debug.LogWarning("Grapple fail, shooter is null");
                    return;
                }

                float grappleDuration = i.GetFloat(NumInfo.Time);
                if (grappleDuration <= 0)
                {
                    Debug.LogWarning("Grapple fail, duration is invalid");
                    return;
                }
                
                EventInfo changeActionEvent = God.E(EventTypes.StartAction);
                changeActionEvent.SetAction(ActionInfo.Action, Actions.Grapple);
                changeActionEvent.SetInt(NumInfo.Priority, 100); // High priority so thing is forced into action
                changeActionEvent.SetFloat(NumInfo.Time, grappleDuration); // Set grapple duration
                changeActionEvent.SetVector(VectorInfo.Position, e.GetVector()); // Set target hook position
                grapplerThingInfo.TakeEvent(changeActionEvent);
                break;
            }
        }
    }
}