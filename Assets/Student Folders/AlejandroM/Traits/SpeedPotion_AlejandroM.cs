using UnityEngine;

public class SpeedPotion_AlejandroM : Trait
{
    public SpeedPotion_AlejandroM()
    {
        Type = Traits.SpeedPotion_AlejandroM;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
                {
                    // Duration of Max speed is 20 sec
                    float duration = i.GetFloat(NumInfo.Max, 20f);
                    i.Set(NumInfo.Default, duration);

                    
                    
                    var who = i.Who.Thing;
                    if (who != null)
                    {
                        i.Set(NumInfo.Min, who.Info.CurrentSpeed);
                    }
                    break;
                }

            case EventTypes.Update:
                {
                    float t = i.GetFloat(NumInfo.Default);
                    if (t <= 0) return;

                    t -= Time.deltaTime;
                    i.Set(NumInfo.Default, t);

                    var who = i.Who.Thing;
                    if (who == null) return;

                    float original = i.GetFloat(NumInfo.Min, who.Info.CurrentSpeed);
                    float boosted = original * 1.5f; // increase speed 
                    who.Info.CurrentSpeed = boosted;

                    // When timer ends for the speed effect you go back to original speed 
                    if (t <= 0)
                    {
                        who.Info.CurrentSpeed = original;
                        i.Set(NumInfo.Default, 0);
                    }
                    break;
                }
        }
    }
}