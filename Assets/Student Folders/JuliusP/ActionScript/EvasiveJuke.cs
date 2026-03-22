using UnityEngine;
using System.Collections.Generic;

public class EvasiveJuke : ActionScript
{
    float EvadeForce = 5f;

    float JukeTimerCooldwn = 0.34f;

    float JukeTimer;

    bool hasJuked = false;

    int timescanJuke;

    public EvasiveJuke(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.EvasiveJuke_JuliusP, who, true);

        HaltMomentum = true;
        MoveMult = 1f;
        Duration = Mathf.Infinity;
    }

    public override void Begin()
    {
        base.Begin();
        hasJuked = false;

        JukeTimer = 0;
    }

    public override void OnRun()
   {
    ThingInfo player = God.Session.Player;
    if (player == null) return;

    float dist = Who.Thing.Distance(player);

    JukeTimer += Time.deltaTime;

    // If far, chase
    if (dist > 3f)
    {
        Chase();
        return;
    }

    // If close,  juke
    if (JukeTimer >= JukeTimerCooldwn || dist <= 3f)
    {
        Juke();
        //timescanJuke++;
        JukeTimer = 0;
    }
    else
    {
        
        Chase(); 
    }

    // After enough jukes → attack
    if (timescanJuke > 3)
    {
        Who.Thing.DoAction(Actions.DefaultAttack);
    }
}
    // Juke movement
    void Juke()
    {
     ThingInfo player = God.Session.Player;
    if (player == null) return;



        float JukeChance = 0.71f;


        if(Random.value < JukeChance)
        {
            
            Who.Thing.TakeKnockback(player, EvadeForce * 5);
            //Who.Thing.DoAction(Actions.DefaultAttack);



        }

        else
        {
            
            Who.Thing.DoAction(Actions.Chase);

        }

   
 
    }
    
    // Chase player
    void Chase()
    {
    
        ThingInfo Player = God.Session.Player;

        ThingInfo Thing = Who;

        Who.Thing.LookAt(Player);

        Who.Thing.MoveTowards(Player);



        ThingInfo player = God.Session.Player;
    if (player == null) return;



        float JukeChance = 0.71f;


        if(Random.value < JukeChance)
        {
            
            Who.Thing.TakeKnockback(player, EvadeForce * 5);
            //Who.Thing.DoAction(Actions.DefaultAttack);

            



        }

        else
        {
            
            Who.Thing.DoAction(Actions.Chase);

        }



    

    
        
    }
}