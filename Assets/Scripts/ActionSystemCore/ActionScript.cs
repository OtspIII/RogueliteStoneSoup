using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionScript
{
    public Actions Type;
    public ActorController Who;
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
        Who.RB.linearVelocity = (MoveMult * Who.Stats.Speed * Who.DesiredMove.normalized) + Who.Knockback;
    }

    public virtual void Begin()
    {
        Reset();
        if(HaltMomentum && Who.RB != null) Who.RB.linearVelocity = Vector2.zero;
        Coro = Who.StartCoroutine(Script());
        if (Anim != "")
        {
            Who.PlayAnim(Anim);
            foreach(AnimationClip c in Who.Body.Anim.runtimeAnimatorController.animationClips)
                if (c.name == Anim)
                    Duration = c.length * Who.Body.Anim.speed;
            
        }
        else
            Who.PlayAnim(Who.DefaultAnim);
        ChangePhase(0);
    }
    
    public virtual void End()
    {
        Who.CurrentAction = null;
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
        return Who.DefaultAction;
    }

    public virtual void ChangePhase(int newPhase)
    {
        Phase = newPhase;
    }

    public virtual void HitBegin(ActorController hit, HurtboxController box)
    {
        
    }
    
    public virtual void HitEnd(ActorController hit, HurtboxController box)
    {
        
    }

    public float Perc()
    {
        if (Duration <= 0) return 0;
        return Timer / Duration;
    }
}
