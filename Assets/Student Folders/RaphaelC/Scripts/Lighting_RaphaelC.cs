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

public class BasicHeal_RaphaelC : Trait
{
    public BasicHeal_RaphaelC()
    {
        Type = Traits.BasicHeal_RaphaelC;
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

public class KillSpeedBoost : Trait
{
    public KillSpeedBoost()
    {
        Type = Traits.KillSpeedBoost;
        AddListen(EventTypes.OnKill);
        AddListen(EventTypes.Update);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        float duration = 1.5f;

        switch (e.Type)
        {
            case EventTypes.OnKill:
            {
                float timer = i.GetFloat(NumInfo.Default, 0f);
                if (timer <= 0f)
                {
                    i.Who.Thing.Info.CurrentSpeed = i.Who.Thing.Info.CurrentSpeed * 2f; 
                }
                i.Set(NumInfo.Default, duration);
                return;
            }
            case EventTypes.Update:
            {
                float timer = i.GetFloat(NumInfo.Default, 0f);
               
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    i.Set(NumInfo.Default, timer);
                    if (timer <= 0)
                    {
                        i.Who.Thing.Info.CurrentSpeed = 6f;
                        i.Set(NumInfo.Default, 0f);
                    }
                }
                return;
            }
            default:
                return;
        }
    }
}

public class WhirlPool_RaphaelC : Trait
{
    public WhirlPool_RaphaelC()
    {
        Type = Traits.WhirlPool_RaphaelC;
        AddListen(EventTypes.OnInside);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type == EventTypes.OnInside)
        {
            float knb = i.Get(NumInfo.Distance,5);
            HitboxController hb = e.GetHitbox();
            foreach (ThingController tc in hb.Touching.ToArray())
            {
                ThingInfo t = tc.Info;
                Vector2 centerPos = i.Who.Thing.transform.position;
                Vector2 targetPos = t.Thing.transform.position;
                Vector2 kb = (targetPos - centerPos).normalized * (-knb);
                t.TakeEvent(God.E(EventTypes.Knockback).Set(kb).Set(i.Who));
            }
        }
    }
}