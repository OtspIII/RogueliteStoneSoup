using UnityEngine;
using System.Collections;

public class StunCancel : Trait
{

    float timer = 0;
   
    public StunCancel()
    {
        Type = Traits.StunNegation_JuliusP;

      
        AddPreListen(EventTypes.StartAction);

       
        AddListen(EventTypes.Update);

      
    }

    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //IF ITS A STARTACTION EVENT
            case EventTypes.StartAction:
            {
                // CHECK IF THE ACTION IS A STUN ACTION//
                if (e.Get(ActionInfo.Action) == Actions.Stun)
                {
                    //CANCEL THE STUN ACTION FROM HAPPENING
                    e.Abort = true;
                    Debug.Log("Stun canceled by StunNegationTrait!");
                }
                break;
                
            }





        }

       
    }


     public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            //IF ITS A UPDATE EVENT
            case EventTypes.Update:
            {
                timer += Time.deltaTime;


            //EFFECT TIME IT LASTS
            if (timer >= 15f)
            {
                i.Who.RemoveTrait(Traits.StunNegation_JuliusP);

                Debug.Log("Hello");
            
            
            
            }
                break;
                
            }

        }
       
    }


}


public class TemporaryDashAbility : Trait
{

    //VARIABLES TO USE FOR TIMER AND CHECKING STATES//
    private float DashTimer = 0f;
    private float TimeAllowedToDash = 6.5f;
    private bool canStartTimer = false;

    public TemporaryDashAbility()
    {
        Type = Traits.TemporaryDash_JuliusP;

        AddListen(EventTypes.OnUse);
       // AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        // GET THE THINGINFO OF THE PLAYER//
        ThingInfo Player = God.Session.Player;
        
        // SAFETY CHECK//
        if (Player == null) return; 

        switch (e.Type)
        {
            case EventTypes.OnUse:
                if (!canStartTimer)
                {

                    //ADDS THE DAHS TRAIT UPON DRINKING THE POTION//
                    Player.AddTrait(Traits.Dash);
                    
                    //ALLOWS TO USE/START COROUTINES
                    God.C(RemoveDashAfterDelay(Player));
                    
                    Debug.Log("DashTrait added from potion");
                }
                break;

          
        }
    }


    private IEnumerator RemoveDashAfterDelay(ThingInfo player)
    {
        float timer = 0f;

        while (timer < TimeAllowedToDash)
        {
            timer += Time.deltaTime;
            yield return null; 
        }

        // IF PLAYER HAS DASH TRAIT, REMOVE IT//
        if (player.Has(Traits.Dash))
        {
            player.RemoveTrait(Traits.Dash);
            Debug.Log("DashTrait removed");
        }
    }

    
}