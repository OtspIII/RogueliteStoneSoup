using UnityEngine;

public class Sidestep_qixiangdong : ActionScript
{
    private Vector2 sideDir;
    private float sideSpeedMult = 1.5f;
    private float sideDuration = 0.5f;

   

    public Sidestep_qixiangdong(ThingInfo who,EventInfo e = null){
        Setup(Actions.Sidestep_qixiangdong, who,true);

        MoveMult = sideSpeedMult;
        HaltMomentum =true;
        Duration = sideDuration;
        CanRotate = false;
        Priority = 2;
    }

    public override void Begin(){
        base.Begin();

        if (Who.Target == null || Who.Thing == null || Who.Target == null || Who.Target.Thing == null){
            Complete();
            return;
        }
        Vector2 myPos = Who.Thing.transform.position;
        Vector2 targetPos = Who.Target.Thing.transform.position;

        Vector2 toTarget = (targetPos - myPos).normalized;

        sideDir = new Vector2(-toTarget.y,toTarget.x);

        if(Random.value >0.5f){
            sideDir *= -1f;
        }
    }

    public override void OnRun(){
        base.OnRun();

        if (Who.Target == null || Who.Thing == null || Who.Target == null || Who.Target.Thing == null)
        {
            
            return;
        }
        Who.Thing.LookAt(Who.Target,0.5f);

        if (Who.Thing.Distance(Who.Target) <= Who.AttackRange)
        {
            Who.DoAction(Actions.DefaultAttack);
        }
    }
    public override void HandleMove(){
        if(Who.Thing == null || Who == null){
            return;
        }
        if(Who.Thing.RB == null){
            return;
        }

        Who.DesiredMove = sideDir;
        Who.Thing.ActualMove =MoveMult * Who.CurrentSpeed * sideDir;
    }
    


    public override void End(){
        Who.DesiredMove = Vector2.zero;
        base.End();
    }


    
}
