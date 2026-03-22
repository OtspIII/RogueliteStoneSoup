using UnityEngine;
using System.Collections;

public class TradeHp : ActionScript
{
    //ALLOWED TO TRADE HEALTH OR NOT//
    bool canTradeHealths = false;
   
    //RANGE WHERE THE THING CAN GRAB THE PLAYER'S HEALTH//
    float tradeRange = 4f;
   
    //ONLY ALLOW TO RUN ONCE//
    bool hasStartedCoroutine = false;

    // SAVE THE WEAPON //
    private ThingInfo Sword;

    bool canAsk = false;
   
    //SAVE ORIGINAL SPEED//
    float OgSpeed;

    
    //IF THE THING IS ATTAHCED OR NOT//
    bool isAttached = false;

    public TradeHp(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.TradeHp_JuliusP, who, true);
        HaltMomentum = false;
        MoveMult = 0.2f;
        Duration = Mathf.Infinity;
    }

    public override void Begin()
    {
        base.Begin();

        // SAVE THE WEAPON AND MAKE IT NULL
        if (Who.CurrentHeld != null)
        {
            Sword = Who.CurrentHeld;
            Sword = null;
        }

        // MAKES THE THING UNABLE TO BE HIT AND STUNNED AND TRIGGERED TO ATTACK//
        Who.AddTrait(Traits.IgnoreDamage_JuliusP);
        Who.AddTrait(Traits.NoTimerStunNegation_JuliusP);
    }

    public override void OnRun()
    {
        // START ASK COOROUTINE ONCE//
        if (!hasStartedCoroutine)
        {
            hasStartedCoroutine = true;
            God.C(AskPlayerForHelp());
        }

        // MAKE THE THING ATTACHED TO THE PLAYER AND FOLLOW BEHIND THE PLAYER//
        if (canAsk && God.Session.Player != null && God.Session.Player.Thing != null)
        {
            isAttached = true;
            ThingController playerTC = God.Session.Player.Thing;
            Vector2 offset = -(Vector2)playerTC.transform.up * 1.0f;
            Who.Thing.transform.position = (Vector2)playerTC.transform.position + offset;
            Who.Thing.transform.rotation = playerTC.transform.rotation;

            //SAFETY CHECK//
            if (!Who.Has(Traits.IgnoreDamage_JuliusP))
                Who.AddTrait(Traits.IgnoreDamage_JuliusP);
        }
        
        else
        {
            isAttached = false;
        }
    }

    // FUNCTION THAT TRIGGERS WHEN THE PLAYER ENCOUTNERS THE VAMPIRE//
    public IEnumerator AskPlayerForHelp()
    {
        //GET THINGINFO OF PLAYER AND EXIT//
        ThingInfo Player = God.Session.Player;
        ThingInfo Exit = God.GM.Exit;

        //CHECKS IF THE EXIT HAS THE EXIT TRAIT AND REMOVES IT, THE PLAYER CAN'T LEAVE UNTIL IT KILLS THE VAMPIRE//
        if (Exit.Has(Traits.Exit))
            Exit.RemoveTrait(Traits.Exit);

        while (!canAsk)
        {

            //SAFETY CHECKS//
            if (Who == null || Who.Thing == null || Player == null || Player.Thing == null)
            {
                yield return null;
                continue;
            }

            float distanceToPlayer = Who.Thing.Distance(Player.Thing);
           
            //ENOCUNTER WITH THE VAMPIRE, ASKS PLAYER TO LEAD IT TO THE EXIT//
            if (distanceToPlayer < 1.3f)
            {
                //TEMPORARILY STOP PLAYER//
                float originalSpeed = Player.CurrentSpeed;
                Player.CurrentSpeed = 0f;
                
                //THING ASKS PLAYER TO LEAD IT TO THE EXIT//
                God.GM.SetUI("TradeMessage", "Can you lead me to the exit?", 2);
                
                //CAN MOVE AGAIN//
                yield return new WaitForSeconds(2f);
                Player.CurrentSpeed = originalSpeed;
                
                //DELETE DIALOGUE ON SCREEN//
                God.GM.SetUI("TradeMessage", null, 2);
                canAsk = true;
            }

            yield return null;
        }

        bool reachedExit = false;
        while (!reachedExit)
        {
            if (Who == null || Who.Thing == null || Player == null || Player.Thing == null || Exit == null || Exit.Thing == null)
            {
                yield return null;
                continue;
            }

            //GETS DISTANCE TO EXIT AND MAKES THING MOVETOWARD EXIT//
            float distanceToExit = Player.Thing.Distance(Exit.Thing);
           // Who.Thing.MoveTowards(Exit.Thing);

            //CHECKS DISTANCE THRESHOLD TO THE EXIT//
            if (distanceToExit < 1f)
            {
                //MAKE PLAYER CANT MOVE//
                float originalSpeed = Player.CurrentSpeed;
                Player.CurrentSpeed = 0f;
                
                //ENEMY TRICKS PLAYER//
                God.GM.SetUI("TradeMessage", "YOUR HEALTH IS MINE NOW!", 2);
                
                yield return new WaitForSeconds(1f);
                
                //DELETE THE DIALOUGE//
                God.GM.SetUI("TradeMessage", null, 2);
                
                //REACHED THE EXIT//
                reachedExit = true;

                //SET PLAYER SPEED TO ORIGINAL SPEED//  
                Player.CurrentSpeed = originalSpeed;

                // START THE STEAL HEALTH COROUTINE//
                God.C(StealHP());
            }

            yield return null;
        }
    }

    // FUNCTION FOR THE THING STEALING THE PLAYER'S HEALTH//
    public IEnumerator StealHP()
    {
        ThingInfo Player = God.Session.Player;
        OgSpeed = Player.CurrentSpeed;

        while (!canTradeHealths)
        {

            //CHECK IF THE PLAYER OT THING IS ALIVE FOR SAFETY CHECK OR EXISTS//
            if (Who == null || Who.Thing == null || Player == null || Player.Thing == null)
            {
                yield return null;
                continue;
            }

            //STORE DISTANCE TO PLAYER//
            float distanceToPlayer = Who.Thing.Distance(Player.Thing);

            if (distanceToPlayer <= tradeRange)
            {
                // MAKE THE ENEMY AND PLAYER FREEZED FOR A SHORT AMOUNT OF TIME AND PLAYER DOES A STUN ACTION//
                Player.CurrentSpeed = 0f;
                MoveMult = 0f;
                Player.DoAction(Actions.Stun);
                
                yield return new WaitForSeconds(1.5f);

                // SWAP HEALTH VALUES, ENEMY GETS PLAYER HEALTH, PLAYER GETS ENEMY HEALTH//
                TraitInfo playerHealth = Player.Get(Traits.Health);
                TraitInfo enemyHealth = Who.Get(Traits.Health);

                //SAFETY CHECK//
                if (playerHealth != null && enemyHealth != null)
                {
                    float playerOriginal = playerHealth.GetN();
                    float enemyOriginal = enemyHealth.GetN();

                    //SET THE HEALTH//
                    enemyHealth.Set(playerOriginal);
                    playerHealth.Set(enemyOriginal);

                    //UPDATE THE PLAYER'S HEALTH ON SCREEN//
                    EventInfo ph = Player.Ask(EventTypes.ShownHP);
                    
                    God.GM.SetUI("Health", ph.GetInt() + "/" + ph.GetInt(NumInfo.Max), 1);
                }

                // TELEPORT ABOVE PLAYER AND LOOK AT THE PLAYER//
                Who.Thing.transform.position = Player.Thing.transform.position + Who.Thing.transform.up * 3f;
                Who.Thing.LookAt(Player.Thing, 0f);

                //THE SWORD IS READY TO USE NOW//
                if (Sword != null)
                {
                    Who.SetHeld(Sword);
                    Who.TakeEvent(God.E(EventTypes.OnHoldStart).Set(Sword));
                }

                // REMOVE THE IGNORE DAMAGE TRAIT, THEY CAN TAKE DAMAGE NOW//
                if (Who.Has(Traits.IgnoreDamage_JuliusP))
                    Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);
               
                // REMOVE THE NO TIMER STUN NEGATION TRAIT, THEY CAN BE STUNNED NOW//
                if (Who.Has(Traits.NoTimerStunNegation_JuliusP))
                    Who.RemoveTrait(Traits.NoTimerStunNegation_JuliusP);


                //MAKE THE THING CHASE AFTER STEALING HEALTH//
                Who.DoAction(Actions.Chase);

                //PLAYER IS BACK TO FULL SPEED//
                Player.CurrentSpeed = OgSpeed;


                //CAN TRADE HEALTHS TO TRUE AND NO LONGER ATTACHED//
                canTradeHealths = true;
                isAttached = false;

                // Wait for NPC death
                yield return God.C(WaitForDeath());

                // REMOVES THE MESSAGE FROM THE LEFT SIDE OF THE SCREEN//
                God.GM.SetUI("TradeMessage", "");
            }

            yield return null;
        }
    }

    // THIS COUROITNE IS FOR CHECKING WHEN THE THING DIES//
    private IEnumerator WaitForDeath()
    {
        //GET ENEMYHEALTH TRAIT//
        TraitInfo enemyHealth = Who.Get(Traits.Health);

        //WHILE THE THING IS ALIVE AND HEALTH IS ABOVE 0, CHECK AND SKIP UNTIL IT DOSEN'T MEET THIS REQURIEMENT//
        while (enemyHealth != null && enemyHealth.GetN() > 0)
        {
            yield return null; 
        }

        Debug.Log("Enemy has died!");
       

        //GET THINGINFO OF THE EXIT//
         ThingInfo Exit = God.GM.Exit;


        //SAFETY CHECK: IF THE EXIT DOSEN'T HAVE THE EXIT TRAIT//
        if (!Exit.Has(Traits.Exit))

            //ADD THE EXIT TRAIT BACK, THE PLAYER CAN NOW LEAVE THE ROOM//
            Exit.AddTrait(Traits.Exit);
    }

    public override Actions NextAction()
    {
        return Actions.DefaultAttack;
    }
}