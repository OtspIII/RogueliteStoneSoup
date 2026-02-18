using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class CriticaldamageTrait : Trait 
{
    public CriticaldamageTrait() 
    { 
        Type = Traits.Criticaldamage; 
        AddListen(EventTypes.Damage); 
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Damage:
                {
                    if (Random.Range(1, 6) == 1)
                    {
                        float damage = e.GetFloat();
                        e.Set(NumInfo.Default, damage * 2f);

                        Debug.Log("dealt DOUBLE damage!");
                        
                    }
                    break;
                }
        }
    }

}
