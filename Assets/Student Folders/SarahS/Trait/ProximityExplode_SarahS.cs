using UnityEngine;

public class ProximityExplode_SarahS : Trait
{
    public ProximityExplode_SarahS()
    {
        Type = Traits.SarahS1;
        
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                float range = i.GetFloat(NumInfo.MiscA, 2f);
                float damage = i.GetFloat(NumInfo.Default, 3f);

                if (i.GetBool(BoolInfo.Default)) return;

                if (i.Who == null || i.Who.Thing == null) return;

                ThingInfo target = i.Who.Target;
                
                if (target == null || target.Thing == null) return;
                
                float dist = Vector2.Distance(i.Who.Thing.transform.position, target.Thing.transform.position);
                
                if (dist > range) return;
                Debug.Log("EXPLODE TRIGGER: " + i.Who.Name + " near " + target.Name);

                i.SetBool(BoolInfo.Default, true);
                
                target.TakeEvent(
                    God.E(EventTypes.Damage).Set(damage).Set(i.Who).Set("Explosion")
                    );
                
                i.Who.Destruct(i.Who);

                break;
            }
        }
    }
    
}
