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
                    if (e.Collision != null) where = e.Collision.Where;
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
                float dur = i.GetFloat(NumInfo.Default,0.2f);
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


public class RageTrait : Trait
{
    public float DamageMultiplier = 1.5f; // 50% buff

    public RageTrait()
    {
        Type = Traits.Rage;
        AddListen(EventTypes.Damage); 
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Damage:
            {
                // GET THE BASE DAMAGE //
                float baseDamage = e.GetN();       
                float buffedDamage = baseDamage * DamageMultiplier; // 50% DAMAGE BUFF
                e.Set(NumInfo.Default, buffedDamage);
                Debug.Log($"{i.Who.Name} RageTrait: damage buffed by 50% from {baseDamage} to {buffedDamage}");
                break;
            }
        }
    }
}





public class DashTrait : Trait
{
    //THIS IS HOW FAR I WANT TO MOVE//
    public float DashDistance = 3f;  

    //HOW LONG THE DASH SHOULD BE//
    public float DashDuration = 0.2f;

    private bool isDashing = false;
    private Vector3 dashDirection;
    private float dashTimer = 0f;

    public DashTrait()
    {
        Type = Traits.Dash;
        AddListen(EventTypes.Update); // Check input each frame
    }

    public override void TakeEvent(TraitInfo i, EventInfo e)
    {
        switch (e.Type)
        {
            case EventTypes.Update:
            {
                //CHECKS IF THE THING IN THE SCENE EXISTS AT ALL//
                if (i.Who.Thing == null) break;

                //GET THE INPUT FROM THE PLAYER TO BE ABLE TO DASH VERTICALLY AND HORIZONTALLY//
                float HorizontalInput = Input.GetAxisRaw("Horizontal");

                float VerticalInput = Input.GetAxisRaw("Vertical");

                

                // START DASH IF KEY PRESSED AND NOT DASHING
                if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
                {

                    //CAN DASH//
                    isDashing = true;

                    //QUICK DASH OF 0.2 SECONDS//
                    dashTimer = DashDuration;
                   
                    //CARRIES OUT THE DASHING//
                    dashDirection = new Vector3(HorizontalInput, VerticalInput, 0).normalized;
                }

                // Move while dashing
                if (isDashing)
                {
                    //DIVIDE DASHDISTANCE BY DASHDURATION TO GET HOW MUCH TO MOVE IN THE 0.2 SECONDS, 
                    //THEN MULTIPLY BY TIME.DELTATIME TO FIND HOW MUCH TO MOVE EACH FRAME//

                    //HOW MUCH TO MOVE IN THE CURRENT FRAME//
                    float MoveinSmallsteps = (DashDistance/DashDuration) * Time.deltaTime;
                   

                    //ACCESS THE TRANSFORM OF THE THING AND MULTIPLY SMALLSTEPS BY THE DASHDIRECION//
                    i.Who.Thing.transform.position += dashDirection * MoveinSmallsteps;

                    dashTimer -= Time.deltaTime;
                    if (dashTimer <= 0f)
                        isDashing = false; // stop dash
                }

                break;
            }
        }
    }
}
