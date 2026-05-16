using UnityEngine;

public class RhythmChaser_SarahS : Trait
{
    public RhythmChaser_SarahS()
    {
        Type = Traits.RhythmChaserSarahS;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.Update);
        AddListen(EventTypes.Message);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                float baseSpeed = i.GetFloat(NumInfo.Default, 0f);
                if (baseSpeed <= 0f)
                    baseSpeed = i.Who.CurrentSpeed;
                i.Set(NumInfo.Default,baseSpeed);
                
                float burstSpeed = i.GetFloat(NumInfo.Max, 0f);
                if (burstSpeed <= 0f)
                    burstSpeed = baseSpeed * 2f;
                i.Set(NumInfo.Max,burstSpeed);

                float burstDur = i.GetFloat(NumInfo.Time, 0.2f);
                i.Set(NumInfo.Time,burstDur);

                i.Set(NumInfo.MiscA, 0f);
                break;
            }
            case EventTypes.Update:
            {
                float remaining = i.GetFloat(NumInfo.MiscA, 0f);
                if (remaining <= 0f) break;
                
                remaining -= Time.deltaTime;
                i.Set(NumInfo.MiscA,remaining);

                if (remaining <= 0f)
                {
                    float baseSpeed = i.GetFloat(NumInfo.Default, i.Who.CurrentSpeed);
                    i.Who.CurrentSpeed = baseSpeed;
                }

                break;
            }
            case EventTypes.Message:
            {
                if (e.GetString(StrInfo.Message) != "Beat") break;
                float burstSpeed = i.GetFloat(NumInfo.Max);
                float burstDur = i.GetFloat(NumInfo.Time, 0.2f);
                i.Who.CurrentSpeed = burstSpeed;
                i.Set(NumInfo.MiscA,burstDur);
                break;
            }
        }
    }
}
