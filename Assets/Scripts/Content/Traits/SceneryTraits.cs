using UnityEngine;
using UnityEngine.SceneManagement;


public class ExitTrait : Trait
{
    public ExitTrait()
    {
        Type = Traits.Exit;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnTouch:
            {
                GameCollision col = e.Collision;
                ThingInfo t = col.Other.Info;
                if (t.Has(Traits.Player))
                {
                    SceneManager.LoadScene("YouWin");
                }

                return;
            }
            default: return;
        }
    }
}

public class DamageZoneTrait : Trait
{
    public DamageZoneTrait()
    {
        Type = Traits.DamageZone;
        AddListen(EventTypes.OnInside);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnInside:
            {
                float timer = i.Get(NumInfo.Speed,1);
                float dmg = i.Get(NumInfo.Amount,1);

                // Debug.Log("INSIDE LAVA: " + timer + " / " + dmg);
                HitboxController hb = e.GetHitbox();
                foreach (ThingController tc in hb.Touching)
                {
                    ThingInfo t = tc.Info;
                    t.TakeEvent(God.E(EventTypes.Damage).Set(dmg).Set(i.Who));
                }
                hb.Timer = timer;
                return;
            }
            default: return;
        }
    }
}
