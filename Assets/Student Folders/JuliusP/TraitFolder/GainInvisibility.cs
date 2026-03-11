using UnityEngine;
using System.Collections;

public class GainInvisibility : Trait
{
    SpriteRenderer[] Sr;
    bool PrepareInvis = false;

    public GainInvisibility()
    {
        Type = Traits.GainInvis_JuliusP;

        AddListen(EventTypes.Update);
        AddListen(EventTypes.Damage);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                if(!PrepareInvis)
                {
                    PrepareInvis = true;

                    // GET ALL SPRITE RENDERERS/
                    Sr = i.Who.Thing.gameObject.GetComponentsInChildren<SpriteRenderer>();

                    God.C(GraduallyDisappear(2f));
                }

                break;
            }

            case EventTypes.Damage:
            {
              
                //REAPPEAR WHEN DAMAGED//
                foreach(SpriteRenderer sr in Sr)
                {
                    Color c = sr.color;
                    c.a = 1f;
                    sr.color = c;
                }

                break;
            }
        }
    }

    //FUNCTION TO GRADUALLY APPLY INVISIBILTY//
    IEnumerator GraduallyDisappear(float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / duration);

            foreach (SpriteRenderer sr in Sr)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }

            yield return null;
        }
    }
}