using Unity.VisualScripting;
using UnityEngine;

public class StatusEffectOnProjectileTrait_AdamD : Trait //Should make this "script" inherit from the "trait" class
{

    public StatusEffectOnProjectileTrait_AdamD() //I guess this is a constructor.. 
    {
        Type = Traits.statusEffectOnProjectile; //connects this trait/enum to the parser script, so "the class knows what enum it's attached to"
        AddListen(EventTypes.OnDestroy);//Makes it listen for that specific event, on destroy
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) //I guess this overrides something,
    //in which overriding is adding onto a function, or giving that function a new definition/actions, in this case..
    //"TakeEvent" which already has its own function(s) from other ppl?
    //i and e, i is supposed to be the user, while e is the event happening and from that, i/e has a ton of dot ops to figure out stuffs
    {
        base.TakeEvent(i, e);//no idea what this is, autocompleted it for me and don't know what a base is.

        switch (e.Type) //I guess this is supposed to be a switch statement. don't know what it exactly is... im guessing it just switches the 
            //e or something? like the type of event, dunno
        {
            case EventTypes.OnDestroy: //I guess when this event happens, do this in code?
                {
                    /*e.OnDestroy.
                    e.GetTrait.*/
                    break;
                }
            case EventTypes.OnTouch:
                {
                   /* e.Collision = e.GetTrait(OnFireTrait);*/
                    break;
                }
        }
    }


}
