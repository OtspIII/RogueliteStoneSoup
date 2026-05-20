using UnityEngine;
using System.Collections;

public class GiveItemv1 : ActionScript
{
    bool canFollow = false;
    bool isEnded = false;

    private ThingInfo itemToGive;

    float originalPlayerSpeed;

    Level_JuliusP LJP;

    ThingInfo ExitRef;

    public GiveItemv1(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.GiveItem_JuliusP, who, true);

        HaltMomentum = false;
        MoveMult = 0.2f;
        Duration = Mathf.Infinity;
    }

    public override void Begin()
    {
        base.Begin();

        ThingInfo player = God.Session.Player;

        ExitRef = God.GM != null ? God.GM.Exit : null;

        if (player == null || player.Thing == null)
        {
            Complete();
            return;
        }

        itemToGive = Who.CurrentHeld;

        Who.AddTrait(Traits.IgnoreDamage_JuliusP);
        Who.AddTrait(Traits.NoTimerStunNegation_JuliusP);

        if (ExitRef != null)
            ExitRef.RemoveTrait(Traits.Exit);

        LJP = God.LB as Level_JuliusP;

        God.C(GiveItemLogic());
    }

    public override void OnRun()
    {
        if (isEnded)
            return;

        if (canFollow && Who != null && Who.Thing != null && God.Session.Player != null && God.Session.Player.Thing != null)
        {
            ThingController p = God.Session.Player.Thing;

            Vector2 offset = -(Vector2)p.transform.up * 1.0f;

            Who.Thing.transform.position = (Vector2)p.transform.position + offset;

            Who.Thing.transform.rotation = p.transform.rotation;
        }

        if (Who != null &&
            !Who.Has(Traits.IgnoreDamage_JuliusP))
        {
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);
        }
    }

    private IEnumerator GiveItemLogic()
    {
        ThingInfo player = God.Session.Player;
        ThingInfo exit = ExitRef;

        while (!canFollow)
        {
            if (isEnded)
                yield break;

            if (Who == null ||Who.Thing == null ||player == null || player.Thing == null)
            {
                yield return null;
                continue;
            }

            if (Who.Thing.Distance(player.Thing) < 1.3f)
            {
                float spd = player.CurrentSpeed;

                player.CurrentSpeed = 0f;

                God.GM.SetUI("TradeMessage", "Can you help me reach the exit?", 2);

                yield return new WaitForSeconds(1.5f);

                if (isEnded)
                    yield break;

                if (player != null)
                    player.CurrentSpeed = spd;

                God.GM.SetUI("TradeMessage", null, 2);

                canFollow = true;

                if (LJP != null)
                    LJP.CanLinkToLootRoom = true;
            }

            yield return null;
        }

        while (true)
        {
            if (isEnded)
                yield break;

            if (Who == null ||Who.Thing == null ||player == null || player.Thing == null || exit == null || exit.Thing == null)
            {
                yield return null;
                continue;
            }

            if (player.Thing.Distance(exit.Thing) < 2.6f)
                break;

            yield return null;
        }

        if (isEnded)
            yield break;

        if (Who == null ||Who.Thing == null ||player == null ||player.Thing == null)
        {
            Complete();
            yield break;
        }

        originalPlayerSpeed = player.CurrentSpeed;

        player.CurrentSpeed = 0f;

        ThingController whoThing = Who != null ? Who.Thing : null;

        ThingController playerThing = player != null ? player.Thing : null;

        if (whoThing != null && playerThing != null)
        {
            whoThing.LookAt(playerThing, 0f);
        }

        God.GM.SetUI("TradeMessage","Thank you... please take this!", 2);

        yield return new WaitForSeconds(1.2f);

        if (isEnded)
            yield break;

        if (itemToGive != null &&
            Who != null)
        {
            Who.DropHeld(false);
        }

        yield return new WaitForSeconds(0.5f);

        if (isEnded)
            yield break;

        God.GM.SetUI("TradeMessage", null, 2);

        if (player != null)
            player.CurrentSpeed = originalPlayerSpeed;

        if (Who != null &&
            Who.Has(Traits.IgnoreDamage_JuliusP))
        {
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);
        }

        if (Who != null &&
            Who.Has(Traits.NoTimerStunNegation_JuliusP))
        {
            Who.RemoveTrait(Traits.NoTimerStunNegation_JuliusP);
        }

        isEnded = true;

        if (Who != null &&
            Who.Thing != null)
        {
            God.C(DestroyAfterEnd());
        }

        Complete();
    }

    private IEnumerator DestroyAfterEnd()
    {
        yield return new WaitForSeconds(0.5f);

        // RESTORE EXIT AFTER ACTION FULLY ENDS
        if (ExitRef != null &&
            !ExitRef.Has(Traits.Exit))
        {
            ExitRef.AddTrait(Traits.Exit);

            Debug.Log("Exit trait restored");
        }

       yield return new WaitForSeconds(0.5f);

        if (Who != null)
            Who.Destruct(Who);
    }

    public override void End()
    {
        isEnded = true;

        base.End();
    }

    public override Actions NextAction()
    {
        return Actions.None;
    }
}