using UnityEngine;

public class RallyTrait_SabahE : Trait
{
    public RallyTrait_SabahE()
    {
      //  Type = Traits.RallySabahE;
        AddListen(EventTypes.Damage);
        AddListen(EventTypes.Update);
        AddListen(EventTypes.OnKill);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Damage:
                {
                    float dmg = e.GetFloat();
                    if (dmg <= 0) break;

                    i.Set(NumInfo.Default, dmg);   // recoverable HP
                    i.Set(NumInfo.Time, 2f);       // rally window duration(Twek it for balance) 
                    break;
                }

            case EventTypes.OnKill:
                {
                    float rallyAmount = i.GetFloat(NumInfo.Default, 0f);
                    float timeLeft = i.GetFloat(NumInfo.Time, 0f);

                    if (rallyAmount > 0 && timeLeft > 0)
                    {
                        i.Who.TakeEvent(God.E(EventTypes.Healing).Set(rallyAmount));
                        i.Set(NumInfo.Default, 0f);
                        i.Set(NumInfo.Time, 0f);
                    }

                    break;
                }

            case EventTypes.Update:
                {
                    float timeLeft = i.GetFloat(NumInfo.Time, 0f);
                    if (timeLeft <= 0) break;

                    timeLeft -= Time.deltaTime;
                    i.Set(NumInfo.Time, timeLeft);

                    if (timeLeft <= 0)
                    {
                        i.Set(NumInfo.Default, 0f);
                    }

                    break;
                }
        }
    }
}