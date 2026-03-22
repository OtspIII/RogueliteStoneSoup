using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Requires:
/// Number for X secs to delay
/// Source Action for enter action to listen for
/// Target Action for target action to switch to
/// </summary>
public class DelayedActionAfterStartingAction : Trait
{
    public class DelayedActionData
    {
        public float duration;
        public float timer;
        public Actions targetAction;
    }

    private Dictionary<ThingInfo, DelayedActionData> delayedActionDict = new();
    
    public DelayedActionAfterStartingAction()
    {
        Type = Traits.DelayedActionAfterStartingAction;
        AddListen(EventTypes.StartAction);
        AddListen(EventTypes.Update);
        
        //Debug.Log($"Instance of {GetType().Name} created");
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.StartAction:
                // Try get delay first
                float delayDuration = i.GetFloat();
                if (delayDuration < 0)
                {
                    Debug.LogError($"{GetType()} on {i.Who.Thing.gameObject.name} requires valid delay number");
                    return;
                }
                
                Actions sourceAction = i.GetAction(ActionInfo.Source);
                Actions targetAction = i.GetAction(ActionInfo.Target);

                // See if the action started matches our source
                Actions startedAction = e.GetAction(ActionInfo.Action);
                if (startedAction != sourceAction)
                {
                    // Debug.Log($"Detected {startedAction}, but does not match {sourceAction}");
                    return;
                }

                // Can't interrupt existing
                if (delayedActionDict.ContainsKey(i.Who))
                    return;

                DelayedActionData newDelayedActionData = new();
                newDelayedActionData.duration = delayDuration;
                newDelayedActionData.timer = 0f;
                newDelayedActionData.targetAction = targetAction;
                
                delayedActionDict.Add(i.Who, newDelayedActionData);
                
                // Debug.Log($"Detected {sourceAction} on {i.Who.Thing.gameObject.name}. Switching to {targetAction} after {delayDuration} seconds.");
                break;
            case EventTypes.Update:
                if (delayedActionDict == null)
                    return;

                if (delayedActionDict.Count == 0)
                    return;

                // Loop through copy since we are modifying collection inside loop
                foreach (var kvp in new Dictionary<ThingInfo, DelayedActionData>(delayedActionDict))
                {
                    ThingInfo who = kvp.Key;
                    DelayedActionData delayedActionData = kvp.Value;

                    if (who.Thing == null)
                    {
                        delayedActionDict.Remove(who);
                        continue;
                    }
                    
                    delayedActionData.timer += Time.deltaTime;
                    if (delayedActionData.timer >= delayedActionData.duration)
                    {
                        Debug.Log($"{GetType()}: Delay is over for {who.Thing.gameObject.name}, attempting to switch to {delayedActionData.targetAction}");
                        who.TakeEvent(God.E(EventTypes.StartAction).Set(ActionInfo.Action, delayedActionData.targetAction));
                        delayedActionDict.Remove(who);
                    }
                }
                break;
        }
    }
}