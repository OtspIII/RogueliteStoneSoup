using UnityEngine;

public class RobAction_JaidenB : SwingAction
{
    public RobAction_JaidenB(ThingInfo who, EventInfo e = null) 
    {
       Setup(Actions.RobAction_JaidenB, who);
       Anim = "Swing";  
    }

   

    public override void HitBegin(GameCollision col)
    {
        base.HitBegin(col);

        //if (col.Other.TakeEvent(God.E(EventTypes.GetScore)) == 0)
        //{
            col.Other.TakeEvent(God.E(EventTypes.AddScore).Set(-1).Set(Who), true);
        //}
    }
    // RemoveInventory(ThingInfo) no idea what I'm going to do with this but I want to use this variable
    // Go to the attackaction and make it where you lose a point if hit with the hitend function, dont be afraid to copy code.
}
