using System.Collections.Generic;
using UnityEngine;

public static class ActionParser 
{
    public static ActionScript GetAction(Actions act,ThingController who,EventInfo e=null)
    {
        switch (act)
        {
            case Actions.Idle: return new IdleAction(who,e);
            case Actions.Stun: return new StunAction(who,e);
            case Actions.Swing: return new SwingAction(who,e);
            case Actions.Shoot: return new ShootAction(who,e);
            case Actions.Lunge: return new LungeAction(who,e);
            case Actions.Patrol: return new PatrolAction(who,e);
            case Actions.Chase: return new ChaseAction(who,e);
            case Actions.Use: return new UseAction(who, e);
        }
        Debug.Log("UNCAUGHT ACTION: " + act);
        return new IdleAction(who,e);
    }
}


public enum Actions
{
    //The Basics
    None=0,
    Idle=1,
    Stun=2,
    DefaultAction=3,
    DefaultAttack=4,
    Use=5,
    
    //Attack Actions
    Swing=101,
    Lunge=102,
    Shoot=103,
    
    //AI Actions
    Patrol=201,
    Chase=202,
    
    //Misc Actions
    // SelfDestruct=301,
}

public enum ProjTypes
{
    None=0,
    Arrow=1,
}