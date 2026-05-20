using UnityEngine;
using System.Collections;

public class RageTrait : Trait
{
    //BOOL FOR TRACJING RAGE//
    private bool rageActive = false;

    private bool OnlyOnce = false;

    // STORE ORIGINAL DAMAGE
    private float storedOriginalDamage = -1f;
    private ThingInfo storedWeapon = null;

    public RageTrait()
    {
        // SETS THE TRAIT TYPE TO RAGE
        Type = Traits.Rage;

        //DMG MULTIPLIER EVENT//
        AddListen(EventTypes.DamageMult);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.DamageMult:

                // GETS THE ATTACKER WHO IS CAUSING THE DAMAGE
                ThingInfo attacker = e.GetThing();

                // RETURN IF ATTACKER IS NULL OR DOSENT EXIST
                if (attacker == null) break;

                // THIS GETS THE ATTACKER'S HEALTH TRAIT TO CHECK//
                TraitInfo health = attacker.Get(Traits.Health);

                // THIS WILL RETURN IF HEALTH TRAIT DOES NOT EXIST
                if (health == null) break;

                // GET THE ATTACKER THING'S CURRENT HP
                float current = health.GetN();

                // GET THE ATTACKER THING'S MAX HP
                float max = health.Get(NumInfo.Max);

                ThingInfo weapon = attacker.CurrentHeld;

                // RESET WHEN ABOVE 50%
                if (current / max > 0.5f)
                {
                    if (rageActive && storedWeapon != null)
                    {
                        TraitInfo tool = storedWeapon.Get(Traits.Tool);
                        if (tool != null && storedOriginalDamage > 0)
                        {
                            tool.Set(storedOriginalDamage);
                            //Debug.Log("Rage ended, damage reset");
                        }
                    }

                    rageActive = false;
                    storedWeapon = null;
                    storedOriginalDamage = -1f;
                    break;
                }

                // APPLY RAGE WHEN BELOW 50%
                if (current / max <= 0.5f && !rageActive)
                {
                  //  Debug.Log("Health is below 50%");

                    if (weapon == null) break;

                    TraitInfo tool = weapon.Get(Traits.Tool);
                    if (tool == null) break;

                    storedWeapon = weapon;
                    storedOriginalDamage = tool.GetN();

                    tool.Set(storedOriginalDamage * 2f);

                    rageActive = true;

                    //Debug.Log("Rage damage boost applied!");
                }

                break;
        }
    }
}