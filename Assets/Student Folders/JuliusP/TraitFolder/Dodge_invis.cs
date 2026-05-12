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
                ThingInfo me = i.Who;


                if (Input.GetKeyDown(KeyCode.Space) && canUse)
                {
                    active = true;
                    canUse = false;

                    activeTimer = 0f;
                    cooldownTimer = 0f;

                    // APPLY EFFECTS
                    me.AddTrait(Traits.GainInvis_JuliusP);
                    me.AddTrait(Traits.Dash);

                    // CLEAR CURRENT TARGET
                    me.SetTarget(null);

                    // THIS SETS THE TARGET TO NOT SEE YOU//s
                    foreach (ThingController t in God.GM.Things)
                    {
                        if (t.Info.Target == me)
                        {
                            t.Info.SetTarget(null);
                        }
                    }
                }

              
                if (active)
                {
                    activeTimer += Time.deltaTime;

                    // KEEP ENEMIES FROM RETARGETING YOU
                    foreach (ThingController t in God.GM.Things)
                    {
                        if (t.Info.Target == me)
                        {
                            t.Info.SetTarget(null);
                        }
                    }

                    // END INVIS
                    if (activeTimer >= 5f)
                    {
                        me.RemoveTrait(Traits.GainInvis_JuliusP);
                        me.RemoveTrait(Traits.Dash);

                        active = false;
                    }
                }

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
}