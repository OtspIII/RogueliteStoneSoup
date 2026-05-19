using UnityEngine;

public class StatusResist : Trait
{
    public StatusResist()
    {
        Type = Traits.StatusResist;
        //AddListen(EventTypes.Update); idk if this is needed
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {

        switch (e.Type) 
        {
            //either have it set the duration to 0, instantly remove the trait, or if possible, prevent trait from being applied
            case EventTypes.GainTrait: //This gets called each time you gain a new trait?
                {

                    Traits t = i.GetTrait();
                    if (t == Traits.Slowed_JuliusP || t== Traits.Bleed_MichaelT || t== Traits.Freeze_TracyH) { e.Abort = true; }   //rn resists on fire/specific traits. without out, other traits wouldn't apply either (Traits. is the enums;
                    i.TakeEvent(God.E(EventTypes.StartAction).Set(ActionInfo.Action, Actions.Idle));
                    Debug.Log("Status Effect:" + e + "resisted!");
                    break;
                }
        }
    }
}
