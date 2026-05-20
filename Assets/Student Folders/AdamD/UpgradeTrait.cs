using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UpgradeTrait : Trait
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


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
                                //WeaponLv+= i.Who.Get(Traits.UpgradeTrait).GetN();
                                God.Session.RemoveInventory(currentItem); ;
                                Debug.Log("weapondestroyed");
                                //WeaponLv += currentItem.Type.UpgradeTrait;
                                break;
                            }
                        }
                    }

                    // GET PLAYER'S CURRENT WEAPON
                    ThingInfo currentWeapon = God.Session.Player.CurrentHeld;
                    ThingInfo weapon = God.Player.CurrentHeld;
                    string weaponName = weapon?.GetName(true)?.ToLower();
                    //ThingInfo spawnedItem = God.Session.Player.CurrentHeld.ChildOf; need to get the link of the "ToSpawn" item
                    if (WeaponLv == 2)
                    {
                        if (weaponName.Contains("Sword"))
                        {
                            //player speed increases, dmg increases
                            
                        }
                        if (weaponName.Contains("Bow"))
                        {
                            //gives a ricochet effect to the spawn arrows, and overrides its destroy function, increasing bounces+1. 

                        }
                        if (weaponName.Contains("Axe"))
                        {
                            //upon below 50% hp, greatly increases spd/dmg/aspd. Attacks also have a 25% chance to stun. Max hp+5
                            /*if (God.Player.
                            i.Who.AddTrait(Traits.);*/
                        }
                        if (weaponName.Contains("Staff"))
                        {
                            //add knockback to explosions
                        }
                        if (weaponName.Contains("Wand"))
                        {
                            //attacks home onto the target
                            //i.Who.Spawn().AddTrait(Traits.Homing_TracyH);
                        }
                        if (weaponName.Contains("Potion"))
                        {
                            //on use, adds status cleanse. hp restored+1
                            
                            i.Who.AddTrait(Traits.StatusResist); //should add it onto the potion rather than the player..
                        }
                    }
                    else if (WeaponLv == 3)
                    {
                        if (weaponName.Contains("Sword"))
                        {
                            //50% chance to resist bleed/status effects, attacks hit twice/deal double dmg
                            
                        }
                        if (weaponName.Contains("Bow"))
                        {
                            //increases bounces by 1, adds a bleed effect to arrows
                        }
                        if (weaponName.Contains("Axe"))
                        {
                            //gives lifesteal on attacks
                        }
                        if (weaponName.Contains("Staff"))
                        {
                            //aoe increases greatly
                        }
                        if (weaponName.Contains("Wand"))
                        {
                            //increases speed greatly on using for X seconds. 
                        }
                        if (weaponName.Contains("Potion"))
                        {
                            //on use, adds speed up, hp+1
                        }
                    }
                    else if (WeaponLv == 4)
                    {
                        if (weaponName.Contains("Sword"))
                        {
                            //if hp is above 70%, teleports behind the enemy target and attacks deal triple dmg
                        }
                        if (weaponName.Contains("Bow"))
                        {
                            //shoots 2 additional arrows in a spreadshot
                        }
                        if (weaponName.Contains("Axe"))
                        {
                            //greatly increases speed of player and size of weapon, and aspd
                        }
                        if (weaponName.Contains("Staff"))
                        {
                            //while equiped, ignores status effects
                        }
                        if (weaponName.Contains("Wand"))
                        {
                            //base player attack +1 on each use
                        }
                        if (weaponName.Contains("Potion"))
                        {
                            //on use, increase max hp on character by 1, +hp1, gains hp regen for 3 secs
                            float healInterval = 3;
                            healInterval -= Time.deltaTime;
                            if (healInterval < 0)
                            {
                                i.Who.TakeEvent(God.E(EventTypes.Damage).Set(-1).Set(i.Who)); //should heal once it is active
                                healInterval = 1;
                            }

                        }
                    }
                    else
                    {
                        for (int num = 0; num <= WeaponLv; num++)
                        {
                            //increase dmg and aspd by 1 factor.
                            if (weaponName.Contains("Potion"))
                            {
                                //regen duration +4 secs
                            }
                            if (weaponName.Contains("Axe"))
                            {
                                //maxhp+1
                            }
                            if (weaponName.Contains("Staff"))
                            {
                                //has a 10% chance to ignore dmg, double chance if dmg comes from explosion, 
                            }
                            if (weaponName.Contains("Wand"))
                            {
                                //takes -1 dmg from explosions from any source
                            }
                            if (weaponName.Contains("Bow"))
                            {
                                //bounce +1
                            }
                            if (weaponName.Contains("Sword"))
                            {
                                //+1 multiplier to the dmg
                            }
                        }
                    }

                 break;
                }
                //Other ideas:
                //triggers whenever you obtain a new item:
                //has a specific item boost depending on which item is picked up; if forging slime picked up, increases the current weapon held by 2
                //special trade event room: OnInteract event or OnDrop, swap currently held item for a random item of that type (potion, consumable, weapon)
        }
    }
}
/*                        if (i.Who.Name == "")
                        {
                            i.Who.Thing.gameObject.transform.localScale += new Vector3 (1, 1, 0); //increases size
                        }
                        else if (i.Who.Name ==)
i.Who.GetName()==""

 */