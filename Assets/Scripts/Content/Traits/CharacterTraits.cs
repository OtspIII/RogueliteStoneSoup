using System.Collections.Generic;
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
        TakeListen.Add(EventTypes.OnTouchEnd);
        TakeListen.Add(EventTypes.DidPickup);
        TakeListen.Add(EventTypes.DidDrop);
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
                if(i.Who.CurrentWeapon != null)
                    i.Who.TakeEvent(God.E(EventTypes.DidPickup).Set(i.Who.CurrentWeapon));
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

                if (Input.GetKeyDown(KeyCode.Mouse0))i.Who.TakeEvent(God.E(EventTypes.UseHeldStart));
                if (Input.GetKey(KeyCode.Mouse0))i.Who.TakeEvent(God.E(EventTypes.UseHeld));
                else if (i.Who.CurrentWeapon == null && God.GM.PlayerInventory.Count > 0)
                {
                    i.Who.SetWeapon(God.GM.PlayerInventory[God.GM.InventoryIndex-1]);
                }
                // {
                    // i.Who.TakeEvent(God.E(EventTypes.StartAction).SetEnum(EnumInfo.Action,(int)Actions.DefaultAttack));
                    // i.Who.Thing.DoAction(Actions.DefaultAttack);
                // }
                    


                if (Input.GetKey(KeyCode.E))
                {
                    List<ThingController> touching = i.Who.Thing.GetTouching();
                    foreach (ThingController t in touching)
                    {
                        t.TakeEvent(God.E(EventTypes.Interact).Set(i.Who));
                    }
                }

                for (int n = 0; n < God.InvKeys.Count; n++)
                {
                    if (Input.GetKeyDown(God.InvKeys[n]) && God.GM.PlayerInventory.Count > n)
                    {
                        God.GM.InventoryIndex = n+1;
                        God.Player.SetWeapon(God.GM.PlayerInventory[n]);
                        God.GM.UpdateInvText();
                    }
                        
                }
                if (Input.GetKey(KeyCode.Alpha1)) vel.y = -1;
        
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
                what.TakeEvent(God.E(EventTypes.PlayerTouched).Set(i.Who));
                break;
            }
            case EventTypes.OnTouchEnd:
            {
                GameCollision col = e.Collision;
                ThingInfo what = col.Other.Info;
                what.TakeEvent(God.E(EventTypes.PlayerLeft).Set(i.Who));
                break;
            }
            case EventTypes.DidPickup:
            {
                God.GM.AddInventory(e.GetActor());
                break;
            }
            case EventTypes.DidDrop:
            {
                God.GM.RemoveInventory(e.GetActor());
                break;
            }
        }
    }
}
