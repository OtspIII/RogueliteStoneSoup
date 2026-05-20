using System;
using UnityEngine;

public class StartWithItemTrait : Trait
{
    public StartWithItemTrait()
    {
        Type = Traits.StartWithItem;
        AddListen(EventTypes.OnSpawn);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
            {
                if (i.Who == null) 
                    return;
                
                // loop over OptionInfo slots and give any ItemOptions found
                OptionInfo[] optionSlots = (OptionInfo[])Enum.GetValues(typeof(OptionInfo));
                foreach (OptionInfo oi in optionSlots)
                {
                    if (oi == OptionInfo.None) 
                        continue;
                    
                    ThingOption opt = i.GetOption(oi);
                    if (opt == null) 
                        continue; // nothing in this slot, try next

                    // if not an item, skip
                    ItemOption itemOption = opt as ItemOption;
                    if (itemOption == null) 
                        continue;

                    // give the thing
                    ThingInfo spawnedItem = itemOption.Create();
                    spawnedItem.Spawn(Vector3.zero);
                    i.Who.TakeEvent(God.E(EventTypes.DidPickup).Set(spawnedItem));

                    Debug.Log($"[StartWithItem] gave item {itemOption.Name} to {i.Who.Name}");
                }

                // remove trait so it only runs once
                i.Who.RemoveTrait(Type);
                break;
            }
        }
    }
}
