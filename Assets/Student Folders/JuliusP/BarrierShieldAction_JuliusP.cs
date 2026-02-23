using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BarrierShieldAction_JuliusP : ActionScript
{
  
    ThingInfo BarrierShield;


    //OFFSET FOR THE SHIELDS//
    float Offset = 1.4f;

    

    public BarrierShieldAction_JuliusP(ThingInfo who, EventInfo e = null)
    {
        
   
        Setup(Actions.BarrierShield_JuliusP, who);

        //MAKE THING STAY IN PLACE WHILE SHIELD IS SUMMON//

        MoveMult = 0;

        HaltMomentum = true;

        CanRotate = true;

       


        
   
   
    }

  public override void Begin()
{
    base.Begin();

    // THIS FINDS THE SHIELD IN THE SPECIFIC RESOURCE FOLDER
    ThingOption Shield = Resources.Load<ThingOption>("JuliusP/BarrierShield");



    //MAKE THE THING SPAWN WITH FOUR SHIELDS ATTACHED TO THE THING//


    List<Vector3> PossibleDirection = new List<Vector3>
    {
        
    //THIS IS A SPAWN POSITION -> UPWARDS//
    Who.Thing.transform.up,

    //THIS IS A SPAWN POSITION -> DOWNWARDS//
    -Who.Thing.transform.up,

    //THIS IS A SPAWN POSITION -> TO THE LEFT//
    -Who.Thing.transform.right,

    //THIS IS A SPAWN POSITION -> TO THE RIGHT//
    Who.Thing.transform.right,

    };




    foreach(Vector3 Locations in PossibleDirection)
    {

    //SET THE SPAWNPOSITION// 

    Vector3 Spawn = Who.Thing.transform.position + Locations * Offset;   
    

    ThingInfo BarrierShield = new ThingInfo(Shield);

    

    //THIS FULLY SPAWNS THE SHIELDS AROUND THE THING//
    ThingController SpawnShield = BarrierShield.Spawn(Spawn); 


    //MAKE THE SHIELDS FOLLOW THE THING//
    SpawnShield.transform.parent = Who.Thing.transform;
    
    

    
    
    }

 
    
    
}


}


