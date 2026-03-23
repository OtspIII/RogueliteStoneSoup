using UnityEngine;
using System.Collections.Generic;

public class InvisbilityAction : ActionScript
{
    SpriteRenderer[] SRS;
    float timer = 0f;
    bool started = false;

    public int timesFound = 0;

    // TELEPORT COOLDOWN TO AVOID SPAM
    float teleportCooldown = 0.5f;
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
        if (!started && distanceToPlayer < 3.5f) 
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
            if (distanceToPlayer < 3f && teleportTimer >= teleportCooldown)
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

        //Debug.Log("Times found: " + timesFound);
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
    Transform player = God.Session.Player.Thing.transform;

    // Define offsets relative to the player
    List<Vector3> offsets = new List<Vector3>
    {
        player.right * 2.1f,    // right
        -player.right * 2.1f,   // left
        player.up * 2.1f,       // up
        -player.up * 2.1f        // down
    };

    // Pick a random offset
    Vector3 randomOffset = offsets[Random.Range(0, offsets.Count)];

    // Teleport relative to the player's position
    Who.Thing.transform.position = player.position + randomOffset;

    // Face the player
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