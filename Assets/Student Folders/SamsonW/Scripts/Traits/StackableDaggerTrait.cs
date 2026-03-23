using UnityEngine;

public class StackableDaggerTrait : Trait
{
    public StackableDaggerTrait()
    {
        Type = Traits.StackableDagger;
        AddListen(EventTypes.DamageMult);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.DamageMult:
                ThingInfo user = e.GetThing();
                if (user == null)
                    return;
                
                ThingInfo currentWeapon = user.CurrentHeld;
                if (currentWeapon == null)
                    return;

                TraitInfo currentWeaponTool = currentWeapon.Get(Traits.Tool);
                if (currentWeaponTool == null)
                    return;
                
                TraitInfo stackableInfo = currentWeapon.Get(Traits.Stackable);
                if (stackableInfo == null)
                    return;
                
                int stacks = stackableInfo.GetInt();
                if (stacks <= 0)
                    return;

                // If original damage not stored yet, store
                if (!i.GetBool())
                {
                    i.Set(currentWeaponTool.GetN());
                    i.Set(true); // So that original damage is only stored once
                }
                
                float finalDamage = i.GetN() * stacks; // Original damage multiplied by stacks
                currentWeaponTool.Set(finalDamage);
                // Debug.Log($"Damage to {finalDamage}x");
                break;
        }
    }
}