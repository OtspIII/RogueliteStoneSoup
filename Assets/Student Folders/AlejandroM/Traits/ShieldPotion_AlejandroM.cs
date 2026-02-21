using UnityEngine;

public class ShieldPotion_AlejandroM : Trait
{
    public ShieldPotion_AlejandroM()
    {
        Type = Traits.ShieldPotion_AlejandroM; 
        AddListen(EventTypes.Setup);
        AddPreListen(EventTypes.Damage, 0); 
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type == EventTypes.Setup)
        {
            // 1 shield by default
            i.Set(NumInfo.Default, i.GetFloat(NumInfo.Default, 1f));
        }
    }

    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type != EventTypes.Damage) return;

        float shields = i.GetFloat(NumInfo.Default, 1f);
        if (shields <= 0) return;

        e.Abort = true;                  // block the hit
        i.Set(NumInfo.Default, shields - 1); // consume shield
    }
}