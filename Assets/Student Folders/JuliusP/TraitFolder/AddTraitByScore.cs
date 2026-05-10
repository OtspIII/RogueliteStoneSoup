using UnityEngine;

public class AddTraitByScore : Trait
{
   
    public AddTraitByScore()
    {
        
        Type = Traits.AddTraitByScore_JuliusP;

        AddListen(EventTypes.GetScore);
    }



    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        

        switch (e.Type)
        {

            case EventTypes.GetScore:
            {
                    
                   //int score = e.GetN("Score");
                   break;


            }

        }




    }
}
