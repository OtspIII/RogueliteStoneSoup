using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterOption", menuName = "Scriptable Objects/CharacterOption")]
public class CharacterOption : ThingOption
{
    public int HP;
    public string Weapon;

    public override void Imprint(ThingController r)
    {
        base.Imprint(r);
        r.AddTrait(Traits.Actor);
        r.AddTrait(Traits.Health, new EventInfo(HP));
        r.SetWeapon(Weapon);
    }
}