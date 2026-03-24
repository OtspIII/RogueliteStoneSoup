using UnityEngine;

public class Sneaky_qixiangdong : Trait
{
    
    public Sneaky_qixiangdong()
    {
        Type = Traits.Sneaky_qixiangdong;
        AddListen(EventTypes.OnSpawn);
        
    }
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
                Debug.Log("Sneaky Setup");

                SpriteRenderer[] sprites = i.Who.Thing.GetComponentsInChildren<SpriteRenderer>();

                foreach (SpriteRenderer s in sprites)
                {
                    s.enabled = false;
                }

                break;

        }
    }
}
