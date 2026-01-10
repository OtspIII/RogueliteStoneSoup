using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HoldableOption", menuName = "Scriptable Objects/HoldableOption")]
public class HoldableOption : ThingOption
{
    public float Damage;
    public Actions DefaultAttack = Actions.Swing;
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
        TraitInfo h = r.AddTrait(Traits.Holdable,God.E().Set(EnumInfo.DefaultAction,(int)DefaultAttack).Set(NumInfo.Amount,Damage));
        r.AddTrait(Traits.Pickup);
        if (Projectile != null) h.Set(Projectile); 
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
