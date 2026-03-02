using UnityEngine;

public class Sneaky_qixiangdong : Trait
{
    private bool Revealed = false;
    public Sneaky_qixiangdong()
    {
        Type = Traits.Sneaky_qixiangdong;
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.Damage);
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:

                Revealed = false;

                SpriteRenderer sr = i.Who.Thing.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.enabled = false;
                };

                break;
            case EventTypes.Damage:
                
                {
                    ThingInfo source = e.Get(ThingEInfo.Source);

                    if(source == i.Who && !Revealed)
                    {
                        Revealed = true;
                        SpriteRenderer sr2= i.Who.Thing.GetComponent<SpriteRenderer>();
                        if (sr2!= null)
                        {
                            sr2.enabled = true;
                        }
                        ;
                    }

                }
                break;
        }
    }
}
