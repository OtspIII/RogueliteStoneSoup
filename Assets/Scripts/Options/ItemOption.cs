using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HoldableOption", menuName = "Scriptable Objects/ItemOption")]
public class ItemOption : ThingOption
{
    public float Damage;
    public float IdealRange = 1.5f;
    public float SpeedMult = 1f;
    public float ActDuration = 0f;
    public Actions DefaultAttack = Actions.None;
    public ThingOption Projectile;
    public BodyController FloorBody;
    public Sprite FloorArt;

    // private void OnValidate()
    // {
    //     if(!Tags.Contains(GameTags.Weapon))
    //         Tags.Add(GameTags.Weapon);
    // }

    

    public override ThingInfo Create()
    {
        ThingInfo r = base.Create();
        r.AddTrait(Traits.Pickupable);
        if (DefaultAttack != Actions.None)
        {
            TraitInfo h = r.AddTrait(Traits.Tool,
                God.E().Set(ActionInfo.DefaultAction, DefaultAttack)
                    .Set(NumInfo.Amount, Damage)
                    .Set(NumInfo.Speed, SpeedMult)
                    .Set(NumInfo.Max, ActDuration)
                    .Set(NumInfo.Distance,IdealRange));
            if (Projectile != null) h.Set(Projectile);
        }
        return r;
    }

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
