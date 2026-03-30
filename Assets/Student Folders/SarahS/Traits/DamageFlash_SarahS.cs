using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash_SarahS : Trait
{
    private HashSet<ThingController> currentlyFlashing = new HashSet<ThingController>();

    public DamageFlash_SarahS()
    {
        Type = Traits.DamageFlashSarahS;
        AddListen(EventTypes.Damage);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        if (e.Type == EventTypes.Damage)
        {
            if (i.Who.Thing == null || i.Who.Thing.Body == null) return;
            
            if (currentlyFlashing.Contains(i.Who.Thing)) return;

            SpriteRenderer[] sprites = i.Who.Thing.Body.GetComponentsInChildren<SpriteRenderer>();
            float flashduration = i.GetFloat(NumInfo.Time, 0.2f);
            
            God.C(FlashRed(i.Who.Thing, sprites, flashduration));
        }
    }

    private IEnumerator FlashRed(ThingController who, SpriteRenderer[] sprites, float duration)
    {
        currentlyFlashing.Add(who);

        Color[] originalColors = new Color[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            originalColors[i] = sprites[i].color;
        }

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = Color.red;
        }
        
        yield return new WaitForSeconds(duration);
        
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i] != null)
            {
                sprites[i].color = originalColors[i];
            }
        }
        
        currentlyFlashing.Remove(who);
    }
}
