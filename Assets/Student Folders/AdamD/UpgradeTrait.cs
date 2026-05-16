using NUnit.Framework;
using UnityEngine;

public class UpgradeTrait : Trait
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //if sword, increase range
    //if axe, increase spd and size
    //if Bow, increase shootspeed/amount of projectiles spawned?
    //if wand, refresh its use+ projectiles home on targets, and cleanse/resist status effects on use
    //if staff, add knockback

    public int WeaponLv = 1;
    public UpgradeTrait()
    {
        Type = Traits.UpgradeTrait; 
        AddListen(EventTypes.OnPickup);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnPickup: 
                {
                    // on obtaining an item: check its tag, whether its consumable or a weapon. if it is
                    // if name_of_consumable/weapon == one of the items in your inventory/or if held, will upgrade the held weapon and destroy the picked up item,
                    // weaponlv for *that* item +1; and destroy the picked up item
                    //for each weapon type, add corresponding effects depending on its level:
                     
                    foreach (ThingInfo currentItem in God.Session.PlayerInventory)
                    {
                        if ( i.Who.Type== currentItem.Type)
                        {
                            if (i.Who == currentItem)
                            {
                                Debug.Log("passed by itself");
                                continue;
                            }
                            else
                            {
                                Debug.Log("WeaponUpgraded");
                                //WeaponLv+= i.Who.Get(Traits.UpgradeTrait).GetN(WeaponLv);
                                God.Session.RemoveInventory(currentItem); ;
                                Debug.Log("weapondestroyed");
                                //WeaponLv += currentItem.Type.UpgradeTrait;
                                break;
                            }
                        }


                    }

/*                    for (int i = 0; i < God.Session.PlayerInventory.Count; i++)
                    {
                        for (int j = i + 1; j < God.Session.PlayerInventory.Count; j++)
                        {
                            if (God.Session.PlayerInventory[i] == God.Session.PlayerInventory[j])
                            {
                                // Found a duplicate or matching pair
                            }
                        }
                    }*/


                    break;
                }
                //triggers whenever you obtain a new item:
                //has a specific item boost depending on which item is picked up; if forging slime picked up, increases the current weapon held by 2
                //special trade event room: OnInteract event or OnDrop, swap currently held item for a random item of that type (potion, consumable, weapon)
        }
    }
}
