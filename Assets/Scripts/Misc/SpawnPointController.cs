using System;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public Tags Type;

    public void Spawn()
    {
        ThingSeed who = ThingBuilder.GetTag(Type).Random();
        Instantiate(God.Library.ActorPrefab, transform.position, transform.rotation).Imprint(who);
        // if (Type == SpawnTypes.Player)
        // {
        //     Instantiate(God.Library.ActorPrefab, transform.position, transform.rotation).Imprint(ThingBuilder.Player);
        // }
        // else if (Type == SpawnTypes.Monster)
        // {
        //     ThingSeed who = ThingBuilder.GetTag("NPC").Random();
        //     Instantiate(God.Library.ActorPrefab, transform.position, transform.rotation).Imprint(who);
        // }
        Destroy(gameObject);
    }
}
