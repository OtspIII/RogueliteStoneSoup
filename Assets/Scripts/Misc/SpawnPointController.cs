using System;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public GameTags Type;

    public void Spawn()
    {
        if (Type == GameTags.Exit)
        {
            Instantiate(God.Library.ExitPrefab, transform.position, transform.rotation);//temp
            Destroy(gameObject);
            return;
        }
        
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
