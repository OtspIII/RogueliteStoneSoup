using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class CriticaldamageTrait : Trait 
{
    public CriticaldamageTrait() 
    { 
        Type = Traits.Criticaldamage;
        AddListen(EventTypes.DamageMult);

    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.DamageMult:
                

             if (e.Type != EventTypes.DamageMult)
                        return;
                ThingInfo attacker = e.GetThing();
                if (attacker == null) return;


                if (Random.value < 0.25f)
                {
                    ThingInfo weapon = attacker.CurrentHeld;
                    if (weapon == null) return;

                    TraitInfo tool = weapon.Get(Traits.Tool);
                    if (tool == null) return;

                    float StartDamage = tool.GetN();

                    tool.Set(StartDamage * 2f);

                        
                    }

                
                    break;
                
        }
    }

}
