using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerTrait : Trait
{
    public PlayerTrait()
    {
        Type = Traits.Player;
        TakeListen.Add(EventTypes.Setup);
        TakeListen.Add(EventTypes.Update);
        TakeListen.Add(EventTypes.IsPlayer);
        TakeListen.Add(EventTypes.OnTouch);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        // Debug.Log("TAKE EVENT PLAYER: " + i.Type);
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                // Debug.Log("SETUP PLAYER");
                God.Player = i.Who;
                i.Set(EnumInfo.DefaultAction, (int)Actions.Idle);
                God.Cam.Target = i.Who.Thing.gameObject;
                break;
            }
            case EventTypes.Update:
            {
                Vector2 vel = Vector2.zero;
                if (Input.GetKey(KeyCode.D)) vel.x = 1;
                if (Input.GetKey(KeyCode.A)) vel.x = -1;
                if (Input.GetKey(KeyCode.W)) vel.y = 1;
                if (Input.GetKey(KeyCode.S)) vel.y = -1;
                i.Who.DesiredMove = vel;
        
                if(Input.GetKey(KeyCode.Mouse0))
                    i.Who.Thing.DoAction(Actions.DefaultAttack);
        
                if(i.Who.ActorTrait.Action.CanRotate) i.Who.Thing.LookAt(God.Cam.Cam.ScreenToWorldPoint(Input.mousePosition),0.1f);
                break;
            }
            case EventTypes.IsPlayer:
            {
                i.Set(BoolInfo.Default,true);
                break;
            }
            case EventTypes.OnTouch:
            {
                GameCollision col = e.Collision;
                ThingInfo what = col.Other.Info;
                Debug.Log("PLAYER TOUCHED: " + what);
                what.TakeEvent(God.E(EventTypes.TryPickup).Set(i.Who));
                break;
            }
        }
    }
}
