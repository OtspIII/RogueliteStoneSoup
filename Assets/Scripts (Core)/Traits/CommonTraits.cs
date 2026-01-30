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
                int amt = e.GetInt();
                if (amt == 0) return;
                if (i.Who.Thing != null)
                {
                    Vector2 where = i.Who.Thing.transform.position;
                    if (i.Collision != null) where = i.Collision.Where;
                    God.Library.GetGnome("Blood").Spawn(where, amt * 3);
                }
                float hp = i.Change(-amt);
                if (hp <= 0)
                {
                    i.Who.TakeEvent(God.E(EventTypes.Death).Set(e.GetActor()));
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
                i.Who.Destruct(e.GetActor());
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
                who.Info.CurrentSpeed = spd;
                who.AddRB();
                who.ActualMove = who.transform.up * spd;
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
                    other.TakeEvent(God.E(EventTypes.Damage).Set(amt).Set(i.Who?.GetOwner()));
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

public class DropTrait : Trait
{
    public DropTrait()
    {
        Type = Traits.Drop;
        AddListen(EventTypes.OnDestroy);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnDestroy:
            {
                ThingOption o = i.GetOption();
                if (o == null)
                {
                    SpawnRequest t = i.SpawnReq;
                    o = God.Library.GetThing(t);
                }
                ThingInfo exp = o.Create();
                exp.ChildOf = i.Who;
                exp.Spawn(e.GetVector());
                break;
            }
        }
    }
}


public class DespawnTrait : Trait
{
    public DespawnTrait()
    {
        Type = Traits.Despawn;
        AddListen(EventTypes.Setup);
        AddListen(EventTypes.Update);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                float dur = e.GetFloat(NumInfo.Amount,0.2f);
                i.Set(NumInfo.Max, dur);
                i.Set(NumInfo.Amount, dur);
                break;
            }
            case EventTypes.Update:
            {
                float dur = i.GetFloat();
                dur -= Time.deltaTime;
                if(dur <= 0) i.Who.Destruct();
                i.Set(NumInfo.Amount, dur);
                break;
            }
        }
    }
}
