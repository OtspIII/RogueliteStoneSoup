using System.Collections.Generic;
using UnityEngine;

public static class ActionParser 
{
    public static ActionScript GetAction(Actions act,ThingController who)
    {
        switch (act)
        {
            case Actions.Idle: return new IdleAction(who);
            case Actions.Stun: return new StunAction(who);
            case Actions.Swing: return new SwingAction(who);
            case Actions.Shoot: return new ShootAction(who);
            case Actions.Lunge: return new LungeAction(who);
            case Actions.Patrol: return new PatrolAction(who);
            case Actions.Chase: return new ChaseAction(who);
        }
        Debug.Log("UNCAUGHT ACTION: " + act);
        return new IdleAction(who);
    }
}


public enum Actions
{
    //The Basics
    None=0,
    Idle=1,
    Stun=2,
    
    //Attack Actions
    Swing=101,
    Lunge=102,
    Shoot=103,
    
    //AI Actions
    Patrol=201,
    Chase=202,
}

public enum ProjTypes
{
    None=0,
    Arrow=1,
}