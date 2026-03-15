using UnityEngine;

public class StunDrink : Trait
{
    
    public StunDrink()
    {
        
        Type = Traits.TemporaryStunImmunity_JuliusP;

        AddListen(EventTypes.OnUse);




    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {

        switch (e.Type)
        {

        
        case EventTypes.OnUse:
        {

        ThingInfo user = e.GetThing(); // THIS is the player
            
        
        user.AddTrait(Traits.StunNegation_JuliusP);
       
        break;
       
        }



            
        }
    


   }
}
