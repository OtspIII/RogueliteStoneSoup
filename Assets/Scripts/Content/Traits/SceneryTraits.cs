using UnityEngine;
using UnityEngine.SceneManagement;


public class ExitTrait : Trait
{
    public ExitTrait()
    {
        Type = Traits.Exit;
        TakeListen.Add(EventTypes.OnHit);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnHit:
            {
                ThingInfo t = e.GetActor();
                if (t.IsPlayer())
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
        TakeListen.Add(EventTypes.OnHitInside);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnHitInside:
            {
                float timer = i.Get(NumInfo.Speed,1);
                float dmg = i.Get(NumInfo.Amount,1);

                // Debug.Log("INSIDE LAVA: " + timer + " / " + dmg);
                HurtboxController hb = e.Hurtbox;
                foreach (ThingController tc in hb.Inside)
                {
                    ThingInfo t = tc.Info;
                    t.TakeEvent(God.E(EventTypes.Damage).Set(dmg));
                }
                hb.Timer = timer;
                return;
            }
            default: return;
        }
    }
}
