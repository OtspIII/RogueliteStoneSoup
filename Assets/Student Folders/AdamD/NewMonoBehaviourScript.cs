using Unity.VisualScripting;
using UnityEngine;

public class StatusEffectOnProjectileTrait_AdamD : Trait //Should make this "script" inherit from the "trait" class
{

    public StatusEffectOnProjectileTrait_AdamD() //I guess this is a constructor.. 
    {
        Type = Traits.statusEffectOnProjectile; //connects this trait/enum to the parser script, so "the class knows what enum it's attached to"
        AddListen(EventTypes.OnDestroy);//Makes it listen for that specific event, on destroy
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) //I guess this overrides something,
    //in which overriding is adding onto a function, or giving that function a new definition/actions, in this case..
    //"TakeEvent" which already has its own function(s) from other ppl?
    //Also I do not know what the arguments i and e are for. i guess it's just a name for the variables we use in the functions?
    {
        base.TakeEvent(i, e);//no idea what this is, autocompleted it for me and don't know what a base is.

        switch (e.Type) //I guess this is supposed to be a switch statement. don't know what it exactly is... im guessing it just switches the 
            //e or something? like the type of event, dunno
        {
        /*    case EventTypes.OnDestroy: //I guess when this event happens, do this in code?
                {
                    
                }*/
        }
    }


}
