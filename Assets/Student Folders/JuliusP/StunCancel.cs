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

    
        if (e.Get(ActionInfo.Action) == Actions.Stun)
        {
            // Cancel the stun
            e.Abort = true;

            // Reset velocity so the NPC doesn't fly around
            if (i.Who != null && i.Who.Thing != null)
            {
                i.Who.Thing.ActualMove = Vector2.zero; // stop movement from knockback
                i.Who.Thing.Knockback = Vector2.zero; // clear knockback vector
            }

           // Debug.Log("Stun canceled and knockback prevented!");
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

               // Debug.Log("Hello");
            
            
            
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
                    
                    //Debug.Log("DashTrait added from potion");
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
            //Debug.Log("DashTrait removed");
        }
    }

    
}






//THIS TRAIT IS FOR LV2 BOSS

public class FullStunNegation : Trait
{

    public bool IsDead;
    public FullStunNegation()
    {
        Type = Traits.NoTimerStunNegation_JuliusP;

        AddPreListen(EventTypes.StartAction);
        AddListen(EventTypes.OnTouch);
        AddListen(EventTypes.Death);
    }

    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.StartAction:
            {
                if (e.Get(ActionInfo.Action) == Actions.Stun && i.Who != null &&i.Who.Thing != null)
                {
                    e.Abort = true;
                    //("NO STUN");
                }
                break;
            }
        }
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnTouch:
            {
                GameCollision col = e.Collision;

                if (col == null || col.Other == null)
                    break;

                ThingInfo other = col.Other.Info;

                if (other == null)
                    break;

                // attacker = thing that hit us
                ThingInfo attacker = other;

                // weapon from attacker
                ThingInfo weapon = attacker.CurrentHeld;

               
                if (weapon == null && attacker.ChildOf != null)
                {
                    weapon = attacker.ChildOf.CurrentHeld;
                }

                string weaponName = weapon?.GetName(true)?.ToLower();

                string hitName =  other.Name?.ToLower();

          
                //CHECK FOR SWORD NAME//
                if (weaponName != null && weaponName.Contains("sword"))
                {
                   
                    

                    i.Who.DesiredMove = Vector2.zero;
                    i.Who.Thing.ActualMove = Vector2.zero;

                    Rigidbody2D rb = i.Who.Thing.gameObject.GetComponent<Rigidbody2D>();

                    if (rb != null)
                    {
                        rb.linearVelocity = Vector2.zero;
                        rb.angularVelocity = 0f;
                    }
                    
                    
                    i.Who.AddTrait(Traits.IgnoreDamage_JuliusP);


                    break;
                }

                //CHECK FOR WOLFBANE ARROW//
                if (weaponName != null && (weaponName.Contains("Wolf Bane Bow") || hitName.Contains("WolfBaneArrow")))
                {
                   // Debug.Log("Bow/Arrow knockback allowed");


                    i.Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);
                }




            if (other.Thing.name.Contains("Lava"))
            {
              //  Debug.Log("Touched Lava!");

                if (i.Who.Has(Traits.IgnoreDamage_JuliusP))
                {
             
                    i.Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);
                }
            }

                break;
            }

            case EventTypes.Death:
            {
              var level = God.LB as Level_JuliusP;
              if (level == null)
              break;

            ThingInfo deadThing = i.Who;

            if (deadThing == null || deadThing.Thing == null)
            break;

            if (deadThing.Thing.name.Contains("Lv2.Boss"))
            {
            level.Lv2BossKilled = true;
            }

         break;
            }



}
}
}
