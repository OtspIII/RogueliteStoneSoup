using UnityEngine;
using System.Collections;

public class UltimateRage : Trait
{    
    
    // BOOL FOR TRACKING RAGE
    private bool OnlyOnce = false;


    private GameObject aura;
  
    public UltimateRage()
    {
        Type = Traits.LowHealthWarrior_JuliusP;

        AddListen(EventTypes.Setup);

        AddListen(EventTypes.ShownHP);

        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
            //GET TRAITINFO OF THING WITH TRAIT//
             TraitInfo health = i.Who.Get(Traits.Health);

            //TRAITINFO CHECK//
            if (health != null)
            {
                    //SET HEALTH TO 1//
                    health.Set(NumInfo.Default, 1);
                    health.Set(NumInfo.Max, 1);
            }
                
            

                break;
            }

            //DISPLAY THE HP TO BE 1//
            case EventTypes.ShownHP:
            {
                e.Set(i.GetInt()); 
                e.Set(NumInfo.Max,i.GetInt(NumInfo.Max)); 
                break;
            }


            // SPAWN RAGE AURA ON SPAWN
            case EventTypes.Update:
            {
                if (OnlyOnce) break;

                ThingInfo me = i.Who;
                if (me.Thing == null) break;

                Transform pos = me.Thing.transform;

                GameObject rageAuraPrefab = Resources.Load<GameObject>("JuliusP/Gnomes/RageAura");
                if (rageAuraPrefab != null)
                {
                    aura = GameObject.Instantiate(rageAuraPrefab, pos.position, Quaternion.identity);
                    aura.transform.SetParent(pos);
                    aura.transform.localPosition = Vector3.zero;

                    ParticleSystem ps = aura.GetComponent<ParticleSystem>();
                    if (ps != null)
                        ps.Play();
                }

                OnlyOnce = true;
                break;
            }
    }
        }

        
}




public class RageAlwaysOn : Trait
{
    private GameObject aura;



    public RageAlwaysOn()
    {
        // SETS THE TRAIT TYPE
        Type = Traits.AlwaysRage_JuliusP;

        // LISTEN FOR DAMAGE MULTIPLIER EVENT
        AddListen(EventTypes.DamageMult);

       
    }

     public override void TakeEvent(TraitInfo i, EventInfo e)
    {   
    switch (e.Type)
    {
        case EventTypes.DamageMult:
        {
            // Get the attacker causing the damage
            ThingInfo attacker = e.GetThing();
            if (attacker == null) break;

            // Double the weapon damage if the attacker is holding a weapon
            ThingInfo weapon = attacker.CurrentHeld;
            if (weapon != null)
            {
                TraitInfo tool = weapon.Get(Traits.Tool);
                if (tool != null)
                {
                    float originalDamage = tool.GetN();
                
                    tool.Set(2f); 
                }
            }

            break;
        }



        
    }
}
}
