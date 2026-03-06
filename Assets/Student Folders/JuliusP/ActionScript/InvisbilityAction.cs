using UnityEngine;
using System.Collections.Generic;

public class InvisbilityAction : ActionScript
{
    SpriteRenderer[] SRS;
    float timer = 0f;
    bool started = false;

    public int timesFound = 0;

    // TELEPORT COOLDOWN TO AVOID SPAM
    float teleportCooldown = 1f;
    float teleportTimer = 0f;

    // Timer for blinking
    float AppearTimer = 0f;

    public InvisbilityAction(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Cloak_JuliusP, who);
        HaltMomentum = false;
        Duration = Mathf.Infinity;
    }

    public override void OnRun()
    {
        base.OnRun();
        if (God.Session.Player == null) return;

        float distanceToPlayer = Who.Thing.Distance(God.Session.Player.Thing);

        // START INVISIBILITY FIRST TIME
        if (!started && distanceToPlayer < 2f) 
        {
            StartInvisibility();
            timesFound++;
        }

        if (started)
        {
            timer += Time.deltaTime;
            teleportTimer += Time.deltaTime;
            AppearTimer += Time.deltaTime;

            // TELEPORT AGAIN IF PLAYER IS CLOSE AND COOLDOWN PASSED
            if (distanceToPlayer < 2f && teleportTimer >= teleportCooldown)
            {
                TeleportNearPlayer();
                teleportTimer = 0f;

                // INCREMENT TIMESFOUND EACH TELEPORT
                timesFound++;
            }

            // SHOW/HIDE THING EVERY 3 SECONDS
            ShowLocationEveryFewSecs();

            // START ATTACKING ONCE TIMESFOUND >= 4
            if (timesFound >= 4)
            {
                Complete();
            }
        }

        Debug.Log("Times found: " + timesFound);
    }

    void StartInvisibility()
    {
        started = true;
        timer = 0f;
        teleportTimer = 0f;
        AppearTimer = 0f;

        // GET ALL SPRITERENDERERS
        SRS = Who.Thing.gameObject.GetComponentsInChildren<SpriteRenderer>(true);

        // ADD IGNORE DAMAGE TRAIT
        if (!Who.Has(Traits.IgnoreDamage_JuliusP))
            Who.AddTrait(Traits.IgnoreDamage_JuliusP);

        // MAKE INVISIBLE
        foreach (SpriteRenderer sr in SRS)
            sr.enabled = false;

        // INITIAL TELEPORT
        TeleportNearPlayer();
    }

    void ShowLocationEveryFewSecs()
    {
        // BLINK EVERY 3 SECONDS//
        if ((int)AppearTimer % 3 == 0)
        {
            foreach (SpriteRenderer sr in SRS)
                sr.enabled = true;
        }
        
        else
        {
            foreach (SpriteRenderer sr in SRS)
                sr.enabled = false;
        }
    
    }

    void TeleportNearPlayer()
    {
        Vector3 playerPos = God.Session.Player.Thing.transform.position;

        // RANDOM OFFSETS
        List<Vector3> RandomPos = new List<Vector3>
        {
            -Who.Thing.transform.right * 3.1f,
             Who.Thing.transform.up * 3.1f,
            -Who.Thing.transform.up * 3.1f
        };

        // PICK RANDOM OFFSET
        Who.Thing.transform.position = playerPos + RandomPos[Random.Range(0, RandomPos.Count)];
        Who.Thing.LookAt(God.Session.Player.Thing, 0f);
    }

    public override void End()
    {
        base.End();

        if (SRS != null)
        {
            foreach (SpriteRenderer sr in SRS)
                sr.enabled = true;
        }

        Who.RemoveTrait(Traits.IgnoreDamage_JuliusP);
    }

    public override Actions NextAction()
    {
        return Actions.DefaultAttack;
    }
}