using UnityEngine;
using System.Collections;

using UnityEngine;
using System.Collections;

public class GiveItem_LvL2 : ActionScript
{
    bool canFollow = false;

    private ThingInfo itemToGive;

    float originalPlayerSpeed;

    Level_WesleyP LWP;

    ThingInfo ExitRef;

    public GiveItem_LvL2(ThingInfo who, EventInfo e = null)
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

        if (player == null || player.Thing == null)
        {
            Complete();
            return;
        }

        itemToGive = Who.CurrentHeld;

        Who.AddTrait(Traits.IgnoreDamage_JuliusP);
        Who.AddTrait(Traits.NoTimerStunNegation_JuliusP);

        // STORE EXIT 
        ExitRef = God.GM.Exit;

        // REMOVE EXIT TRAIT
        if (ExitRef != null && ExitRef.Has(Traits.Exit))
        {
            ExitRef.RemoveTrait(Traits.Exit);
        }

        God.C(GiveItemLogic());

        LWP = God.LB as Level_WesleyP;
    }

    public override void OnRun()
    {
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
    }

    private bool MyMethod()
    {
        return LWP != null;
    }

    private IEnumerator GiveItemLogic()
    {
        ThingInfo player = God.Session.Player;
        ThingInfo Exit = God.GM.Exit;

        if (Exit != null && Exit.Has(Traits.Exit))
            Exit.RemoveTrait(Traits.Exit);

        while (!canFollow)
        {
            if (Who == null || Who.Thing == null ||
                player == null || player.Thing == null)
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

                if (player != null)
                    player.CurrentSpeed = spd;

                God.GM.SetUI("TradeMessage", null, 2);

                canFollow = true;

                if (MyMethod())
                    LWP.LvL2AllyFound = true;
            }

            yield return null;
        }

        while (true)
        {
            if (Who == null || Who.Thing == null ||
                player == null || player.Thing == null ||
                Exit == null || Exit.Thing == null)
            {
                yield return null;
                continue;
            }

            if (player.Thing.Distance(Exit.Thing) < 1f)
                break;

            yield return null;
        }

        if (Who == null || Who.Thing == null ||
            player == null || player.Thing == null)
        {
            Complete();
            yield break;
        }

        originalPlayerSpeed = player.CurrentSpeed;
        player.CurrentSpeed = 0f;

        if (Who != null && Who.Thing != null &&
            player != null && player.Thing != null)
        {
            Who.Thing.LookAt(player.Thing, 0f);
        }

        God.GM.SetUI("TradeMessage", "Thank you... please take this!", 2);

        yield return new WaitForSeconds(1.2f);

        if (Who != null && itemToGive != null)
            Who.DropHeld(false);

        yield return new WaitForSeconds(0.5f);

        God.GM.SetUI("TradeMessage", null, 2);

        if (player != null)
            player.CurrentSpeed = originalPlayerSpeed;

        if (Who != null)
        {
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);
            Who.RemoveTrait(Traits.NoTimerStunNegation_JuliusP);
        }

   
        if (ExitRef != null)
        {
            God.C(RestoreExit());
        }

      

        Complete();

        if (Who != null)
            Who.Destruct(Who);
    }

    private IEnumerator RestoreExit()
    {
        yield return null; 

        if (ExitRef != null)
        {
            if (!ExitRef.Has(Traits.Exit))
            {
                ExitRef.AddTrait(Traits.Exit);
                Debug.Log("Exit trait restored successfully (delayed safe)");
            }
        }
    }

    public override Actions NextAction()
    {
        return Actions.None;
    }
}