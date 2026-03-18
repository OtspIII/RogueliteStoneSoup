using UnityEngine;
using System.Collections.Generic;

public class Lighting_RaphaelC : Trait
{
    public Lighting_RaphaelC()
    {
        Type = Traits.Lighting_RaphaelC;
        AddListen(EventTypes.OnUseStart);
        //AddListen(EventTypes.OnTargetDie);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type == EventTypes.OnUseStart)
        {
            ThingInfo player = i.Who;
            if (player.Thing == null)
            {
                player = e.Get(ThingEInfo.Default);
            }

            float LightningRange = i.Get(NumInfo.Default, 3);
            float LightningDmg = i.Get(NumInfo.Default, 1);
            Vector3 center = player.Thing.transform.position;

            Collider2D[] hits = Physics2D.OverlapCircleAll(center, LightningRange);        
            List<ThingInfo> alreadyHit = new List<ThingInfo>();            
            
            foreach (Collider2D hit in hits)
            {
                ThingController tc = hit.GetComponentInParent<ThingController>(); 

                if (tc != null && tc.Info != null)
                {
                    ThingInfo target = tc.Info;

                    if (target == player || alreadyHit.Contains(target)) continue;
                    if (target.Has(Traits.Player)) continue;

                    alreadyHit.Add(target);
                    
                    target.TakeEvent(God.E(EventTypes.Damage).Set(LightningDmg).Set(player));
                }
            }
        }
    }
}

public class Trait2_RaphaelC : Trait
{
    public Trait2_RaphaelC()
    {
        Type = Traits.Trait2_RaphaelC;
        AddListen(EventTypes.OnUse);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type == EventTypes.OnUse)
        {
            int amt = i.GetInt(NumInfo.Default, 1);
            EventInfo heal = God.E(EventTypes.Healing).Set(amt);
            e.GetThing().TakeEvent(heal,true);
            
        } 
    }
}

public class Trait3_RaphaelC : Trait
{
    public Trait3_RaphaelC()
    {
        //Type = Traits.Trait3_RaphaelC;
        AddListen(EventTypes.OnKill);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type == EventTypes.OnKill)
        {
            //teleport random
            //OR you just get big for a second
            Debug.Log("Teleport");
            ThingInfo player = i.Who;
            //player.Thing.transform.position.x += Random.Range(0, 3);
            //player.Thing.transform.position.x += Random.Range(0, 3);
        }
    }
}