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
        AddListen(EventTypes.LoseTrait);
        AddListen(EventTypes.Damage);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
           case EventTypes.Update:
           {
            if (!PrepareInvis || Sr == null)
            {
                 PrepareInvis = true;

                 Sr = i.Who.Thing.gameObject.GetComponentsInChildren<SpriteRenderer>();

               
                 Reappear(); 
                 God.C(GraduallyDisappear(0.19f));
            }

         break;
            }

            case EventTypes.LoseTrait:
            {
              
                PrepareInvis = false;
                //REAPPEAR WHEN DAMAGED//
                Reappear();
               
                break;
            }


            case EventTypes.Damage:
            {
                    
                Reappear();

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
            float alpha = Mathf.Lerp(1f, 0.5f, time / duration);

            foreach (SpriteRenderer sr in Sr)
            {
                //SAFETY MEASURES//
                if (sr == null) continue; 
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }

            yield return null;
        }
    }


    //FUNCTION THAT MAKES THE THING APPEAR TEMPORARILY WHEN DAMAGED//
    IEnumerator ReappearTemporarily()
    {
       
        
        yield return new WaitForSeconds(0f);
        //REAPPEAR WHEN DAMAGED//
         foreach(SpriteRenderer sr in Sr)
          {
               //SAFETY MEASURES//
               if (sr == null) continue; 
               Color c = sr.color;
               c.a = 1f;
               sr.color = c;
          }


         yield return new WaitForSeconds(0.2f);

        //MAKE THE THING GRADUALLY DISAPPEAR//
        God.C(GraduallyDisappear(0.5f));


    }


    //FUNCTION TO APPEAR//
   void Reappear()
{
    if (Sr == null) return;

    foreach (SpriteRenderer sr in Sr)
    {
        if (sr == null) continue;
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;
    }
}


}