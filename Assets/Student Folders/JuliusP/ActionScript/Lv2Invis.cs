using UnityEngine;
using System.Collections;

public class Lv2Invis : ActionScript
{
    
  
    float timer = 0f;
    bool started = false;

    bool Teleported = false;

    bool CanTeleport = false;

    bool canGoInvis = false;

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



    IEnumerator ChanceToGoInvis()
    {
        yield return new WaitForSeconds(0.6f);
        

  
      Who.AddTrait(Traits.GainInvis_JuliusP);




    }


   void MoveTowardPlayerWhileCloak()
   {
   

     if (God.Session.Player == null || Who == null || Who.Thing == null)
        return;

    ThingInfo Player = God.Session.Player;
    float Distance = Who.Thing.Distance(Player);

    Who.Thing.LookAt(Player, 0f);


    

    // Only move if within a certain range
    if (Distance < 4.5f) // Adjust as needed
    {
        Who.Thing.MoveTowards(Player);

        God.C(ChanceToGoInvis());
    }



    // Optional: attack / teleport logic
    if (Distance <= Who.AttackRange)
    {
        Who.Thing.ActualMove = Vector2.zero; // stop moving
        Complete(); // allow NextAction() to run
    }
   } 

    //FINCTION THAT MAKES THE THING TELEPORT (RANDOM CHANCE)//
    IEnumerator ChanceToTeleport()
    {
    
    yield return new WaitForSeconds(0.19f);

    float TeleportChance = 0.25f;
    ThingInfo Player = God.Session.Player;
    float Dist = Who.Thing.Distance(Player);

   
    if (Dist <= 2.49f && Random.value > TeleportChance)
    {
        Vector2 playerPos = Player.Thing.transform.position;
        Vector2 RandomPos = UnityEngine.Random.insideUnitCircle * 4f;
        Vector2 TeleportPos = playerPos + RandomPos;

        Who.Thing.transform.position = TeleportPos;

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