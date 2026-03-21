using UnityEngine;
public class ProtectionCircle_SarahS : Trait
{
    public ProtectionCircle_SarahS()
    {
        Type = Traits.ProtectionCircleSarahS;
        AddListen(EventTypes.OnTouch);
        AddListen(EventTypes.OnTouchEnd);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnTouch:
            {
                ThingInfo t = e.GetThing(ThingEInfo.Source);
                if (t == null) return;
                
                if (t.Team != GameTeams.Player) return;

                EventInfo protectionInfo = new EventInfo();
                protectionInfo.SetThing(ThingEInfo.Source, i.Who);
                t.AddTrait(Traits.ProtectionSpellSarahS, protectionInfo);
                break;
            }
            case EventTypes.OnTouchEnd:
            {
                ThingInfo t = e.GetThing(ThingEInfo.Source);
                if (t == null) return;
                t.RemoveTrait(Traits.ProtectionSpellSarahS);
                break;
            }
        }
    }
}
