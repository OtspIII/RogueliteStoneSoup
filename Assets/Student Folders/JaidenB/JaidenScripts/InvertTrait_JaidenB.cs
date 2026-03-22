using UnityEngine;

public class InvertControlsTrait : Trait
{
    public InvertControlsTrait() 
    {
        Type = Traits.InvertControls;
        AddListen(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e) 
    {
        switch (e.Type) 
        {
            case EventTypes.OnTouch:
                {

                    GameCollision col = e.Collision;
                    ThingInfo what = col.Other.Info;
                    if (col.HBMe.Type == HitboxTypes.Body)
                        what.TakeEvent(God.E(EventTypes.PlayerTouched).Set(i.Who).Set(col.HBMe));

                    float score = Mathf.Floor(i.Change(e.GetN()));
                    God.GM.SetUI("Score", "Score: " + score, 2);

                    // Vector2 vel = Vector2.zero;
                    // i.vel.x = -1;
                    // i.vel.x = 1;
                    // i.vel.y = -1;
                    // i.vel.y = 1;

                    //ThingInfo.DesiredMove(0, 0);
                }
                break;
        }
    }

}
