using UnityEngine;

public class SloMo_SarahS : Trait
{
    public SloMo_SarahS()
    {
      // Type = Traits.SloMo;
        AddListen(EventTypes.OnSee);
        AddPreListen(EventTypes.GetActSpeed);
        AddListen(EventTypes.OnUseStart);
    }

    public override void ReUp(TraitInfo old, EventInfo n)
    {
        if (n == null || !n.GetBool(BoolInfo.Default)) return;
        old.SetFloat(NumInfo.Time, n.GetFloat(NumInfo.Time, 5f));
        old.SetFloat(NumInfo.Speed, n.GetFloat(NumInfo.Speed, 0.5f));
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSee:
            {
                if (i.GetBool(BoolInfo.Default)) return;

                ThingInfo target = e.GetThing(ThingEInfo.Target);
                if (target == null) return;

                EventInfo slowInfo = new EventInfo();
                slowInfo.SetFloat(NumInfo.Time, i.GetFloat(NumInfo.Time, 5f));
                slowInfo.SetFloat(NumInfo.Speed, i.GetFloat(NumInfo.Speed, 0.5f));
                slowInfo.SetBool(BoolInfo.Default, true);
                
                target.AddTrait(Type, slowInfo);
                break;
            }

            case EventTypes.Update:
            { 
                if (i.GetBool(BoolInfo.Default)) return;
                
                float timeLeft = i.GetFloat(NumInfo.Time, 0f);
                timeLeft -= Time.deltaTime;
                i.SetFloat(NumInfo.Time, timeLeft);

                if (timeLeft <= 0)
                    i.Who.RemoveTrait(Type);
                break;
            }
        }
    }

    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type != EventTypes.GetActSpeed) return;
        
        if (!i.GetBool(BoolInfo.Default)) return;
        
        float slowMult  = i.GetFloat(NumInfo.Speed, 0.5f);
        float currentMult = e.GetFloat(NumInfo.Default, 1f);
        e.SetFloat(NumInfo.Default, currentMult * slowMult);
    }
}
