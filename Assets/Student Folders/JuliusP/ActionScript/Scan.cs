using UnityEngine;

public class Scan : ActionScript
{
    public float ViewDistance = 8f;

    private bool hasSeenPlayer = false;


    private float fireCooldown = 0f;

    public Scan(ThingInfo who, EventInfo e = null)
    {
        Setup(Actions.Scan_JuliusP, who);

        MoveMult = 0.2f;
        HaltMomentum = false;

        Duration = Mathf.Infinity;
    }

   public override void OnRun()
{
    base.OnRun();

    fireCooldown -= Time.deltaTime;

    // GET PLAYER
    ThingInfo player = God.Session.Player;

    if (player == null || player.Thing == null)
        return;

    
    if (player.Has(Traits.GainInvis_JuliusP))
        return;

    float dist = Who.Thing.Distance(player);

    if (dist > ViewDistance)
        return;

    Vector2 toPlayer =(player.Thing.transform.position - Who.Thing.transform.position).normalized;

    Vector2 forward = Who.Thing.transform.right.normalized;

    float dot = Vector2.Dot(forward, toPlayer);

    if (dot < 0.999f)
        return;

    // ONLY FIRE WHEN COOLDOWN IS READY
    if (fireCooldown > 0f)
        return;

    fireCooldown = 0.5f; // fire every 0.8 seconds

    //Debug.Log("FIRING FIREBALL");

    // SET TARGET
    Who.SetTarget(player);

    SpawnRequest req = new SpawnRequest("EnemyFire");
    ThingOption fireOpt = God.Library.GetThing(req);

    if (fireOpt == null)
    {
       // God.LogError("No EnemyFire found in library");
        return;
    }

    //OFFSET POS FOR THE FIREBALL//
    Vector2 spawnPos =  Who.Thing.transform.position + (Vector3)(Who.Thing.transform.right * 0.5f + (Who.Thing.transform.up * 0.11f));

    ThingInfo fireball = fireOpt.Create();
    fireball.Spawn(spawnPos);

    ThingController tc = fireball.Thing;

    if (tc == null)
    {
       // God.LogError("Fireball has no ThingController");
        return;
    }

    tc.AddRB();

    Vector2 dir = Who.Thing.transform.right.normalized;
    tc.ActualMove = dir * 15f;

    Who.Thing.DoAction(Actions.Chase);
}
}