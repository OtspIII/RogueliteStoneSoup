using UnityEngine;
using System.Collections;

public class GiveItem : ActionScript
{
    bool hasStartedCoroutine = false;
    bool canFollow = false;

    private ThingInfo itemToGive;

    float originalPlayerSpeed;

    Level_JuliusP LJP;

    public GiveItem(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.GiveItem_JuliusP, who, true);
        HaltMomentum = false;
        MoveMult = 0.2f;
        Duration = Mathf.Infinity;
    }

    public override void Begin()
    {
        base.Begin();

        //GET THINGINFO OF PLAYER//
        ThingInfo player = God.Session.Player;

        // EXITS IF THE THING OR PLAYER DOSNE'T EXIST FOR SAFETY CHECK//
        if (player == null || player.Thing == null)
        {
            Complete();
            return;
        }

        //SAVES THE ITEM IN MEMORY TO USE LATER//
        itemToGive = Who.CurrentHeld;


        // THE THING CAN'T BE HIT//
        Who.AddTrait(Traits.IgnoreDamage_JuliusP);
        Who.AddTrait(Traits.NoTimerStunNegation_JuliusP);

        //PLAY THE GIVEITEM COUROUTINE//
        God.C(GiveItemLogic());


        LJP = God.LB as Level_JuliusP;
    }

    public override void OnRun()
    {
        // CODE FOR FOLLOW AND ATTACH LOGIC TO THE PLAYER//
        if (canFollow && God.Session.Player != null && God.Session.Player.Thing != null)
        {
            ThingController p = God.Session.Player.Thing;

            Vector2 offset = -(Vector2)p.transform.up * 1.0f;

            Who.Thing.transform.position = (Vector2)p.transform.position + offset;
            Who.Thing.transform.rotation = p.transform.rotation;

  
            
        }


         //SAFETY CHECK//
            if (!Who.Has(Traits.IgnoreDamage_JuliusP))
                Who.AddTrait(Traits.IgnoreDamage_JuliusP);
    }

    private IEnumerator GiveItemLogic()
    {
        //GET THE THINGINOF OF THE THING AND THE EXIT TO ACCESS TRAITS//
        ThingInfo player = God.Session.Player;
        ThingInfo exit = God.GM.Exit;

        // WHILE CAN FOLLOW IS FALSE -> HASN'T APPROACHED THE THING YET
        while (!canFollow)
        {
            if (Who == null || player == null || player.Thing == null)
            {
                yield return null;
                continue;
            }


            //IF THE PLAYER IS CLOSE ENOUGH TO THE THING, DISPLAY THE HELP TEXT//
            if (Who.Thing.Distance(player.Thing) < 1.3f)
            {
                float spd = player.CurrentSpeed;
                player.CurrentSpeed = 0f;

                //ASK PLAYER TO HELP//
                God.GM.SetUI("TradeMessage", "Can you help me reach the exit?", 2);

                yield return new WaitForSeconds(1.5f);

                //SET PLAYER BCK TO NORMAL SPEED AND EMPTY THE MESSAGE//
                player.CurrentSpeed = spd;
                God.GM.SetUI("TradeMessage", null, 2);

                canFollow = true;

                LJP.CanLinkToLootRoom = true;

                Debug.Log (LJP.CanLinkToLootRoom);
            }

            yield return null;
        }

        //WHILE CANFOLLOW IS TRUE//
        while (true)
        {
            if (Who == null || player == null || exit == null)
            {
                yield return null;
                continue;
            }

            //IF PLAYER IS CLOSE TO THE EXIT, BREAK OUT THE LOOP//
            if (player.Thing.Distance(exit.Thing) < 2.6f)
                break;

            yield return null;
        }

        // MAKE THE PLAYER STOP WHEN THE NPC IS ABOUT TO TALK..
        originalPlayerSpeed = player.CurrentSpeed;
        player.CurrentSpeed = 0f;

        Who.Thing.LookAt(player.Thing, 0f);

        //DISPLAY DIALOGUE FOR TALKING//
        God.GM.SetUI("TradeMessage", "Thank you... please take this!", 2);

        yield return new WaitForSeconds(1.2f);

        // GIVE THE ITEM//
        if (itemToGive != null)
        {
            Who.DropHeld(false);
            
        }

        yield return new WaitForSeconds(0.5f);

        God.GM.SetUI("TradeMessage", null, 2);

        //SET PLAYER SPEED BACK TO NORMAL//
        player.CurrentSpeed = originalPlayerSpeed;

      
        if (Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);

        if (Who.Has(Traits.NoTimerStunNegation_JuliusP))
            Who.RemoveTrait(Traits.NoTimerStunNegation_JuliusP);

        // MAKE NPC DESTROY ITSELF OR DISAPPEAR AFTER GIVING ITEM//
        Who.Destruct(Who);

        //MARK AS COMPLETE//
        Complete();
    }

    public override Actions NextAction()
    {
        return Actions.None;
    }




 


}