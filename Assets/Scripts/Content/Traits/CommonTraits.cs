using UnityEngine;
using UnityEngine.SceneManagement;


public class HealthTrait : Trait
{
    public HealthTrait()
    {
        Type = Traits.Health;
        TakeListen.Add(EventTypes.Setup);
        TakeListen.Add(EventTypes.ShownHP);
        TakeListen.Add(EventTypes.Damage);
        TakeListen.Add(EventTypes.Healing);
        TakeListen.Add(EventTypes.Death);
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

public class PlayerTrait : Trait
{
    public PlayerTrait()
    {
        Type = Traits.Player;
        TakeListen.Add(EventTypes.Setup);
        TakeListen.Add(EventTypes.Update);
        TakeListen.Add(EventTypes.IsPlayer);
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
        }
    }
}

public class FighterTrait : Trait
{
    public FighterTrait()
    {
        Type = Traits.Fighter;
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            default: return;
        }
    }
}

public class HoldableTrait : Trait
{
    public HoldableTrait()
    {
        Type = Traits.Holdable;
        TakeListen.Add(EventTypes.GetDefaultAttack);
    }
    
    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.GetDefaultAttack:
            {
                e.Set(EnumInfo.DefaultAction, (int)i.Get<Actions>(EnumInfo.DefaultAction));
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
        TakeListen.Add(EventTypes.Start);
        TakeListen.Add(EventTypes.OnHit);
        TakeListen.Add(EventTypes.OnHitWall);
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
            case EventTypes.OnHit:
            {
                ThingInfo other = e.GetActor();
                float amt = i.GetN();
                if (amt > 0)
                {
                    other.TakeEvent(God.E(EventTypes.Damage).Set(amt));
                }
                i.Who.Destruct();
                break;
            }
            case EventTypes.OnHitWall:
            {
                i.Who.Destruct();
                break;
            }
            default: return;
        }
    }
}

public class ExitTrait : Trait
{
    public ExitTrait()
    {
        Type = Traits.Exit;
        TakeListen.Add(EventTypes.OnCollide);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnCollide:
            {
                ThingInfo t = e.GetActor();
                if (t.IsPlayer())
                {
                    SceneManager.LoadScene("YouWin");
                }

                return;
            }
            default: return;
        }
    }
}

public class DamageZoneTrait : Trait
{
    public DamageZoneTrait()
    {
        Type = Traits.DamageZone;
        TakeListen.Add(EventTypes.OnInside);
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.OnInside:
            {
                float timer = i.Get(NumInfo.Speed,1);
                float dmg = i.Get(NumInfo.Amount,1);

                // Debug.Log("INSIDE LAVA: " + timer + " / " + dmg);
                HurtboxController hb = e.Hurtbox;
                foreach (ThingController tc in hb.Inside)
                {
                    ThingInfo t = tc.Info;
                    t.TakeEvent(God.E(EventTypes.Damage).Set(dmg));
                }
                hb.Timer = timer;
                return;
            }
            default: return;
        }
    }
}
