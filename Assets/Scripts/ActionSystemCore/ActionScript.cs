using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScript
{
    public Actions Type;
    public ThingController Who;
    public TraitInfo Trait;
    public float MoveMult = 0;
    public bool HaltMomentum = true;
    public Coroutine Coro;
    public float Priority = 1;
    public string Anim="";
    public float Duration;
    public float Timer;
    public bool CanRotate = false;
    public int Phase;

    protected void BeIdleAction()
    {
        Priority = 0;
        MoveMult = 1;
        CanRotate = true;
    }
    
    public void Run()
    {
        OnRun();
        HandleMove();
        if (Duration > 0)
        {
            Timer+=Time.deltaTime;
            if(Timer >= Duration)
                End();
        }
    }

    public virtual void OnRun()
    {
        
    }

    public virtual void Reset()
    {
        Timer = 0;
    }
    
    public virtual void HandleMove()
    {
        if (Who.RB == null) return;
        Who.RB.linearVelocity = (MoveMult * Who.CurrentSpeed * Who.DesiredMove.normalized) + Who.Knockback;
    }

    public virtual void Begin()
    {
        Reset();
        if(HaltMomentum && Who.RB != null) Who.RB.linearVelocity = Vector2.zero;
        Coro = Who.StartCoroutine(Script());
        if (Anim != "")
        {
            Duration = Who.PlayAnim(Anim);
            // foreach(AnimationClip c in Who.Body.Anim.runtimeAnimatorController.animationClips)
            //     if (c.name == Anim)
            //         Duration = c.length * Who.Body.Anim.speed;
            
        }
        else
            Who.PlayAnim();
        ChangePhase(0);
    }
    
    public virtual void End()
    {
        Trait.Action = null;
        Who.DoAction(NextAction());
        if (Coro != null)
        {
            Who.StopCoroutine(Coro);
            Coro = null;
        }
    }

    public virtual IEnumerator Script()
    {
        yield return null;
    }

    public virtual Actions NextAction()
    {
        // EventInfo e = God.E(EventTypes.GetDefaultAction);
        // Who.TakeEvent(e);
        // return Who.DefaultAction;
        return Who.Ask(EventTypes.GetDefaultAction).Get<Actions>(EnumInfo.DefaultAction);
    }

    public virtual void ChangePhase(int newPhase)
    {
        Phase = newPhase;
    }

    public virtual void HitBegin(ThingController hit, HurtboxController box)
    {
        
    }
    
    public virtual void HitEnd(ThingController hit, HurtboxController box)
    {
        
    }
    
    public virtual TraitInfo GetWeapon()
    {
        return Who.CurrentWeapon;
    }

    public float Perc()
    {
        if (Duration <= 0) return 0;
        return Timer / Duration;
    }
}
