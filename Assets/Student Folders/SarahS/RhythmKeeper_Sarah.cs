using UnityEngine;

public class RhythmKeeper_SarahS : Trait
{
    public RhythmKeeper_SarahS()
    {
        Type = Traits.RhythmKeeperSarahS;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                i.Set(NumInfo.MiscA, 0f);
                i.Set(NumInfo.MiscB, 0f);
                i.Set(NumInfo.MiscC, 0f);
                break;
            }
            case EventTypes.Update:
            {
                RhythmSheet_Sarah sheet = i.GetOption() as RhythmSheet_Sarah;
                float interval;
                float sectionDuration;
                bool loop;
                int sectionCount;

                if (sheet == null || sheet.SectionCount == 0)
                {
                    interval = 0.5f;
                    sectionDuration = float.MaxValue;
                    loop = true;
                    sectionCount = 1;
                }
                else
                {
                    int section = i.GetInt(NumInfo.MiscC);
                    interval = sheet.GetInterval(section);
                    sectionDuration = sheet.GetDuration(section);
                    loop = sheet.Loop;
                    sectionCount = 1;
                }

                float sectionTimer = i.GetFloat(NumInfo.MiscB) + Time.deltaTime;
                if (sectionTimer >= sectionDuration)
                {
                    sectionTimer = 0f;
                    int nextSection = i.GetInt(NumInfo.MiscC) + 1;
                    if (nextSection >= sectionCount)
                    {
                        if (loop)
                            nextSection = 0;
                        else
                        {
                            i.Set(NumInfo.MiscB, sectionTimer);
                            return;
                        }
                    }
                    i.Set(NumInfo.MiscC, (float)nextSection);
                    if (sheet != null)
                        interval = sheet.GetInterval(nextSection);
                }
                i.Set(NumInfo.MiscC, sectionTimer);
                
                float beatTimer = i.GetFloat(NumInfo.MiscA) + Time.deltaTime;
                i.Set(NumInfo.MiscA, beatTimer);

                if (beatTimer < interval) return;
                i.Set(NumInfo.MiscA, beatTimer - interval);

                if (God.GM == null) return;
                foreach (ThingController thing in God.GM.Things)
                {
                    thing.TakeEvent(God.E(EventTypes.Message).Set(StrInfo.Message, "Beat"));
                }
            }
                break;
        }
    }
}
