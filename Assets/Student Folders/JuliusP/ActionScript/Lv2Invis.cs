using UnityEngine;
using System.Collections;

public class Lv2Invis : ActionScript
{
    private bool chaseStarted = false;

    public Lv2Invis(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Lv2_Cloak_JuliusP, who, true);

        HaltMomentum = false;
        MoveMult = 1f;          // Use normal move speed so AI can navigate properly
        Duration = Mathf.Infinity;
    }

    public override void Begin()
    {
        // Activate invisibility
        Who.AddTrait(Traits.GainInvis_JuliusP);
    }

    public override void OnRun()
    {
        if (Who == null || Who.Thing == null || God.Session.Player == null)
            return;

        ThingInfo player = God.Session.Player;

        // Face the player
        Who.Thing.LookAt(player, 0f);

        // Start the chase only once
        if (!chaseStarted)
        {
            chaseStarted = true;
            Who.DoAction(Actions.Chase); // Let built-in AI navigation handle movement
        }

        // Check if close enough to attack
        float distance = Who.Thing.Distance(player);
        if (distance <= Who.AttackRange)
        {
            // End invisibility and allow attack
            End();
        }
    }

    public override void End()
    {
        // Remove invisibility
       // Who.RemoveTrait(Traits.GainInvis_JuliusP);

        // Automatically attack after becoming visible
        Who.DoAction(Actions.DefaultAttack);
    }

    public override Actions NextAction()
    {
        // After attack, fallback to normal AI behavior
        return Actions.Chase;
    }
}