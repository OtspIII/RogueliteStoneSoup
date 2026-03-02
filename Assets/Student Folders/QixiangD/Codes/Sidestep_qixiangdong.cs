/**using UnityEngine;

public class Sidestep_qixiangdong : ActionScript
{
    private Vector2 sideDir;
    private float sideSpeedMult = 2.5f;
    private float sideDuration = 0.5f;

    public Sidestep_qixiangdong(ThingInfo who,EventInfo e = null){
        Setup(Actions.Sidestep_qixiangdong, who);

        MoveMult = sideSpeedMult;
        HaltMomentum =true;
        Duration = sideDuration;
        CAnRotate = false;
        Prior = 2;
    }

    public override void Begin(){
        target = who.Target;

        if (target == null){
            Complete();
            return;
        }

        Vector2 toTarget = (Who.Target.Pos - Who.Pos).normalized;

        sideDir = new Vector2(-toTarget.y,toTarget.x);

        if(Random.value >0.5f){
            sideDir *= -1f;
        }
    }

    public override void HandleMove(){
        if(Who.Thing == null){
            return;
        }
        if(Who.Thing.RB = null){
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
**/