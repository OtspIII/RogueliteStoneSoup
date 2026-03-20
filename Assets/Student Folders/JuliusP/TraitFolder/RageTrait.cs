using UnityEngine;
using System.Collections;

public class RageTrait : Trait
{
    //BOOL FOR TRACJING RAGE//
    private bool rageActive = false;

    private bool OnlyOnce = false;
    

    public RageTrait()
    {
        // SETS THE TRAIT TYPE TO RAGE
        Type = Traits.Rage;

        //DMG MULTIPLIER EVENT//
        AddListen(EventTypes.DamageMult);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.DamageMult:
                
                // IF RAGE IS ALREADY ACTIVE, DO NOTHING
                if (rageActive) break;

                // GETS THE ATTACKER WHO IS CAUSING THE DAMAGE
                ThingInfo attacker = e.GetThing();
               
                // RETURN IF ATTACKER IS NULL OR DOSENT EXIST
                if (attacker == null) break;

                // THIS GETS THE ATTACKER'S HEALTH TRAIT TO CHECK//
                TraitInfo health = attacker.Get(Traits.Health);
                
                // THIS WILL RETURN IF HEALTH TRAIT DOES NOT EXIST
                if (health == null) break;

                // GET THE ATTACKER THING'S CURRENT HP
                float current = health.GetN();
               
                // GET THE ATTACKER THING'S MAX HP
                float max = health.Get(NumInfo.Max);

                // CHECK IF THE ATTACKER'S HP IS 50% OR LESS
                if (current / max <= 0.5f && !OnlyOnce)
                {

                    Debug.Log("Active");
                    rageActive = true;

                    if (attacker.Thing == null) break;

                    //GET THE TRANSOFMR OF THE ATTACKER//
                    Transform ThingPos = attacker.Thing.transform;

                    // LOADS THE RAGEAURA PARTICLE EFFECT IN THE FOLDER//
                    GameObject rageAura = Resources.Load<GameObject>("JuliusP/Gnomes/RageAura");

                    GameObject aura = null;
                    if (rageAura != null)
                    {
                        // THIS SPAWNS THE AURA PARTICLE EFFECT//
                        aura = GameObject.Instantiate(rageAura, ThingPos.position, Quaternion.identity);

                        // MAKES THE AURA FOLLOW OR BE ATTACHED TO THING//
                        aura.transform.SetParent(ThingPos);

                        // KEEP THE RAGEAURA CENTERED ON THE THING//
                        aura.transform.localPosition = Vector3.zero;

                        // PLAYS THE PARTICLE SYSTEM//
                        ParticleSystem ps = aura.GetComponent<ParticleSystem>();
                        if (ps != null)
                            ps.Play();
                    }

                    // GET THE WEAPON THE ATTACKER IS CURRENTLY HOLDING
                    ThingInfo weapon = attacker.CurrentHeld;

                    // RETURNS IF NO WEAPON IS HELD
                    if (weapon == null)
                    {
                        rageActive = false;
                        break;
                    }

                    // GET THE TOOL TRAIT FROM THE WEAPON (WHERE DAMAGE MULTIPLIER IS STORED)
                    TraitInfo tool = weapon.Get(Traits.Tool);

                    // SAFETY CHECK: RETURN IF THE TOOL TRAIT DOES NOT EXIST
                    if (tool == null)
                    {
                        rageActive = false;
                        break;
                    }

                    // STORE ORIGINAL DAMAGE
                    float originalDamage = tool.GetN();


                    // SET THE WEAPON'S DAMAGE TO DOUBLE (FIXED)
                    tool.Set(originalDamage * 2f);

                    // DOUBLE DAMAGE LASTS FOR 4 SECONDS//
                    God.C(RageTimer(attacker, tool, originalDamage, aura));

                    OnlyOnce = true;
                }
                break;
        }
    }

    public IEnumerator RageTimer(ThingInfo attacker, TraitInfo tool, float originalDamage, GameObject aura)
    {
        // WAIT FOR 5 SECONDS
        yield return new WaitForSeconds(5f);

        // RESET WEAPON DAMAGE TO ORIGINAL VALUE
        if (tool != null)
            tool.Set(originalDamage);

        // DESTROY THE RAGE AURA
        if (aura != null)
            GameObject.Destroy(aura);

        // LOGGING
        Debug.Log("Rage effect ended.");

        

        // ALLOW RAGE TO TRIGGER AGAIN NEXT TIME
        rageActive = false;
    }
}