using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

///A type of Option that auto-adds the Actor and Health traits, along with some combat stats
[CreateAssetMenu(fileName = "CharacterOption", menuName = "Scriptable Objects/CharacterOption")]
public class CharacterOption : ThingOption
{
    public int HP;           //How much HP does the character have?
    public float Speed = 5;  //How fast do they walk?
    public ItemOption Held;//What are they holding? Determines their default attack/action
    public Actions DefaultAction=Actions.Patrol;     //What do they do whenever they aren't doing any other action?
    public Actions DefaultChaseAction=Actions.Chase; //What do they do once they see the player?

    //Runs ThingOption.Create and then adds a few extra traits/etc onto the ThingInfo to make it a Character
    public override ThingInfo Create()
    {
        ThingInfo r = base.Create();
        r.AddTrait(Traits.Actor,God.E().Set(NumInfo.Speed,Speed).SetAction(ActionInfo.DefaultAction,DefaultAction)
            .SetAction(ActionInfo.DefaultChaseAction,DefaultChaseAction));
        r.AddTrait(Traits.Health, new EventInfo(HP));
        r.SetHeld(Held);
        return r;
    }
}