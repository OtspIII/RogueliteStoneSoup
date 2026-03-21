using UnityEngine;

public class ProtectionSpell_SarahS : Trait
{
    public ProtectionSpell_SarahS()
    {
        Type = Traits.ProtectionSpellSarahS;
        AddPreListen(EventTypes.Damage);
    }

    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type == EventTypes.Damage)
        {
            e.Abort = true;
        }
    }
}
