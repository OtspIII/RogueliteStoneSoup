using UnityEngine;

public class Blight_ElioR : Trait
{
    public Blight_ElioR()
    {
        Type = Traits.Blight_ElioR;
        AddPreListen(EventTypes.Damage);
        
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
       
    }
    public override void PreEvent(TraitInfo i, EventInfo e)
    {
        switch(e.Type)
        {
            case EventTypes.Damage:
                {
                    //Borrowed JuliusP MonadoPower modifier
                    float dmg = e.GetFloat(NumInfo.Default);
                        e.SetFloat(NumInfo.Default, dmg * 1.5f);
                        i.Who.RemoveTrait(i.Trait);
                    break;
                }
        }
    }
}
