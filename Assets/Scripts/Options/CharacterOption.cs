using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterOption", menuName = "Scriptable Objects/CharacterOption")]
public class CharacterOption : ThingOption
{
    public int HP;
    public float Speed = 5;
    public HoldableOption Weapon;
    public Actions DefaultAction=Actions.Patrol;

    public override ThingInfo Create()
    {
        ThingInfo r = base.Create();
        r.AddTrait(Traits.Actor,God.E().Set(NumInfo.Speed,Speed).SetEnum(EnumInfo.DefaultAction,(int)DefaultAction));
        r.AddTrait(Traits.Health, new EventInfo(HP));
        r.SetWeapon(Weapon);
        return r;
    }
}