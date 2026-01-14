using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerTrait : Trait
{
    public PlayerTrait()
    {
        Type = Traits.Player;
        AddListen(EventTypes.Setup,5);
        AddListen(EventTypes.Update);
        AddListen(EventTypes.IsPlayer);
        AddListen(EventTypes.OnTouch);
        AddListen(EventTypes.OnTouchEnd);
        AddListen(EventTypes.DidPickup);
        AddListen(EventTypes.DidDrop);
        AddListen(EventTypes.Damage,5);
        AddListen(EventTypes.Healing,5);
        AddListen(EventTypes.Death,5);
        AddListen(EventTypes.AddScore);
        AddListen(EventTypes.GetScore);
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
                EventInfo hpi = i.Who.Ask(EventTypes.ShownHP);
                God.GM.SetUI("Health",hpi.GetInt()+"/"+hpi.GetInt(NumInfo.Max),1);
                God.GM.SetUI("Score","Score: "+Mathf.Floor(i.GetN()),2);
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
                if (i.Who.ActorTrait.Action.Priority <= 0)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))i.Who.TakeEvent(God.E(EventTypes.UseHeldStart));
                    if (Input.GetKey(KeyCode.Mouse0))i.Who.TakeEvent(God.E(EventTypes.UseHeld));
                    else if (i.Who.CurrentWeapon == null)
                    {
                        i.Who.SetWeapon(God.GM.InventoryIndex - 1);//God.GM.PlayerInventory[God.GM.InventoryIndex-1]);
                    }
                    if (Input.GetKey(KeyCode.E))
                    {
                        List<ThingController> touching = i.Who.Thing.GetTouching();
                        foreach (ThingController t in touching)
                        {
                            t.TakeEvent(God.E(EventTypes.Interact).Set(i.Who));
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        i.Who.DropHeld(false);
                    }
                }
                // if (Input.GetKeyUp(KeyCode.Mouse0)){i.Who.TakeEvent(God.E(EventTypes.UseHeldEnd));}//Done via Use action now
                for (int n = 0; n < God.InvKeys.Count; n++)
                {
                    if (Input.GetKeyDown(God.InvKeys[n]) && God.GM.PlayerInventory.Count > n)
                    {
                        God.GM.InventoryIndex = n + 1;
                        God.Player.SetWeapon(God.GM.PlayerInventory[n]);
                        God.GM.UpdateInvText();
                    }

                }
                
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
            case EventTypes.Healing:
            case EventTypes.Damage:
            {
                EventInfo hpi = i.Who.Ask(EventTypes.ShownHP);
                God.GM.SetUI("Health",hpi.GetInt()+"/"+hpi.GetInt(NumInfo.Max),1);
                break;
            }
            case EventTypes.Death:
            {
                God.GM.SetUI("Health","GAME OVER",1);
                break;
            }
            case EventTypes.AddScore:
            {
                float score = Mathf.Floor(i.Change(e.GetN()));
                God.GM.SetUI("Score","Score: "+score,2);
                break;
            }
            case EventTypes.GetScore:
            {
                e.Set(i.GetFloat());
                break;
            }
        }
    }
}
