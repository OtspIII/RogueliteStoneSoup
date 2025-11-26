using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterOption", menuName = "Scriptable Objects/CharacterOption")]
public class CharacterOption : ThingOption
{
    public int HP;
    public float Speed = 5;
    public string Weapon;

    public override void Imprint(ThingController r)
    {
        base.Imprint(r);
        r.AddTrait(Traits.Actor,God.E().Set(NumInfo.Speed,Speed));
        r.AddTrait(Traits.Health, new EventInfo(HP));
        r.SetWeapon(Weapon);
    }
}