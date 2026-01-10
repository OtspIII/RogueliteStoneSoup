using UnityEngine;
using UnityEngine.SceneManagement;


public class HealthTrait : Trait
{
    public HealthTrait()
    {
        Type = Traits.Health;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.ShownHP);
        AddListen(EventTypes.Damage);
        AddListen(EventTypes.Healing);
        AddListen(EventTypes.Death);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                //Begin the game with max health
                float hp = i.GetN();
                if(hp <= 0) Debug.Log("INVALID HEALTH: " + i.Type + " / " + i.Who + " / " + hp);
                i.Set(NumInfo.Max,hp);
                break;
            }
            case EventTypes.ShownHP:
            {
                e.Set(i.GetInt());
                e.Set(NumInfo.Max,i.GetInt(NumInfo.Max));
                break;
            }
            case EventTypes.Damage:
            {
                // Debug.Log("Damage: " + i.Get(NumInfo.Amount) + " / " + e.Get(NumInfo.Amount));
                float hp = i.Change(-e.GetN());
                if (hp <= 0)
                {
                    i.Who.TakeEvent(EventTypes.Death);
                }
                break;
            }
            case EventTypes.Healing:
            {
                float hp = i.Change(e.GetN());
                if (hp > i.Get(NumInfo.Max))
                {
                    i.Set(i.Get(NumInfo.Max));
                }
                break;
            }
            case EventTypes.Death:
            {
                GameObject.Destroy(i.Who.Thing.gameObject);
                break;
            }
        }
    }
}

public class ProjectileTrait : Trait
{
    public ProjectileTrait()
    {
        Type = Traits.Projectile;
        AddListen(EventTypes.Start);
        AddListen(EventTypes.OnTouch);
        AddListen(EventTypes.OnTouchWall);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Start:
            {
                ThingController who = i.Who.Thing;
                float spd = i.Get(NumInfo.Speed,10);
                who.CurrentSpeed = spd;
                who.AddRB();
                who.RB.linearVelocity = who.transform.up * spd;
                break;
            }
            case EventTypes.OnTouch:
            {
                GameCollision col = e.Collision;
                if (col.HBOther.Type != HitboxTypes.Body && col.HBOther.Coll.isTrigger) break;
                ThingInfo other = col.Other.Info;
                float amt = i.GetN();
                if (amt > 0)
                {
                    other.TakeEvent(God.E(EventTypes.Damage).Set(amt));
                }
                i.Who.Destruct();
                break;
            }
            case EventTypes.OnTouchWall:
            {
                i.Who.Destruct();
                break;
            }
            default: return;
        }
    }
}
