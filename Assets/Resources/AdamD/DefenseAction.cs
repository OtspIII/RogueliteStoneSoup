using UnityEngine;

public class DefendAction : ActionScript
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public DefendAction(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.DefendAction_AdamD, who);
        
        //if (who.CurrentHeld)
        MoveMult = 0; //if a defender class, is able to move at reduced spd instead, dmg ignored
        HaltMomentum = false; //maybe getting hit adds force; defender class can set it still
        Priority = 2; //if defender class, can't be status effected while defending
        Duration = 1;  //duration increases with Defender class, with CD reduced
    }
    public override void Begin()
    {
        base.Begin();
        Who.TakeEvent(God.E(EventTypes.GainTrait).Set(Traits.Barrier));
    }
    public override void End()
    {
        base.End();
        Who.TakeEvent(God.E(EventTypes.LoseTrait).Set(Traits.Barrier));
    }

    //Can't move. Reduce dmg taken/ignore dmg, has a cooldown to how long this action can be used. Can be interrupted by other actions like move
    //maybe next action if clicking launches a regular attack as a block counter
    //Game: Nuclear Throne
}
