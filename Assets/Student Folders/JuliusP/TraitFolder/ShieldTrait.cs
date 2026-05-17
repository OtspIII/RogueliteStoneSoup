using UnityEngine;

public class ShieldTrait : Trait
{
    public ShieldTrait()
    {
        Type = Traits.ShieldTrait_JuliusP;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnTouch:
            {
                GameCollision col = e.Collision;
                if (col == null || col.Other == null)
                    break;

                ThingInfo other = col.Other.Info;
                if (other == null)
                    break;

                ThingInfo attacker = other;

                ThingInfo weapon = attacker.CurrentHeld;
                if (weapon == null && attacker.ChildOf != null)
                    weapon = attacker.ChildOf.CurrentHeld;

                string weaponName = weapon?.GetName(true);
                string hitName = other.Name;

                if (weaponName != null)
                    weaponName = weaponName.ToLower();

                if (hitName != null)
                    hitName = hitName.ToLower();

                // ONLY BOW / ARROW CHECK
                if ((weaponName != null && weaponName.Contains("wolfbane bow")) || (hitName != null && hitName.Contains("lv2.wolfbanearrows")))
                {
                    Debug.Log("Wolfbane arrow hit shield");

                    i.Who.Destruct(i.Who);
                    break;
                }

                // SWORD CASE → GIVE IMMUNITY
                if (weaponName != null && weaponName.Contains("sword"))
                {
                    Debug.Log("Sword hit shield → ignoring damage");

                    i.Who.AddTrait(Traits.IgnoreDamage_JuliusP);
                    break;
                }

                break;
            }
        }
    }
}