using UnityEngine;
using System.Collections;

public class IgnoreDamage : Trait
{
  
    public IgnoreDamage()
    {
        
        Type = Traits.IgnoreDamage_JuliusP;

        AddPreListen(EventTypes.Damage);
   
   
    }


    public override void PreEvent(TraitInfo i, EventInfo e) 
    {
        switch (e.Type) 
        {
            case EventTypes.Damage:
            {
                   
                e.Abort = true;

                break;
            }
           
        }
    }

}


public class TemporaryDamageResist : Trait
{

    public float DmgTimer = 6f;
    

    public TemporaryDamageResist()
    {
        
        Type = Traits.TemporaryDmgResist_JuliusP;

        AddListen(EventTypes.OnUse);
    
    
    }


    public override void TakeEvent(TraitInfo i, EventInfo e)
    {

        ThingInfo Player = God.Session.Player;

        switch (e.Type)
        {
            

            case EventTypes.OnUse:
            {

            
            Player.AddTrait(Traits.IgnoreDamage_JuliusP);      
            
           // Debug.Log("Used");

            God.C(DmgResistTimer(Player));

            break;
            
            
            
            
            }



        }




    }


    private IEnumerator DmgResistTimer(ThingInfo Player)
    {
        
        float timer = 0f;


        while(timer < DmgTimer)
        {
            
            timer += Time.deltaTime;

            yield return null;



        }


        if (Player.Has(Traits.IgnoreDamage_JuliusP))
        {
            
            Player.RemoveTrait(Traits.IgnoreDamage_JuliusP);

            //Debug.Log("Trait removed");


        }

        
    }




}

public class InvisPotion_JuliusP : Trait
{
    public float InvisDuration = 5f;

    public InvisPotion_JuliusP()
    {
        Type = Traits.TempInvis_JuliusP;

        AddListen(EventTypes.OnUse);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch(e.Type)
        {
            case EventTypes.OnUse:
            {
                ThingInfo player = God.Session.Player;

                // ADD INVIS TRAIT
                player.AddTrait(Traits.DodgeInvis_JuliusP);

               // Debug.Log("Invis Activated");

                // START TIMER
                God.C(RemoveInvis(player));

                break;
            }
        }
    }

    private IEnumerator RemoveInvis(ThingInfo player)
    {
        yield return new WaitForSeconds(InvisDuration);

        if(player.Has(Traits.DodgeInvis_JuliusP))
        {
            player.RemoveTrait(Traits.DodgeInvis_JuliusP);

            player.RemoveTrait(Traits.Dash);

            player.RemoveTrait(Traits.GainInvis_JuliusP);

        }
    }
}


