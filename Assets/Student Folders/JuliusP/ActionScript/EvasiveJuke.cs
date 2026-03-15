using UnityEngine;
using System.Collections.Generic;

public class EvasiveJuke : ActionScript
{
    float EvadeForce = 5.6f;

    float JukeTimerCooldwn = 0.94f;

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
    
        // If close to player → juke
        if (dist < 3f && JukeTimer >= JukeTimerCooldwn)
        {
            Juke();
            timescanJuke++;
            JukeTimer = 0;
        }

        else if(timescanJuke > 3)
        {
            
            Who.Thing.DoAction(Actions.DefaultAttack);

        }


        else if(dist > 2.2f)
        {
            Chase();

        }
       


    }

    // Juke movement
    void Juke()
    {
        ThingInfo player = God.Session.Player;

        if (player == null) return;

        // Push away from player
        Who.Thing.TakeKnockback(player, EvadeForce * 5);

        // After juking → attack
       // Who.Thing.DoAction(Actions.DefaultAttack);
    }

    // Chase player
    void Chase()
    {
    
        ThingInfo Player = God.Session.Player;

        Who.Thing.LookAt(Player);

        Who.Thing.MoveTowards(Player);
        
    }
}