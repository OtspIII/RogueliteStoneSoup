using UnityEngine;
using System.Collections;

public class Lv3Invis : ActionScript
{

    //STEPS FOR ACTIONS;

    // 1. Enemy teleports behind player
    // 2. Enemy prepares to strike
    // 3. Player hits stunning the enemy 
    // 4. Enemy cant take damage while stunned
    // 5. Enemy dissapeaes
    // 6. Enemy teleports to random location and is invisible when near it reappears and prepares to attack
    // 7. After two times stunned it fully attacks


    bool CanGainTrait = false;

    bool DeliverFatalBlow = false;

    bool canTriggerAura = false;

    bool DetectedHit = false;


    public Lv3Invis(ThingInfo who, EventInfo e = null)
    {
        
        Setup(Actions.Lv3_Cloak_JuliusP, who, true);

        HaltMomentum = false;

        MoveMult = 0.2f;

        Duration = Mathf.Infinity;


    }


    public override void OnRun()
    {

        //MAKES THE THING TELEPORT BEIHND THE PLAYER//
        TeleportBehindPlayer();


        //GET THE THING OF THE PLAYER//
        ThingInfo player = God.Session.Player;

    
        //MAKE THE THING FACE THE PLAYER
        Who.Thing.LookAt(player);


        //IF CANTRIGGERAURA IS TRUE
        if (canTriggerAura)
        {
            
        // MAKES THE THING ATTACHED TO THE PLAYER A CHILD//
        Who.Thing.transform.SetParent(player.Thing.transform);

        //OFFSET THE THING SLIGHTLY BEHIND THE PLAYER//
        Who.Thing.transform.localPosition = new Vector3(0, -1.9f, 0);
        }


      

    }
    

    public override void HitBegin(GameCollision Col)
    {
        

        Debug.Log("Hit");

        DetectedHit = true;





    }


   //FUNCTION TO TELEPORT BEHIND THE PLAYER//
   void TeleportBehindPlayer()
  {

    //GET THE THINGINFO OF THE PLAYER//
    ThingInfo Player = God.Session.Player;

   
    //GET TRANSFORM OF THE PLAYER//
    Transform PlayerPos = Player.Thing.transform;


    //GET THE DISTANCE BETWEEN PLAYER AND THING//
    float Distance = Who.Thing.Distance(Player);


    //IF DISTANCE IS LESS THAN CONDITION AND IF DOSEN'T HAS TRAIT//
    if(Distance < 3f && !CanGainTrait)
    {

        //DEBUGGING DISTANCE//
        Debug.Log("Close");

        //ADD THE INVISIBILTY TRAIT TO THE THING//
        Who.AddTrait(Traits.GainInvis_JuliusP);

        //THE THING HAS THE INVIS TRAIT SET TO TRUE//
        CanGainTrait = true;
    }


    //IF THE THING HAS THE INVISIBILITY TRAIT AND DOSEN'T HAVE THE AURA//

    if (CanGainTrait && !canTriggerAura)
    {
    
        //PREPARE TO ATTACK THE PLAYER//
        God.C(PrepareToAttack());  


        //SET TO TRUE BECAUSE THE THING WILL GET THE AURA THROUGH THE COROUTINE//
        canTriggerAura = true;  
    
    }

  
  }


  //COROUTINE THAT'S FOR THE ATTACK PHASE//
  IEnumerator PrepareToAttack()
  {
        
    
    yield return new WaitForSeconds(2f);
    
    //BREAKS OUT OF THE COROUTINE, STOPPING THE ONE-HIT KILL FROM HAPPENING WHEN THE PLAYER HITS THE THING//
    if (DetectedHit) yield break;

    //GETS TRANSFORM OF THE THING//
    Transform ThingPos = Who.Thing.transform;


    //LOAD DANGER PARTICLE EFFECT//
    GameObject DangerAura = Resources.Load<GameObject>("JuliusP/Gnomes/DangerAura");

  
    //SPAWNS THE AURA AT THE THING'S LOOCATION//
    GameObject Thingaura = GameObject.Instantiate(DangerAura, ThingPos.position, Quaternion.identity);


    // MAKES THE AURA FOLLOW OR BE ATTACHED TO THING//
    Thingaura.transform.SetParent(ThingPos);

    // KEEP THE RAGEAURA CENTERED ON THE THING//
    Thingaura.transform.localPosition = Vector3.zero;

    // PLAYS THE PARTICLE SYSTEM//
    ParticleSystem ps = Thingaura.GetComponent<ParticleSystem>();
                        

    //PLAY THE PARTICLE SYSTEM AURA//               
    if (ps != null)
    {
     ps.Play();
    
    }


    yield return new WaitForSeconds(2f);
    
    //BREAKS OUT OF THE COROUTINE, STOPPING THE ONE-HIT KILL FROM HAPPENING WHEN THE PLAYER HITS THE THING//
    if (DetectedHit) yield break;
    
    //INCREASE THE SPEED OF THE AURA//
    if (ps != null)
    {
    var main = ps.main;
    main.simulationSpeed = 5f; 

    
    }


   yield return new WaitForSeconds(2f);

   //BREAKS OUT OF THE COROUTINE, STOPPING THE ONE-HIT KILL FROM HAPPENING WHEN THE PLAYER HITS THE THING//
   if (DetectedHit) yield break;

   //IF THE THING HAS THE INVISIBLE TRAIT//
   if (Who.Has(Traits.GainInvis_JuliusP))
   {
        //REMOVE IT, MAKING THEM VISIBLE OR ABLE TO BE SEEN//
        Who.RemoveTrait(Traits.GainInvis_JuliusP);

   }


     yield return new WaitForSeconds(1f);

    //BREAKS OUT OF THE COROUTINE, STOPPING THE ONE-HIT KILL FROM HAPPENING WHEN THE PLAYER HITS THE THING//
     if (DetectedHit) yield break;

    //DO THE SWING ANIMATION//
     Who.DoAction(Actions.Swing);
    
    
     yield return new WaitForSeconds(0.21f);
    
     //BREAKS OUT OF THE COROUTINE, STOPPING THE ONE-HIT KILL FROM HAPPENING WHEN THE PLAYER HITS THE THING//
     if (DetectedHit) yield break;
     

     //DELIVER FATAL BLOW//
      DealDamage(10f);


  }


// FUNCTION THAT SENDS A DAMAGE EVENT TO THE PLAYER
void DealDamage(float amount)
{
    if (!DeliverFatalBlow)
    {
            
    //GETS THINGINFO OF THE PLAYER
    ThingInfo Player = God.Session.Player;

    // CREATES DAMAGE EVENT AND SETS NUMBER FOR THE EVENT
    EventInfo DamageEvent = God.E(EventTypes.Damage);
    DamageEvent.Set(NumInfo.Default, amount);

    //GET THE THINGINFO OF THE KILLER
    ThingInfo killer = Who;

    //SET WHO IS THE KILLER'S NAME//
    DamageEvent.Set(killer); 

    // MAKES THE PLAYER TAKE DAMAGE
    Player.TakeEvent(DamageEvent, true);

    
    DeliverFatalBlow = true;
  
  }

}

}
