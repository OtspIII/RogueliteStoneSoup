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

    public void Setup(Actions type, ThingController who,bool isIdle=false)
    {
        Type = type;
        Who = who;
        Trait = who.ActorTrait;
        if (isIdle)
        {
            Priority = 0;
            MoveMult = 1;
            CanRotate = true;
        }
    }
    
    public void Run()
    {
        OnRun();
        HandleMove();
        if (Duration > 0)
        {
            Timer+=Time.deltaTime;
            if(Timer >= Duration)
                Complete();
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
        EventInfo sp = Who.Ask(EventTypes.GetActSpeed,true);
        float speedMult = sp.GetFloat(NumInfo.Amount,1);
        Duration = Mathf.Max(Duration,sp.GetFloat(NumInfo.Max,0));
        if (Anim != "")
        {
            Duration = Mathf.Max(Duration,Who.PlayAnim(Anim,speedMult));
            // foreach(AnimationClip c in Who.Body.Anim.runtimeAnimatorController.animationClips)
            //     if (c.name == Anim)
            //         Duration = c.length * Who.Body.Anim.speed;
            
        }
        else
            Who.PlayAnim();
        ChangePhase(0);
    }
    
    //Runs no matter what when the action ends
    public virtual void End()
    {
        Trait.ActScript = null;
        Who.DoAction(NextAction());
        if (Coro != null)
        {
            Who.StopCoroutine(Coro);
            Coro = null;
        }
    }

    //Runs at end if not interrupted
    public virtual void Complete()
    {
        if(Who != null) End();
    }
    
    //Runs at end if action interrupted by a higher priority action
    public virtual void Abort(ActionScript newAct)
    {
        if(Who != null) End();
    }

    public bool TryInterrupt(ActionScript newAct, float prio=-1)
    {
        if (prio < 0) prio = newAct.Priority;
        return Priority < prio;
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
        return Who.Ask(God.E(EventTypes.GetDefaultAction).Set(Who.Target)).Get(ActionInfo.DefaultAction);
    }

    public virtual void ChangePhase(int newPhase)
    {
        Phase = newPhase;
    }

    public virtual void HitBegin(GameCollision col)
    {
        
    }
    
    public virtual void HitEnd(GameCollision col)
    {
        
    }
    
    public virtual ThingInfo GetWeapon()
    {
        return Who.CurrentWeapon;
    }

    public float Perc()
    {
        if (Duration <= 0) return 0;
        return Timer / Duration;
    }
}
