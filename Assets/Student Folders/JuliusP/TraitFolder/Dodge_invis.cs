using System.Collections;
using UnityEngine;

public class Dodge_invis : Trait
{
    float activeTimer = 0f;
    float cooldownTimer = 0f;

    bool active = false;
    bool canUse = true;

    public Dodge_invis()
    {
        Type = Traits.DodgeInvis_JuliusP;

        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                ThingInfo ThingWTrait = i.Who;

                // START INVIS
                if (Input.GetKeyDown(KeyCode.Space) && canUse)
                {
                    active = true;
                    canUse = false;

                    activeTimer = 0f;
                    cooldownTimer = 0f;

                    // APPLY EFFECTS
                    if (!ThingWTrait.Has(Traits.GainInvis_JuliusP))
                    {
                        ThingWTrait.AddTrait(Traits.GainInvis_JuliusP);
                    }

                    if (!ThingWTrait.Has(Traits.Dash))
                    {
                        ThingWTrait.AddTrait(Traits.Dash);
                    }

                    // CLEAR CURRENT TARGET
                    ThingWTrait.SetTarget(null);

                    // CLEAR ENEMY TARGETS
                    foreach (ThingController t in God.GM.Things)
                    {
                        if (t != null && t.Info != null && t.Info.Target == ThingWTrait)
                        {
                            t.Info.SetTarget(null);
                        }
                    }
                }

                // ACTIVE INVIS
                if (active)
                {
                    activeTimer += Time.deltaTime;

                    // KEEP ENEMIES FROM TARGETING YOU
                    foreach (ThingController t in God.GM.Things)
                    {
                        if (t != null && t.Info != null && t.Info.Target == ThingWTrait)
                        {
                            t.Info.SetTarget(null);
                        }
                    }

                    // END INVIS
                    if (activeTimer >= 5f)
                    {
                        active = false;

                        // REMOVE TRAITS SAFELY
                        God.C(RemoveEffects(ThingWTrait));
                    }
                }

                // COOLDOWN
                else if (!canUse)
                {
                    cooldownTimer += Time.deltaTime;

                    if (cooldownTimer >= 5f)
                    {
                        canUse = true;
                    }
                }

                break;
            }
        }
    }

    private IEnumerator RemoveEffects(ThingInfo thing)
    {
        // WAIT ONE FRAME
        // prevents modifying traits during TakeEvent loop
        yield return null;

        if (thing != null)
        {
            if (thing.Has(Traits.GainInvis_JuliusP))
            {
                thing.RemoveTrait(Traits.GainInvis_JuliusP);
            }

            if (thing.Has(Traits.Dash))
            {
                thing.RemoveTrait(Traits.Dash);
            }
        }
    }
}