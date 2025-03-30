using System;
using UnityEngine;

public class ActorTrait : Trait
{
    public ActorTrait()
    {
        Type = Traits.Actor;
        TakeListen.Add(EventTypes.Setup);
        TakeListen.Add(EventTypes.Start);
        TakeListen.Add(EventTypes.Update);
        TakeListen.Add(EventTypes.StartAction);
        TakeListen.Add(EventTypes.SetPhase);
        TakeListen.Add(EventTypes.GetCurrentAction);
        TakeListen.Add(EventTypes.GetDefaultAction);
        TakeListen.Add(EventTypes.Knockback);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                i.Who.ActorTrait = i;
                if(i.Get<Actions>(EnumInfo.DefaultAction) == Actions.None)
                    i.Set(EnumInfo.DefaultAction,(int)Actions.Patrol);
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
        }
    }
    
    public virtual void DoAction(TraitInfo t, ActionScript a, Infos i=null)
    {
        float prio = i != null ? i.Get(FloatI.Priority, 1) : 1;
        if (t.Action != null && t.Action.Priority >= prio) return;
        if (a == null) Debug.Log("ERROR: NULL ACTION / " + this);
        t.Action = a;
        if (t.Action != null)
        {
            t.Action.Begin();
        }
    }

    public virtual void DoAction(TraitInfo t, Actions a=Actions.None, Infos i=null)
    {
        Actions defaultAct = t.Get<Actions>(EnumInfo.DefaultAction);
        if (a == Actions.DefaultAttack)
        {
            EventInfo e =TraitManager.Get(t.Who.CurrentWeapon.Trait).Ask(t.Who.CurrentWeapon, EventTypes.GetDefaultAttack);
            a = e.Get<Actions>(EnumInfo.DefaultAction);
        }
        ActionScript act = ActionParser.GetAction(a == Actions.None ? defaultAct : a,t.Who);
        DoAction(t,act,i);
    }
}
