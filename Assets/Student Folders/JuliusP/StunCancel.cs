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
            //IF ITS A STARTACTION EVENT
            case EventTypes.Update:
            {
                timer += Time.deltaTime;



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