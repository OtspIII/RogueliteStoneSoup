using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class SpeedIncrease_yuTrait : Trait
{
    public SpeedIncrease_yuTrait()
    {
        Type = Traits.SpeedIncrease_yu;
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


                if (Random.value < 0.35f)
                {
                    TraitInfo actor = attacker.Get(Traits.Actor);
                    if (actor == null) 
                        return;
                    float normalspeed = actor.GetFloat(NumInfo.Speed, 5f);

                    actor.Set(NumInfo.Speed, normalspeed + 1f);

                    Debug.Log("Speed increased by 1");//speed will increase in next level
                }


                break;

        }
    }

}
