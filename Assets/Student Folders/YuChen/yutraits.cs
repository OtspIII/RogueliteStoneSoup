using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class CriticaldamageTrait : Trait 
{
    public CriticaldamageTrait() 
    { 
        Type = Traits.Criticaldamage; 
        AddListen(EventTypes.GetDamage); 
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.GetDamage:
                {
                    if (e.Type != EventTypes.GetDamage)
                        return;

                    if (Random.value < 0.2f)
                    {
                        float multiplier = e.GetFloat();
                        multiplier *= 2f;

                        e.Set(NumInfo.Default, multiplier);

                        Debug.Log("Critical Hit!");
                    }

                }
                    break;
                
        }
    }

}
