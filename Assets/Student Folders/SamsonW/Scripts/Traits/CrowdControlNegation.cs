using UnityEngine;

/// <summary>
/// Blocks stuns and knockback for a duration.
/// Requires: Duration in seconds (NumInfo.Time)
/// </summary>
public class CrowdControlNegation : Trait
{
    public CrowdControlNegation()
    {
        Type = Traits.CrowdControlNegation;
        AddPreListen(EventTypes.StartAction);
        AddListen(EventTypes.Update);
    }

    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.StartAction:
                // Block stuns
                if (e.Get(ActionInfo.Action) == Actions.Stun)
                {
                    e.Abort = true;
                    if (i.Who != null && i.Who.Thing != null)
                    {
                        i.Who.Thing.ActualMove = Vector2.zero;
                        i.Who.Thing.Knockback = Vector2.zero;
                    }
                    God.Log($"[CrowdControlNegation] Stun blocked for {i.Who.Thing.gameObject.name}");
                }
                break;
        }
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
                float duration = i.GetFloat(NumInfo.Time, 5f);
                
                // Clear knockback each frame
                if (i.Who != null && i.Who.Thing != null)
                    i.Who.Thing.Knockback = Vector2.zero;
                
                duration -= Time.deltaTime;
                i.Set(NumInfo.Time, duration);
                
                if (duration <= 0f)
                {
                    God.Log($"[CrowdControlNegation] Immunity expired for {i.Who.Thing.gameObject.name}");
                    i.Who.RemoveTrait(Type);
                }
                break;
        }
    }
}
