using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HoldableOption", menuName = "Scriptable Objects/HoldableOption")]
public class HoldableOption : ThingOption
{
    public float Damage;
    public Actions DefaultAttack = Actions.Swing;
    public ThingOption Projectile;

    // private void OnValidate()
    // {
    //     if(!Tags.Contains(GameTags.Weapon))
    //         Tags.Add(GameTags.Weapon);
    // }

    public override ThingInfo Create()
    {
        ThingInfo r = base.Create();
        TraitInfo h = r.AddTrait(Traits.Holdable,God.E().Set(EnumInfo.DefaultAction,(int)DefaultAttack).Set(NumInfo.Amount,Damage));
        if (Projectile != null) h.Set(Projectile); 
        return r;
    }
}
