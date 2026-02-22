using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class PlayerTrait : Trait
{
    public PlayerTrait()
    {
        Type = Traits.Player;
        // AddListen(EventTypes.Setup, 5);
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.Update);
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
            // case EventTypes.Setup:
            // {
            //     // Debug.Log("SETUP PLAYER");
            //     
            //     // i.Set(EnumInfo.DefaultAction, (int)Actions.Idle);
            //     
            //     break;
            // }
            case EventTypes.OnSpawn:
            {
                God.Player = i.Who;
                God.Cam.Target = i.Who.Thing.gameObject;
                if(i.Who.CurrentHeld != null)
                    i.Who.TakeEvent(God.E(EventTypes.DidPickup).Set(i.Who.CurrentHeld));
                EventInfo hpi = i.Who.Ask(EventTypes.ShownHP);
                God.GM.SetUI("Health",hpi.GetInt()+"/"+hpi.GetInt(NumInfo.Max),1);
                God.GM.SetUI("Score","Score: "+Mathf.Floor(i.GetN()),2);
                God.GM.UpdateInvText();
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
                if (i.Who.ActorTrait.ActScript.Priority <= 0)
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))i.Who.TakeEvent(God.E(EventTypes.UseHeldStart).Set(God.MouseLoc()));
                    if (Input.GetKey(KeyCode.Mouse0))i.Who.TakeEvent(God.E(EventTypes.UseHeld).Set(God.MouseLoc()));
                    else if (i.Who.CurrentHeld == null)
                    {
                        i.Who.SetHeld(God.Session.InventoryIndex - 1);//God.Session.PlayerInventory[God.GM.InventoryIndex-1]);
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
                    if (Input.GetKeyDown(God.InvKeys[n]) && God.Session.PlayerInventory.Count > n)
                    {
                        God.Session.InventoryIndex = n + 1;
                        God.Player.SetHeld(God.Session.PlayerInventory[n]);
                        God.GM.UpdateInvText();
                    }

                }
                
                if(i.Who.ActorTrait.ActScript.CanRotate) i.Who.Thing.LookAt(God.Cam.Cam.ScreenToWorldPoint(Input.mousePosition),0.1f);
                break;
            }
            case EventTypes.OnTouch:
            {
                GameCollision col = e.Collision;
                ThingInfo what = col.Other.Info;
                if(col.HBMe.Type == HitboxTypes.Body)
                    what.TakeEvent(God.E(EventTypes.PlayerTouched).Set(i.Who).Set(col.HBMe));
                break;
            }
            case EventTypes.OnTouchEnd:
            {
                GameCollision col = e.Collision;
                ThingInfo what = col.Other.Info;
                if(col.HBMe.Type == HitboxTypes.Body)
                    what.TakeEvent(God.E(EventTypes.PlayerLeft).Set(i.Who).Set(col.HBMe));
                break;
            }
            case EventTypes.DidPickup:
            {
                God.Session.AddInventory(e.GetThing());
                break;
            }
            case EventTypes.DidDrop:
            {
                God.Session.RemoveInventory(e.GetThing());
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
                God.Session.PlayerDeath(e);
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

public class HostileTrait : Trait
{
    public HostileTrait()
    {
        Type = Traits.Hostile;
        AddListen(EventTypes.OnSee);
        AddListen(EventTypes.OnTargetDie);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSee:
            {
                ThingInfo t = e.GetThing();
                //If the thing I saw is a valid target, and also I either don't have a target or I can't see my target. . . 
                if (IsTarget(i,t) && (i.Who.Target == null || !i.Who.CanSee(i.Who.Target)))
                {
                    i.Who.Target = t;
                }
                break;
            }
            case EventTypes.OnTargetDie:
            {
                if (i.Who.Thing == null) return;
                ThingInfo targ = e.GetThing();               //Who died
                ThingInfo killer = e.Get(ThingEInfo.Source); //Who killed them
                Vector3 deathSite = e.GetVector();        //Where did they die
                //Make a list of things I might target next
                List<ThingInfo> newTarg = new List<ThingInfo>();
                //Look at everything I can see and see if they're a valid target
                foreach (ThingInfo t in i.Who.Thing.SeenThings)
                {
                    if(IsTarget(i,t)) newTarg.Add(t);
                }
                //If I see any valid targets, pick a new random one to attack
                if (newTarg.Count > 0)
                    i.Who.Target = newTarg.Random();
                
                break;
            }
        }
    }

    public virtual bool IsTarget(TraitInfo i,ThingInfo t)
    {
        if (i.Who.Team == GameTeams.Neutral) return true;
        GameTeams opp = i.Who.Team == GameTeams.Enemy ? GameTeams.Player : GameTeams.Enemy;
        if (t.Team == opp) return true;
        if (t.Team == GameTeams.Neutral && t.Has(Traits.Hostile)) return true;
        return false;
    }
}