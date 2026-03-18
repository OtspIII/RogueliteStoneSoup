
using System.Collections;
using UnityEngine;

public class TestAction1_Misha : ActionScript
{

    public MishaTestAction1Mindsets Mindset = MishaTestAction1Mindsets.Approach;
    public float waitTime = 0;
    public bool backing = true;
    
    public TestAction1_Misha(ThingInfo who,EventInfo e=null)
    {
        Setup(Actions.MishaTestAct1,who,true);
    }

    public override void OnRun()
    {
        ThingInfo targ = Who.Target;
        if (targ == null) return;
        if (Mindset == MishaTestAction1Mindsets.Attack)
        {
            Attacking(targ);
        }
        else if (Mindset == MishaTestAction1Mindsets.Wait)
        {
            Wait(targ);
        }
        else
        {
            Approach(targ);
        }
        
    }

    public enum MishaTestAction1Mindsets
    {
        Approach,
        Wait,
        Attack
    }

    public void Approach(ThingInfo targ)
    {
        MoveMult = 1;
        Who.Thing.MoveTowards(targ);
        Who.Thing.LookAt(targ);
        if (Who.Thing.Distance(targ) < Who.AttackRange + 1.5f)
        {
            Mindset = MishaTestAction1Mindsets.Wait;
            waitTime = Random.Range(0.2f, 0.6f);
        }
    }

    public void Wait(ThingInfo targ)
    {
        if (Who.Thing.Distance(targ) > Who.AttackRange + 3f)
        {
            Mindset = MishaTestAction1Mindsets.Approach;
            return;
        }
        else if (Who.Thing.Distance(targ) < Who.AttackRange)
        {
            Who.DoAction(Actions.DefaultAttack);
            Mindset = MishaTestAction1Mindsets.Approach;
            return;
        }
        MoveMult = backing ? -0.3f : 0.3f;
        Who.Thing.MoveTowards(targ);
        Who.Thing.LookAt(targ);
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {
            backing = !backing;
            waitTime = Random.Range(0.2f, 0.6f);
            if (God.CoinFlip(0.2f))
                Mindset = MishaTestAction1Mindsets.Attack;
        }
    }

    public void Attacking(ThingInfo targ)
    {
        MoveMult = 1;
        Who.Thing.MoveTowards(targ);
        Who.Thing.LookAt(targ);
        if (Who.Thing.Distance(targ) < Who.AttackRange)
        {
            Who.DoAction(Actions.DefaultAttack);
            Mindset = MishaTestAction1Mindsets.Approach;
        }
    }

}

public class TestAction2_Misha : ActionScript
{
    public TestAction2_Misha(ThingInfo who,EventInfo e=null)
    {
        //Setup(Actions.MishaTestAct1,who);
    }
}