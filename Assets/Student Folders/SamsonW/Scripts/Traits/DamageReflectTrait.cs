using UnityEngine;

public class DamageReflectTrait : Trait
{
    public DamageReflectTrait()
    {
        Type = Traits.DamageReflect;
        AddListen(EventTypes.Damage);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Damage:
            {
                ThingInfo attacker = e.GetThing();
                if (attacker == null)
                    return;
                ThingInfo victim = i.Who;
                if (victim == null)
                    return;
                float damageTaken = e.GetFloat();
                float reflectAmount = i.GetFloat(NumInfo.Default, 1f); // Reflect flat dmg, defaulted to 1
                attacker.TakeEvent(new EventInfo(EventTypes.Damage)
                    .Set(NumInfo.Default, reflectAmount) 
                    .Set(ThingEInfo.Default, victim) // Victim is the one reflecting
                );
                Debug.Log($"DamageReflectTrait: {attacker.Thing.gameObject.name} dealt {damageTaken} to {victim.Thing.gameObject.name}, reflecting {reflectAmount}");
                // Possible recursion with damage calling upon damage? Will worry about later.
                break;
            }
        }
    }
}