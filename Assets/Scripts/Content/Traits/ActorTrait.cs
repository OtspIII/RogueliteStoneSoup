using System;
using UnityEngine;

public class ActorTrait : Trait
{
    public ActorTrait()
    {
        Type = Traits.Actor;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.Start);
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
                if(i.Get<Actions>(EnumInfo.DefaultAction) == Actions.None)
                    i.Set(EnumInfo.DefaultAction,(int)Actions.Idle);
                float spd = i.Get(NumInfo.Speed,5);
                i.Who.CurrentSpeed = spd;
                if (spd > 0) i.Who.Thing.AddRB();
                break;
            }
            case EventTypes.Start:
            {
                if(i.Who != God.Player)
                    i.Who.Target = God.Player;
                DoAction(i);
                break;
            }
            case EventTypes.Update:
            {
                if(i.Action != null)
                    i.Action.Run();
                break;
            }
            case EventTypes.StartAction:
            {
                if(e.Action != null)
                    DoAction(i,e.Action);
                else
                {
                    Actions a = e.Get<Actions>(EnumInfo.Action);
                    DoAction(i,a);
                }
                break;
            }
            case EventTypes.SetPhase:
            {
                if(i.Action != null)
                    i.Action.ChangePhase(e.GetInt());
                break;
            }
            case EventTypes.GetCurrentAction:
            {
                if (i.Action != null)
                    e.Action = i.Action;
                break;
            }
            case EventTypes.GetDefaultAction:
            {
                Actions r = i.Get<Actions>(EnumInfo.DefaultAction);
                if (r != Actions.None)
                    e.Set(EnumInfo.DefaultAction,(int)r);
                break;
            }
            case EventTypes.Knockback:
            {
                i.Who.Knockback = e.GetVector();
                break;
            }
            case EventTypes.OnTouch:
            {
                if (i.Action != null) i.Action.HitBegin(e.Collision);
                break;
            }
            case EventTypes.OnTouchEnd:
            {
                if (i.Action != null) i.Action.HitEnd(e.Collision);
                break;
            }
            case EventTypes.UseHeld:
            {
                if(i.Who.CurrentWeapon != null) i.Who.CurrentWeapon.TakeEvent(God.E(EventTypes.OnUse).Set(i.Who));
                break;
            }
            case EventTypes.UseHeldStart:
            {
                if(i.Who.CurrentWeapon != null) i.Who.CurrentWeapon.TakeEvent(God.E(EventTypes.OnUseStart).Set(i.Who));
                break;
            }
            case EventTypes.UseHeldEnd:
            {
                if(i.Who.CurrentWeapon != null) i.Who.CurrentWeapon.TakeEvent(God.E(EventTypes.OnUseEnd).Set(i.Who));
                break;
            }
        }
    }
    
    public virtual void DoAction(TraitInfo t, ActionScript a, Infos i=null)
    {
        float prio = i != null ? i.Get(FloatI.Priority, 1) : 1;
        if (t.Action != null)
        {
            if(!t.Action.TryInterrupt(a,prio)) return;
            else
            {
                t.Action.Abort(a);
            }
        }
        if (a == null) Debug.Log("ERROR: NULL ACTION / " + this);
        t.Action = a;
        if (t.Action != null)
        {
            t.Action.Begin();
            t.Who.Thing.DebugTxt = ""+t.Action;
        }
    }

    public virtual void DoAction(TraitInfo t, Actions a=Actions.None, Infos i=null)
    {
        Actions defaultAct = t.Get<Actions>(EnumInfo.DefaultAction);
        if (a == Actions.DefaultAttack)
        {
            // Debug.Log(t.Who.Name + ": " + t.Who.CurrentWeapon);
            // Debug.Log(t.Who.CurrentWeapon.Trait);
            EventInfo e = t.Who.CurrentWeapon.Ask(EventTypes.GetDefaultAttack);
            a = e.Get<Actions>(EnumInfo.DefaultAction);
        }
        ActionScript act = ActionParser.GetAction(a == Actions.None ? defaultAct : a,t.Who.Thing);
        DoAction(t,act,i);
    }
}
