using UnityEngine;
using System.Collections.Generic;

public class SelfDestruction : Trait
{
    public SelfDestruction()
    {
        Type = Traits.SelfDestruct_JuliusP;
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                // SAFETY FOR CHECKING IF THE THING IS DEAD//
                if (i.Who == null || i.Who.Thing == null) return;

                //  CHASE PLAYER 
                ThingInfo player = God.Session.Player;
                if (player != null && player.Thing != null)
                {
                    i.Who.Thing.MoveTowards(player); // move toward player
                    i.Who.Thing.LookAt(player, 0.5f); // smoothly face player
                }
               
                // GET THE TIMER
                float TimeUntilSelfDestruct = i.Get(NumInfo.Default, 5f);

                // DECREASE THE TIMER
                TimeUntilSelfDestruct -= Time.deltaTime;

                // SAVE BACK TO TRAIT SHEET//
                i.Set(NumInfo.Default, TimeUntilSelfDestruct);

                // IF TIMER HAS REACHED ZERO
                if (TimeUntilSelfDestruct <= 0f)
                {
                    // SAFETY CHECK//
                    if (i.Who == null || i.Who.Thing == null) return;

                    //GET LOCATION OF THE ENTITY WITH THE SELF-DESTRUCT TRAIT//
                    Vector2 location = i.Who.Thing.transform.position;
                   
                    //RADIUS OF EXPLOSION//
                    float radius = 0.4f;

                    // GET ALL THINGS WITHIN EXPLOSION RADIUS
                    List<ThingInfo> possibleHits = God.GM.CollideCircle(location, radius);

                    // APPLY DAMAGE TO EACH THING WITHIN THE RADIUS//
                    foreach (ThingInfo Ti in possibleHits)
                    {
                        //SKIPS IF IT FINDS THE ENTITY WITH THE TRAIT//
                        if (Ti == null || Ti == i.Who) continue; 

                        //APPLY 1 DAMAGE//
                        EventInfo dmg = God.E(EventTypes.Damage);
                        dmg.Set(NumInfo.Default, 1f);  

                        //WHO CAUSED THE DAMAGE//     
                        dmg.Set(ThingEInfo.Source, i.Who);  

                        //CARRY OUT THE DAMAGE EVENT TO THOSE IN THE RADIUS//
                        Ti.TakeEvent(dmg, true);           
                    }

                    // SELF-DESTRUCT AFTER DAMAGING THOSE AROUND IT//
                    i.Who.Destruct();
                }

                break;
            }
        }
    }
}