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

                    if (e.Type != EventTypes.Damage)
                        return;

                    if (Random.Range(0,6)>1)
                    {

                        


                       
                        
                        int doudamage = i.GetInt(NumInfo.Default, 15);
                        EventInfo damage = God.E(EventTypes.Damage).Set(doudamage);
                        e.GetThing().TakeEvent(damage, true);
                        

                        Debug.Log("ss");
                    }

                }
                    break;
                
        }
    }

}
