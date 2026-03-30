using UnityEngine;

public class Repel_SarahS : ActionScript
{
    private float knockbackForce = 15f;
    private float damageAmt = 1f;

    public Repel_SarahS(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.RepelSarahS, who);
        Priority = 1;
        Duration = 0.5f;
        Anim = "Use";
    }

    public override void Begin()
    {
        base.Begin();

        foreach (ThingController thing in God.GM.Things)
        {
            if (thing.Info.Team == GameTeams.Enemy)
            {
                thing.Info.TakeEvent(God.E(EventTypes.Damage).Set(damageAmt).Set(Who));
                
                Vector2 pushDirection = (thing.transform.position - Who.Thing.transform.position).normalized;
                Vector2 finalKnockback = pushDirection * knockbackForce;
                thing.Info.TakeEvent(God.E(EventTypes.Knockback).Set(finalKnockback).Set(Who));
            }
        }

        if (God.Session != null && God.Session.PlayerInventory != null)
        {
            for (int i = God.Session.PlayerInventory.Count - 1; i >= 0; i--)
            {
                ThingInfo item = God.Session.PlayerInventory[i];
                TraitInfo limitedUse = item.Get(Traits.LimitedUse);

                if (limitedUse != null && limitedUse.GetInt() <= 0)
                {
                    if (Who.CurrentHeld == item)
                    {
                        Who.DropHeld(true);
                    }
                    else
                    {
                        God.Session.PlayerInventory.RemoveAt(i);
                    }

                    break;
                }
            }
        }
    }
}
