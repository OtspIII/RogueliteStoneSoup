using UnityEngine;

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
