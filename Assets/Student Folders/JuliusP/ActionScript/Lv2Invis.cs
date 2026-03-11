using UnityEngine;
using System.Collections;

public class Lv2Invis : ActionScript
{
    //STEPS FOR ACTIONS;

    // 1. Enemy teleports behind player
    // 2. Enemy prepares to strike
    // 3. Player hits stunning the enemy 
    // 4. Enemy cant take damage while stunned
    // 5. Enemy dissapeaes
    // 6. Enemy teleports to random location and is invisible when near it reappears and prepares to attack
    // 7. After two times stunned it fully attacks

    
    SpriteRenderer[] SRS;
    float timer = 0f;
    bool started = false;

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
        MoveMult = 0.5f;
        Duration = Mathf.Infinity;

        
    }

    public override void Begin()
    {
      Who.AddTrait(Traits.GainInvis_JuliusP);
    }

    public override void OnRun()
    {
        base.OnRun();

        

      MoveTowardPlayerWhileCloak();

    }



    void MoveTowardPlayerWhileCloak()
    {



    
      SRS = Who.Thing.gameObject.GetComponentsInChildren<SpriteRenderer>(true);
     
    
      ThingInfo Player = God.Session.Player;

    
      float Distance = Who.Thing.Distance(Player);


      if(Distance < 7f)
      {
            
        Debug.Log("Hi");

       // God.C(GraduallyDisappear(1f));

       Who.Thing.MoveTowards(Player);


      }
     
       
       
      // Who.DoAction(Actions.Chase);



    }



//FUNCTION THAT DECREASES ALPHA OF SPRITE//

IEnumerator GraduallyDisappear(float duration)
{
    float time = 0f;

    while (time < duration)
    {
        time += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, time / duration);

        foreach (SpriteRenderer sr in SRS)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }

        yield return null;
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