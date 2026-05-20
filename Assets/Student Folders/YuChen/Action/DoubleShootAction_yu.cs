using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline.Actions;
using UnityEngine;


public class DoubleShootAction_yu : AttackAction
{

    private bool doubleshoot = false;
    private float time = 0f;

    public DoubleShootAction_yu(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Shoot, who);
        Duration = 0.5f;
        CanRotate = true;
        MoveMult = 0.5f;
        Anim = "Shoot";
    }

    public override void Begin()
    {
        base.Begin();
        DoubleS();
    }

    public override void OnRun()
    {
        base.OnRun();

        time += Time.time;

        if (Who.Target != null)
        {
            Who.Thing.LookAt(Who.Target,0.5f);
            Who.Thing.MoveTowards(Who.Target, Who.AttackRange);
        }

        if (!doubleshoot && time >= 0.5f )
        {
            DoubleS();
            doubleshoot = true;
        }
    }

    private void DoubleS()
    {
        ThingOption proj = GetHeld().Ask(EventTypes.GetProjectile).GetOption();
        Who.Thing.Shoot(proj);
    }

    public override void End()
    {
        base.End();
        doubleshoot = false; 
    }
}