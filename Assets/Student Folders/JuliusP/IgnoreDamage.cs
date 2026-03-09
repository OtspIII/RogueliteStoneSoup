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
            
            Debug.Log("Used");

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

            Debug.Log("Trait removed");


        }

        
    }




}


