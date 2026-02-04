using UnityEngine;
using UnityEngine.SceneManagement;

///The trait that manages damage and healing.
public class HealthTrait : Trait
{
    public HealthTrait()
    {
        Type = Traits.Health;
        AddListen(EventTypes.Setup);   //Set MaxHP
        AddListen(EventTypes.ShownHP); //If asked for health, returns current HP
        AddListen(EventTypes.Damage);  //Lowers health, sprays blood, checks for death
        AddListen(EventTypes.Healing); //Raises health up to max
        AddListen(EventTypes.Death);   //Destroys the thing
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                //Begin the game with max health
                float hp = i.GetN();   //What's my current health?
                if(hp <= 0)            //If I have 0 or less HP, something is wrong
                    Debug.Log("INVALID HEALTH: " + i.Type + " / " + i.Who + " / " + hp);
                i.Set(NumInfo.Max,hp); //Set my MaxHP to equal my current hp at game start
                break;
            }
            case EventTypes.ShownHP:
            {
                e.Set(i.GetInt()); //Update the EventInfo with my current health
                e.Set(NumInfo.Max,i.GetInt(NumInfo.Max)); //Include my max health, just in case
                break;
            }
            case EventTypes.Damage:
            {
                int amt = God.RoundRand(e.GetN()); //How much damage does the event say to take? If a fraction, round in a semi-random direction
                if (amt == 0) return;    //If 0, then don't do anything
                if (i.Who.Thing != null) //If I still exist, spray blood
                {
                    Vector2 where = i.Who.Thing.transform.position;
                    if (i.Collision != null) where = i.Collision.Where;
                    God.Library.GetGnome("Blood").Spawn(where, amt * 3);
                }
                float hp = i.Change(-amt); //Lower my health by the amount
                if (hp <= 0)               //If that takes me to 0, send myself a Death message
                    i.Who.TakeEvent(God.E(EventTypes.Death).Set(e.GetThing()));
                break;
            }
            case EventTypes.Healing:
            {
                float hp = i.Change(e.GetN());  //Raise my health by the event's amount
                if (hp > i.Get(NumInfo.Max))    //If that takes me over my max, set me to max
                {
                    i.Set(i.Get(NumInfo.Max));
                }
                break;
            }
            case EventTypes.Death:
            {
                i.Who.Destruct(e.GetThing());  //Destroy me, giving credit to who the event says did it
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
        AddListen(EventTypes.OnSpawn);
        AddListen(EventTypes.OnTouch);
        AddListen(EventTypes.OnTouchWall);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnSpawn:
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
                float dur = e.GetFloat(NumInfo.Default,0.2f);
                i.Set(NumInfo.Max, dur);
                i.Set(NumInfo.Default, dur);
                break;
            }
            case EventTypes.Update:
            {
                float dur = i.GetFloat();
                dur -= Time.deltaTime;
                if(dur <= 0) i.Who.Destruct();
                i.Set(NumInfo.Default, dur);
                break;
            }
        }
    }
}
