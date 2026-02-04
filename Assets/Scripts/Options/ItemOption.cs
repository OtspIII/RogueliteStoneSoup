using System;
using System.Collections.Generic;
using UnityEngine;

///A type of Option that auto-adds the Pickupable trait, and lets you pick an action to give it the Tool trait
[CreateAssetMenu(fileName = "HoldableOption", menuName = "Scriptable Objects/ItemOption")]
public class ItemOption : ThingOption
{
    public Actions DefaultAttack = Actions.None;  //What action do you take when you use this item? If none, not equippable
    public SpawnRequest ToSpawn;      //Is there an item using this action spawns?
    public BodyController FloorBody;  //Holdable items can have different bodies on the ground vs when held. This is their floor body
    public Sprite FloorArt;           //Overrides the art of their floor body, like Art does for the main body
    public float Damage;              //Does the item do damage? If so, it does this much.
    public float IdealRange = 1.5f;   //What distance does a NPC try to get from their target before using this item?
    public float SpeedMult = 1f;      //Changes the speed of the item's animation when used. Smaller numbers are slower animations
    public float ActDuration = 0f;    //If not set to 0, makes the action that comes from this tool take this long--overrides animation as length

    //Runs ThingOption.Create and then adds a few extra traits/etc onto the ThingInfo to make it an item
    public override ThingInfo Create()
    {
        ThingInfo r = base.Create();
        r.AddTrait(Traits.Pickupable);
        if (DefaultAttack != Actions.None)
        {
            TraitInfo h = r.AddTrait(Traits.Tool,
                God.E().Set(ActionInfo.DefaultAction, DefaultAttack)
                    .Set(NumInfo.Default, Damage)
                    .Set(NumInfo.Speed, SpeedMult)
                    .Set(NumInfo.Max, ActDuration)
                    .Set(NumInfo.Distance,IdealRange));
            h.Set(ToSpawn);
        }
        return r;
    }

    ///Because tools have two bodies--one when held and one on the ground--we need fancy functions for finding which
    public override BodyController GetBody(bool held = false)
    {
        if (held) return Body;
        return FloorBody != null ? FloorBody : God.Library.PickupDefaultBody;
    }
    public override Sprite GetArt(bool held = false)
    {
        if (held) return Art;
        return FloorArt != null ? FloorArt : Art;
    }
}
