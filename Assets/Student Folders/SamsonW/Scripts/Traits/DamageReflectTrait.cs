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
                Debug.Log($"DamageReflectTrait: {attacker.Thing.gameObject.name} dealt {damageTaken} to {victim.Thing.gameObject.name}");
                attacker.TakeEvent(new EventInfo(EventTypes.Damage)
                    .Set(NumInfo.Default, damageTaken)
                    .Set(ThingEInfo.Default, victim) // Victim is the one reflecting
                );
                // Possible recursion with damage calling upon damage? Will worry about later.
                break;
            }
        }
    }
}