using UnityEngine;

public class SpeedUpTrait_SabahE : Trait
{
    public SpeedUpTrait_SabahE()
    {
        //Type = Traits.SpeedUpSabahE;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.Damage);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
                {
                    
                    i.Set(NumInfo.Max, i.GetFloat(NumInfo.Speed, 5f));
                    i.Set(NumInfo.Time, 0f);
                    break;
                }

            case EventTypes.Damage:
                {
                    // Start / refresh speed boost for 10 seconds
                    float baseSpeed = i.GetFloat(NumInfo.Max, i.GetFloat(NumInfo.Speed, 5f));
                    float boostAmount = i.GetFloat(NumInfo.Default, 5f); 
                    i.Set(NumInfo.Speed, baseSpeed + boostAmount);
                    i.Set(NumInfo.Time, 10f);
                    break;
                }

            case EventTypes.Update:
                {
                    float t = i.GetFloat(NumInfo.Time, 0f);
                    if (t <= 0) break;

                    t -= Time.deltaTime;
                    i.Set(NumInfo.Time, t);

                    if (t <= 0)
                    {
                        float baseSpeed = i.GetFloat(NumInfo.Max, 5f);
                        i.Set(NumInfo.Speed, baseSpeed);
                        i.Who.RemoveTrait(Type);
                    }
                    break;
                }
        }
    }
}
