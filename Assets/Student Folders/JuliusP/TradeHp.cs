using UnityEngine;
using System.Collections.Generic;

public class TradeHp : ActionScript
{
    //BOOLEAN TO NOT ALLOW TRADING HEALTHS ENDLESSLY..
    bool canTradeHealths = false;


    public TradeHp(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.TradeHp_JuliusP, who);
        HaltMomentum = false;
        Duration = Mathf.Infinity;
    }

    public override void Begin()
    {
        base.Begin();


        //CHECKS IF THE THING DOSEN'T ALREADY HAVE IGNOREDAMAGE TRAIT AND ADD IT//

        if (!Who.Has(Traits.IgnoreDamage_JuliusP))
        {
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);
        }


    }

    public override void OnRun()
    {
        // GETS THE DISTANCE TO THE PLAYER//
        float distanceToPlayer = Who.Thing.Distance(God.Session.Player.Thing);




        if (distanceToPlayer < 1f && !canTradeHealths)
        {
            // GET THE TRAITINFO OF THE PLAYER AND THE ENEMY// 
            TraitInfo playerHealthTrait = God.Session.Player.Get(Traits.Health);
            TraitInfo enemyHealthTrait = Who.Get(Traits.Health);

            //MAKE THE THING TELEPORT INSTEAD OF AUTOMATICALLY ATTACKING THE PLAYER WHEN IN RANGE//
            VanishWithHealth();

            if (playerHealthTrait != null && enemyHealthTrait != null)
            {
                // SWAP HEALTH VALUES//
                float ThingHealth = enemyHealthTrait.GetN();
                enemyHealthTrait.Set(playerHealthTrait.GetN());
                playerHealthTrait.Set(ThingHealth);

                //UPDATES THE PLAYER'S HEALTH VALUES//
                EventInfo PlayersHealth = God.Session.Player.Ask(EventTypes.ShownHP);
                God.GM.SetUI("Health", PlayersHealth.GetInt() + "/" + PlayersHealth.GetInt(NumInfo.Max), 1);

                canTradeHealths = true;

                 //Debug.Log($"Swapped healths: Player={playerHealthTrait.GetN()} Enemy={enemyHealthTrait.GetN()}");


                //IF THE THING HAS THE IGNORE DAMAGE TRAIT, REMOVE IT TO ALLOW DAMAGE//
                 if (Who.Has(Traits.IgnoreDamage_JuliusP))
                 {

                     Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);
                 
                 
                 }

                //END THE ACTION//
                Complete();


            }
        }


        

    }

    //FUNCTION TO MAKE THE THING TELEPORT//
    void VanishWithHealth()
    {
        
        //GETS THE CURRENT PLAYER IN THE SESSION POSITION//
        Vector3 PlayerPos = God.Session.Player.Thing.transform.position;


        //TELEPORT THE THING TO THE DESIRED POSITION//
        Who.Thing.transform.position = PlayerPos + (Who.Thing.transform.up * 3.3f);
       
        Who.Thing.LookAt(God.Session.Player.Thing, 0f);

        
   
    }



    //CHOOSES THE NEXT ACTION//
    public override Actions NextAction()
    {
        return Actions.DefaultAttack;
    }
}
