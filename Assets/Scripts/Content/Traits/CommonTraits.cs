using UnityEngine;


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
                GameObject.Destroy(i.Who.gameObject);
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
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Setup:
            {
                God.Player = i.Who;
                i.Set(EnumInfo.DefaultAction, (int)Actions.Idle);
                God.Cam.Target = i.Who.gameObject;
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
                    i.Who.DoAction(Actions.DefaultAttack);
        
                if(i.Who.ActorTrait.Action.CanRotate) i.Who.LookAt(God.Cam.Cam.ScreenToWorldPoint(Input.mousePosition),0.1f);
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
            
        }
    }
}

public class WeaponTrait : Trait
{
    public WeaponTrait()
    {
        Type = Traits.Weapon;
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
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            
        }
    }
}

