using UnityEngine;
using System.Collections;

public class Lv2Invis : ActionScript
{
    
    SpriteRenderer[] SRS;
    float timer = 0f;
    bool started = false;

    bool Teleported = false;

    public int timesFound = 0;

    // TELEPORT COOLDOWN TO AVOID SPAM
    float teleportCooldown = 1f;
    float teleportTimer = 0f;

    // Timer for blinking
    float AppearTimer = 0f;


    //MAKE A CHANCE VARIABLE//

    float AttackChance = 0.4f;


    float speed = 1.2f;



    public enum Lv2MindSet
    {
        
        Hunt,

        
        Teleport,


        Attack,



    }

    Lv2MindSet currentMindSet = Lv2MindSet.Teleport;

    public Lv2Invis(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Lv2_BarrierShield_JuliusP, who, true);
        HaltMomentum = false;
        MoveMult = 0.2f;
        Duration = Mathf.Infinity;

        
    }

    public override void Begin()
    {
     

      
    }

    public override void OnRun()
    {
        base.OnRun();

        

      MoveTowardPlayerWhileCloak();

    }



   void MoveTowardPlayerWhileCloak()
   {
    
    ThingInfo Player = God.Session.Player;

   
    float Distance = Who.Thing.Distance(Player);


   

    //DIST CHECK//

    if(Distance <= 2.5)
    {
 

     //GIVE THE THING A CHANCE TO TELEPORT//
     God.C(ChanceToTeleport());

    


    }
  
    //ADD INVISBILITY TRAIT WHEN CLOSE//
    if(Distance < 4.5f && !Who.Has(Traits.GainInvis_JuliusP))
    {


        //MAKE THE THING MOVE TOWARDS PLAYER//
        Who.Thing.MoveTowards(Player);

        //MAKE THE THING LOOK AT THE PLAYER//
        Who.Thing.LookAt(Player);
    
        //ADD THE INVSISIBILITY TRAIT TO THE THING//    
        Who.AddTrait(Traits.GainInvis_JuliusP);

        

    }

   
   
   }


    //FINCTION THAT MAKES THE THING TELEPORT (RANDOM CHANCE)//
    IEnumerator ChanceToTeleport()
    {

        yield return new WaitForSeconds(0.19f);
        
        //40% CHANCE TO TELEPORT//
        float TeleportChance = 0.25f;

        //GET THIINGINFO OF PLAYER//
        ThingInfo Player = God.Session.Player;

        //DISTANCE TO THE PLAYER//
        float Dist = Who.Thing.Distance(Player);


        //IF DIST IS LESS THAN 2.1, RANDOM CHANCE VALUE GREATER THAN 0.4, AND HASNT TELEPORTED YET//
        if(Dist <= 2.49f  && Random.value > TeleportChance && !Teleported)
        {
            
        
        //GET THE PLAYER'S POSITION//
        Vector2 playerPos = God.Session.Player.Thing.transform.position;


        //GET A RANDOM POINT ON THE CIRCLE//
        Vector2 RandomPos = UnityEngine.Random.insideUnitCircle * 4f;


        //TELEPORT TO LOCATION//
        Vector2 TeleportPos = playerPos + RandomPos;


        //ACTUALLY TELEPORT TO THE POSITION//
        Who.Thing.transform.position = TeleportPos;


        //SET TO TRUE, TO ONLY TELEPORT ONCE//
        Teleported = true;


        yield return new WaitForSeconds(1f);

        Complete();





        }


        
    }



    public override void End()
    {
        base.End();

    }
        

    public override Actions NextAction()
    {
        return Actions.DefaultAttack;
    }
}