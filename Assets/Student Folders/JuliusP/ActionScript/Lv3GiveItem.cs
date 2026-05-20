using UnityEngine;
using System.Collections;

public class GiveItem_Lv3 : ActionScript
{
    bool canFollow = false;
    bool isEnded = false;

    private ThingInfo itemToGive;

    float originalPlayerSpeed;

    Level_JuliusP LJP;

    ThingInfo ExitRef;

    bool exitRemoved = false;
    bool followDialoguePlayed = false;

    public GiveItem_Lv3(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.GiveItem_Lv3_JuliusP, who, true);

        HaltMomentum = false;
        MoveMult = 0.2f;
        Duration = Mathf.Infinity;
    }

    public override void Begin()
    {
        base.Begin();

        ThingInfo player = God.Session.Player;

        if (player == null || player.Thing == null)
        {
            Complete();
            return;
        }

        itemToGive = Who.CurrentHeld;

        Who.AddTrait(Traits.IgnoreDamage_JuliusP);
        Who.AddTrait(Traits.NoTimerStunNegation_JuliusP);

        LJP = God.LB as Level_JuliusP;

        exitRemoved = false;
        followDialoguePlayed = false;
        ExitRef = null;

        God.C(GiveItemLogic());
    }

    public override void OnRun()
    {
        if (isEnded)
            return;

        // ONLY RUN ONCE
        if (!exitRemoved)
        {
            ExitRef = God.GM != null ? God.GM.Exit : null;

            if (ExitRef != null)
                ExitRef.RemoveTrait(Traits.Exit);

            exitRemoved = true;
        }

        if (canFollow &&
            Who != null &&
            Who.Thing != null &&
            God.Session.Player != null &&
            God.Session.Player.Thing != null)
        {
            ThingController p = God.Session.Player.Thing;

            Vector2 offset = -(Vector2)p.transform.up * 1.0f;

            Who.Thing.transform.position = (Vector2)p.transform.position + offset;
            Who.Thing.transform.rotation = p.transform.rotation;
        }

        if (Who != null && !Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);
    }

    private IEnumerator GiveItemLogic()
    {
        ThingInfo player = God.Session.Player;

      
        while (!canFollow)
        {
            if (isEnded)
                yield break;

            if (Who == null || Who.Thing == null || player == null || player.Thing == null)
            {
                yield return null;
                continue;
            }

            if (Who.Thing.Distance(player.Thing) < 1.3f)
            {
                float spd = player.CurrentSpeed;
                player.CurrentSpeed = 0f;

                if (!followDialoguePlayed)
                {
                    God.GM.SetUI("TradeMessage", "Let's get out of here!", 2);
                    followDialoguePlayed = true;
                }

                yield return new WaitForSeconds(1.5f);

                if (isEnded)
                    yield break;

                if (player != null)
                    player.CurrentSpeed = spd;

                God.GM.SetUI("TradeMessage", null, 2);

                canFollow = true;

                if (LJP != null && LJP.Lv3FirstRedLightKilled && LJP.Lv3FirstLevel2ShieldEnemyKilled && LJP.Lv3RedLight3Killed &&  LJP.Lv3FinalShieldEnemKilled)
                    LJP.Lv3FinalDoorCanOpen = true;
            }

            yield return null;
        }

        
        while (true)
        {
            if (isEnded)
                yield break;

            ThingInfo exit = God.GM != null ? God.GM.Exit : null;

            if (Who == null || Who.Thing == null ||
                player == null || player.Thing == null ||
                exit == null || exit.Thing == null)
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

        if (Who == null || Who.Thing == null || player == null || player.Thing == null)
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

        God.GM.SetUI("TradeMessage", "Be careful with this!", 2);

        yield return new WaitForSeconds(1.2f);

        if (isEnded)
            yield break;

        if (itemToGive != null && Who != null)
        {
            Who.DropHeld(false);
        }

        yield return new WaitForSeconds(0.5f);

        if (isEnded)
            yield break;

        God.GM.SetUI("TradeMessage", null, 2);

        if (player != null)
            player.CurrentSpeed = originalPlayerSpeed;

        if (Who != null && Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);

        if (Who != null && Who.Has(Traits.NoTimerStunNegation_JuliusP))
            Who.RemoveTrait(Traits.NoTimerStunNegation_JuliusP);

        isEnded = true;

        if (Who != null && Who.Thing != null)
        {
            God.C(DestroyAfterEnd());
        }

        Complete();
    }

    private IEnumerator DestroyAfterEnd()
    {
        yield return new WaitForSeconds(0.5f);

        ThingInfo exit = God.GM != null ? God.GM.Exit : null;

        if (exit != null && !exit.Has(Traits.Exit))
        {
            exit.AddTrait(Traits.Exit);
           // Debug.Log("Exit trait restored");
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