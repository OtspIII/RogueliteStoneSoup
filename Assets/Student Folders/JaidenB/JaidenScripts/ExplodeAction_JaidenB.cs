using UnityEngine;

public class ExplodeAction_JaidenB : ActionScript
{
    public ExplodeAction_JaidenB(ThingInfo who, EventInfo e = null) 
    {
       Setup(Actions.ExplodeAction_JaidenB, who, true);
    }

    public override void OnRun() // just copy and pasted from the movement script thingy, I want to learn but I'm having a lot of trouble
    {
        //base.OnRun();
        //Who.Thing.MoveTowards(Who.Target, Who.AttackRange);
        //Who.Thing.LookAt(Who.Target, 0.5f);

        //if ((Who.AttackRange <= 0.5f || Who.Thing.Distance(Who.Target) <= Who.AttackRange) && Who.Thing.IsFacing(Who.Target, 5))
        //    Who.DoAction(Actions.DefaultAttack);
     
        // Who.DoAction(Who.DefaultAttackAction());
    }




}
