using UnityEngine;

public class DetectDeath : Trait
{

    Level_JuliusP LJP;

    private ThingInfo WeapontoDrop;


    //THIS TRAIT IS ONLY FOR CHECKING IF A THING DIES TO OPEN A PATHWAY//
   
    public DetectDeath()
    {
        
        Type = Traits.DetectDeath_JuliusP;

        AddListen(EventTypes.Death);

        AddListen(EventTypes.Setup);


    }


    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        

         switch (e.Type)
        {
            case EventTypes.Death:
            {
                int Level = God.Session.Level;

                 //LEVEL 3 LOGIC//
                LJP = God.LB as Level_JuliusP;     


                //FIRST CONDITION//
                if (LJP.Lv3FirstRedLightKilled && Level == 3)
                {
                     LJP.Lv3FirstLevel2ShieldEnemyKilled = true;     
                }



                //SECOND CONDITION//
                if(Level == 3 && LJP.Lv3FirstLevel2ShieldEnemyKilled && LJP.Lv3FirstRedLightKilled && LJP.Lv3RedLight3Killed)
                {
                     LJP.Lv3FinalShieldEnemKilled = true;  
                }



                //FOURTH LEVEL//
                if(Level == 4)
                {
                    Debug.Log("sup");

                    WeapontoDrop = i.Who.CurrentHeld;

                    i.Who.DropHeld(false);
                }   

                }

                break;
            
            

            case EventTypes.OnSpawn:
            {
                //LEVEL 3 LOGIC//
                LJP = God.LB as Level_JuliusP;     

                break;
            }
        }
    }
}