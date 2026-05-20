using UnityEngine;

/// <summary>
/// Grants crowd control immunity after accumulating enough stuns.
/// Requires: Stun threshold, window duration (X), immunity duration (Y), gap timeout (Z)
/// </summary>
public class ComboBreakTrait : Trait
{
    public ComboBreakTrait()
    {
        Type = Traits.ComboBreak;
        AddListen(EventTypes.StartAction);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.StartAction:
                HandleStunEvent(i, e);
                break;
        }
    }

    private void HandleStunEvent(TraitInfo traitInfo, EventInfo e)
    {
        // Check if stun action
        Actions startedAction = e.GetAction(ActionInfo.Action);
        if (startedAction != Actions.Stun)
            return;

        float immunityDuration = traitInfo.GetFloat(NumInfo.Max, 5f);
        float gapTimeout = traitInfo.GetFloat(NumInfo.Speed, 8f);
        int stunThreshold = (int)traitInfo.GetFloat(NumInfo.Default, 3f);
        bool debugEnabled = traitInfo.GetBool(BoolInfo.Default);

        float currentTime = Time.realtimeSinceStartup;
        int stunCount = (int)traitInfo.GetFloat(NumInfo.Phase, 0f);
        float lastStunTime = traitInfo.GetFloat(NumInfo.Priority, 0f);

        // Check gap or first stun
        if (stunCount == 0 || currentTime - lastStunTime > gapTimeout)
        {
            stunCount = 1;
            if (lastStunTime > 0)
                God.Log(
                    $"[ComboBreak] {traitInfo.Who.Thing.gameObject.name}: Gap of {currentTime - lastStunTime:F1}s > {gapTimeout}s. Combo reset to 1",
                    debugEnabled);
            else
                God.Log($"[ComboBreak] {traitInfo.Who.Thing.gameObject.name}: Stun #1 detected", debugEnabled);
        }
        else
        {
            stunCount++;
            God.Log(
                $"[ComboBreak] {traitInfo.Who.Thing.gameObject.name}: Stun #{stunCount} detected. Gap: {currentTime - lastStunTime:F1}s",
                debugEnabled);
        }

        traitInfo.Set(NumInfo.Priority, currentTime);
        traitInfo.Set(NumInfo.Phase, stunCount);

        if (stunCount >= stunThreshold)
        {
            God.Log(
                $"[ComboBreak] {traitInfo.Who.Thing.gameObject.name}: Threshold {stunThreshold} reached! Granting {immunityDuration}s immunity",
                debugEnabled);
            GrantStunImmunity(traitInfo, immunityDuration);
            traitInfo.Set(NumInfo.Phase, 0);
        }
    }

    private void GrantStunImmunity(TraitInfo traitInfo, float duration)
    {
        // Add crowd control negation trait with duration parameter
        traitInfo.Who.AddTrait(Traits.CrowdControlNegation, God.E(EventTypes.GainTrait).Set(NumInfo.Time, duration));
    }
}