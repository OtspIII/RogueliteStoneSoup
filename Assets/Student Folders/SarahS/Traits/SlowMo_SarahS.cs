using UnityEngine;

public class SlowMo_SarahS : Trait
{
    private float trapRadius = 1.5f;
    private float slowMult = 0.1f;
    private bool playerSlowed = false;
    public SlowMo_SarahS()
    {
        Type = Traits.SlowMoSarahS;
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.Update);
        AddListen(EventTypes.OnDestroy);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
            {
                if (i.Who.Thing != null && i.Who.Thing.Body != null)
                {
                    Collider2D[] colliders = i.Who.Thing.Body.gameObject.GetComponentsInChildren<Collider2D>();
                    foreach (Collider2D col in colliders)
                    {
                        col.isTrigger = true;
                    }
                }

                break;
            }

            case EventTypes.Update:
            {
                if (i.Who.Thing == null || God.Player == null || God.Player.Thing == null) return;
                float dist = Vector2.Distance(i.Who.Thing.transform.position, God.Player.Thing.transform.position);
                if (dist < trapRadius)
                {
                    if (!playerSlowed)
                    {
                        God.Player.CurrentSpeed *= slowMult;
                        playerSlowed = true;
                        Debug.Log("player slowed");
                    }
                }
                else
                {
                    if (playerSlowed)
                    {
                        God.Player.CurrentSpeed /= slowMult;
                        playerSlowed = false;
                        Debug.Log("player sped up");
                    }
                }

                break;
            }
            case EventTypes.OnDestroy:
            {
                if (playerSlowed && God.Player != null)
                {
                    God.Player.CurrentSpeed /= slowMult;
                }

                break;
            }
        }
    }
}
