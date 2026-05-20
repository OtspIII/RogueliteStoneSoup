using Unity.VisualScripting;
using UnityEngine;

public class ContactDamage_SarahS : Trait
{
    public float damageCooldown = 1f;
    private float lastHitTime = -999f;
    public ContactDamage_SarahS()
    {
        Type = Traits.ContactDamageSarahS;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnTouch:
            {
                GameCollision col = e.Collision;
                if (col == null || col.Other == null) break;
                
                ThingInfo hitThing = col.Other.Info;
                if (hitThing.Has(Traits.Player))
                {
                    if (Time.time >= lastHitTime + damageCooldown)
                    {
                        Debug.Log("ghost Dealt Damage");
                        
                        hitThing.TakeEvent(God.E(EventTypes.Damage).Set(1).Set(i.Who));
                        lastHitTime = Time.time;
                    }
                }

                break;
            }
        }
    }
}
