using UnityEngine;
using System.Collections.Generic;

public class RedGreen : Trait
{
    bool canStart = false;
    bool isRed = false;

    float distance = 20f;
    float timer = 0f;

    Dictionary<SpriteRenderer, Color> originalColors = new Dictionary<SpriteRenderer, Color>();

    public RedGreen()
    {
        Type = Traits.RedLight;

        AddListen(EventTypes.Update);
        AddListen(EventTypes.Death); 
        AddListen(EventTypes.OnSpawn);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                ThingInfo self = i.Who;
                ThingInfo player = God.Session.Player;

                int Level = God.Session.Level;
               
                if (self == null || self.Thing == null)
                    //return;

                
                if (player == null || player.Thing == null)
                {
                    ResetFloors();
                    canStart = false;
                    //return;
                }

                //GET SQR DIST/
                //GET NULL REFERENCE HERE//
                float sqrDist = (self.Thing.transform.position - player.Thing.transform.position).sqrMagnitude;


                //RESET GAME WHEN NOT IN RANGE//
                if (sqrDist > distance * distance)
                {
                    ResetFloors();
                    canStart = false;
                    return;
                }

                //DISTANCE CHECK//
                if (!canStart && sqrDist < distance * distance)
                {
                    canStart = true;
                    isRed = false;
                    timer = 0f;

                    ResetFloors();

                    God.GM.SetUI("PlayMessage", "Let's Play a Game!", 2);
                }

                if (!canStart)
                    return;

                    // ADD THIS
                    timer += Time.deltaTime;

    

                if (timer >= 2f)
                {
                    timer = 0f;
                    isRed = !isRed;

                    SetFloors(isRed ? Color.red : Color.green);

                    God.GM.SetUI("PlayMessage", isRed ? "Red Light!" : "Green Light!", 2);
                }

               
                //CHECK IF THE PLAYER IS MOVING DURING RED//
                Rigidbody2D rb = player.Thing.GetComponentInChildren<Rigidbody2D>();

                if (isRed && rb != null && rb.linearVelocity.sqrMagnitude > 0.001f)
                {
                    EventInfo dmg = God.E(EventTypes.Damage);
                    dmg.Set(NumInfo.Default, 1f);
                    player.TakeEvent(dmg);
                }

                break;
            }

            //SETS COLOR OF FLOOR BACK ON DEATH//
            case EventTypes.Death:
            {
                ResetFloors();
                canStart = false;
                isRed = false;
                timer = 0f;

                God.GM.SetUI("PlayMessage", null , 2);
                break;
            }


            case EventTypes.OnSpawn:
            {
                    
                ThingInfo Player = God.Session.Player;


                if(Player != null && !i.Who.Has(Traits.RedLight))
                {
                        
                    i.Who.AddTrait(Traits.RedLight);


                 }


                break;



            }
        }
    }

   //SETS COLOR OF FLOOR TP BE RED OR GREEN//
    void SetFloors(Color c)
    {
        ThingController[] all = Object.FindObjectsOfType<ThingController>();

        foreach (ThingController tc in all)
        {
            if (tc == null) continue;
            if (!tc.name.ToLower().Contains("floor")) continue;

            SpriteRenderer sr = tc.GetComponentInChildren<SpriteRenderer>();
            if (sr == null) continue;

  
            if (!originalColors.ContainsKey(sr))
            {
                originalColors[sr] = sr.color;
            }

            sr.color = c;
        }
    }

    //PUTS THE COLOR BACK TO NORMAL//
    void ResetFloors()
    {
        foreach (var pair in originalColors)
        {
            if (pair.Key != null)
            {
                pair.Key.color = pair.Value;
            }
        }

        originalColors.Clear();
    }
}