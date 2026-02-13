using System;
using UnityEngine;

public class ActorTrait : Trait
{
    public ActorTrait()
    {
        Type = Traits.Actor;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.OnSpawn);
        // AddListen(EventTypes.Start);
        AddListen(EventTypes.Update);
        AddListen(EventTypes.StartAction);
        AddListen(EventTypes.SetPhase);
        AddListen(EventTypes.GetCurrentAction);
        AddListen(EventTypes.GetDefaultAction);
        AddListen(EventTypes.Knockback);
        AddListen(EventTypes.OnTouch);
        AddListen(EventTypes.OnTouchEnd);
        AddListen(EventTypes.UseHeld);
        AddListen(EventTypes.UseHeldStart);
        AddListen(EventTypes.UseHeldEnd);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                i.Who.ActorTrait = i;
                if(i.Get(ActionInfo.DefaultAction) == Actions.None)
                    i.Set(ActionInfo.DefaultAction,Actions.Idle);
                break;
            }
            case EventTypes.OnSpawn:
            {
                float spd = i.Get(NumInfo.Speed,5);
                i.Who.CurrentSpeed = spd;
                if (spd > 0) i.Who.Thing.AddRB();
                // if(i.Who != God.Player)
                //     i.Who.Target = God.Player;
                DoAction(i);
                break;
            }
            case EventTypes.Update:
            {
                if(i.ActScript != null)
                    i.ActScript.Run();
                break;
            }
            case EventTypes.StartAction:
            {
                if(e.ActScript != null)
                    DoAction(i,e.ActScript,e);
                else
                {
                    Actions a = e.Get(ActionInfo.Action);
                    DoAction(i,a,e);
                }
                break;
            }
            case EventTypes.SetPhase:
            {
                int p = e.GetInt();
                i.Set(NumInfo.Phase, p);
                if(i.ActScript != null)
                    i.ActScript.ChangePhase(p);
                break;
            }
            case EventTypes.GetCurrentAction:
            {
                if (i.ActScript != null)
                {
                    e.ActScript = i.ActScript;
                    e.Set(i.ActScript.Type);
                }
                break;
            }
            case EventTypes.GetDefaultAction:
            {
                Actions r = i.Get(ActionInfo.DefaultAction);
                if (e.GetThing() != null)
                {
                    Actions ch = i.Get(ActionInfo.DefaultChaseAction);
                    if (ch != Actions.None) r = ch;
                }
                
                if (r != Actions.None)
                    e.Set(ActionInfo.DefaultAction,r);
                break;
            }
            case EventTypes.Knockback:
            {
                i.Who.Thing.Knockback = e.GetVector();
                break;
            }
            case EventTypes.OnTouch:
            {
                if (i.ActScript != null) i.ActScript.HitBegin(e.Collision);
                break;
            }
            case EventTypes.OnTouchEnd:
            {
                if (i.ActScript != null) i.ActScript.HitEnd(e.Collision);
                break;
            }
            case EventTypes.UseHeld:
            {
                if(i.Who.CurrentHeld != null) i.Who.CurrentHeld.TakeEvent(God.E(EventTypes.OnUse).Set(i.Who));
                break;
            }
            case EventTypes.UseHeldStart:
            {
                if(i.Who.CurrentHeld != null) i.Who.CurrentHeld.TakeEvent(God.E(EventTypes.OnUseStart).Set(i.Who));
                break;
            }
            case EventTypes.UseHeldEnd:
            {
                if(i.Who.CurrentHeld != null) i.Who.CurrentHeld.TakeEvent(God.E(EventTypes.OnUseEnd).Set(i.Who));
                break;
            }
        }
    }
    
    public virtual void DoAction(TraitInfo t, ActionScript a, EventInfo e=null)
    {
        if (t.Who.Thing == null) return;//Maybe you died so you can't do the action
        float prio = e != null ? e.Get(NumInfo.Priority, 1) : 1;
        if (t.ActScript != null)
        {
            if(!t.ActScript.TryInterrupt(a,prio)) return;
            else
            {
                t.ActScript.Abort(a);
            }
        }
        if (a == null) Debug.Log("ERROR: NULL ACTION / " + this);
        t.ActScript = a;
        t.Set(NumInfo.Phase, 0);
        if (t.ActScript != null)
        {
            t.ActScript.Begin();
            t.Who.Thing.DebugTxt = ""+t.ActScript;
        }
    }

    public virtual void DoAction(TraitInfo t, Actions a=Actions.None, EventInfo e=null)
    {
        if (t.Who.Thing == null) return;//Maybe you died so you can't do the action
        Actions defaultAct = t.Get(ActionInfo.DefaultAction);
        if (a == Actions.DefaultAttack)
        {
            if (t.Who.CurrentHeld == null)
            {
                God.LogWarning("Tried to do default attack with no held item: " + t.Who.Name + " ("+t.Who.Type.Author+")");
                return;
            }
            // Debug.Log(t.Who.Name + ": " + t.Who.CurrentWeapon);
            // Debug.Log(t.Who.CurrentWeapon.Trait);
            EventInfo atk = t.Who.CurrentHeld.Ask(EventTypes.GetDefaultAttack);
            a = atk.Get(ActionInfo.DefaultAction);
        }
        ActionScript act = Parser.Get(a == Actions.None ? defaultAct : a,t.Who);
        DoAction(t,act,e);
    }
}
