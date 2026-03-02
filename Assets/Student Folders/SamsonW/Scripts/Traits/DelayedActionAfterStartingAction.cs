using UnityEngine;

/// <summary>
/// Requires:
/// Number for X secs to delay
/// Source Action for enter action to listen for
/// Target Action for target action to switch to
/// </summary>
public class DelayedActionAfterStartingAction : Trait
{
    private float delayDuration;
    private float delayTimer;
    private bool isTimerStarted;
    private Actions targetAction;
    private ThingInfo targetThingInfo;
    
    public DelayedActionAfterStartingAction()
    {
        Type = Traits.DelayedActionAfterStartingAction;
        AddListen(EventTypes.StartAction);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.StartAction:
                // Try get delay first
                delayDuration = i.GetFloat();
                if (delayDuration < 0)
                {
                    God.LogError($"{GetType()} on {i.Who.Thing.gameObject.name} requires valid delay number");
                    return;
                }
                
                Actions sourceAction = i.GetAction(ActionInfo.Source);
                targetAction = i.GetAction(ActionInfo.Target);

                // See if the action started matches our source
                Actions startedAction = e.GetAction(ActionInfo.Action);
                if (startedAction != sourceAction)
                {
                    God.Log($"Detected {startedAction}, but does not match {sourceAction}");
                    return;
                }
                
                // Switch after validating inputs
                isTimerStarted = true;
                delayTimer = 0f;

                targetThingInfo = i.Who;
                
                God.Log($"Detected {sourceAction}. Switching to {targetAction} after {delayDuration} seconds.");
                break;
            case EventTypes.Update:
                if (!isTimerStarted)
                    return;

                delayTimer += Time.deltaTime;
                if (delayTimer >= delayDuration)
                {
                    isTimerStarted = false;
                    OnDelayFinished();
                }
                break;
        }
    }

    private void OnDelayFinished()
    {
        God.Log($"Delay is over, attempting to switch to {targetAction}");
        targetThingInfo.TakeEvent(God.E(EventTypes.StartAction).Set(ActionInfo.Action, targetAction));
    }
}