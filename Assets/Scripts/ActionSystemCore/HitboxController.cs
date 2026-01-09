using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public HitboxTypes Type;
    public ThingController Who;
    public BodyController Body;
    public Collider2D Coll;
    public List<ThingController> Touching;
    public float Timer = 0; //If code sets this to be over 0, OnInside won't call until it hits 0 again

    void Awake()
    {
        if (Who == null) Who = gameObject.GetComponentInParent<ThingController>();
        if (Body == null) Body = gameObject.GetComponentInParent<BodyController>();
        if (Coll == null) Coll = GetComponent<Collider2D>();
    }

    void Start()
    {
        //Let's do a quick audit of our hitbox based on our type
        Audit();
    }

    void Update()
    {
        if (Touching.Count > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Who.TakeEvent(God.E(EventTypes.OnInside).Set(this));
            }
        }
    }

    
    public void SetTeam(GameTeams t)
    {
        // Debug.Log("Set Team: " + Who.Info.Name + " / " + t + " / " + gameObject);
        //Do we need to make a distinction between bodies and attacks for layers? Maybe not??
        string layer = "Neutral";
        switch (t)
        {
            case GameTeams.Player:
            {
                layer = Type != HitboxTypes.Friendly ? "Player" : "Enemy";
                break;
            }
            case GameTeams.Enemy:
            {
                layer = Type != HitboxTypes.Friendly ? "Enemy" : "Player";
                break;
            }
        }
        if(Type == HitboxTypes.Neutral) layer = "Neutral";
        gameObject.layer = LayerMask.NameToLayer(layer);
    }
    

    private void OnValidate()
    {
        if (Coll == null) Coll = GetComponent<Collider2D>();
        if (Body == null)
        {
            Body = GetComponentInParent<BodyController>();
            if (Body != null) Body.Hitbox = this;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 where = other.GetContact(0).point;
        HitboxController hb = other.collider.GetComponent<HitboxController>();
        if (hb == null)
        {
            Who.TouchWall(where);
            return;
        }

        GameCollision col = new GameCollision(hb, this, where);
        Who.TouchStart(col);
        if(!Touching.Contains(hb.Who)) Touching.Add(hb.Who);

        // ThingController tc = other.gameObject.GetComponent<ThingController>();
        // if (tc == null)
        // {
        //     HitboxController hb = other.gameObject.GetComponent<HitboxController>();
        //     if (hb != null && hb.Who != null) tc = hb.Who;
        // }
        // if (tc != null)
        // {
        //     Who.TakeEvent(God.E(EventTypes.OnTouch).Set(tc));
        //     if(!Touching.Contains(tc)) Touching.Add(tc);
        //     //Only one because they'll call their own version
        // }
        // else
        // {
        //     Who.TakeEvent(God.E(EventTypes.OnTouchWall).Set(other.GetContact(0).point));
        // }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        HitboxController hb = other.collider.GetComponent<HitboxController>();
        if (hb == null) return;
        GameCollision col = new GameCollision(hb,this);
        Who.TouchEnd(col);
        Touching.Remove(hb.Who);
        // // ThingController tc = other.gameObject.GetComponent<ThingController>();
        // if (tc != null)
        // {
        //     Touching.Remove(tc);
        // }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        HitboxController hb = other.GetComponent<HitboxController>();
        if (hb == null)
        {
            if(!other.isTrigger)
                Who.TouchWall(Vector2.Lerp(transform.position,other.transform.position,0.5f));
            return;
        }
        GameCollision col = new GameCollision(hb, this);
        Who.TouchStart(col);
        if(!Touching.Contains(hb.Who)) Touching.Add(hb.Who);
        
        
        // HitboxController hb = other.gameObject.GetComponent<HitboxController>();
        // if (hb != null && hb.Who != null)
        // {
        //     // Debug.Log("COLL: " + hb.Who);
        //     Who.TakeEvent(God.E(EventTypes.OnTouch).Set(hb.Who));
        //     if(!Touching.Contains(hb.Who)) Touching.Add(hb.Who);
        //     //Only one because they'll call their own version
        // }
        // else
        // {
        //     Who.TakeEvent(God.E(EventTypes.OnTouchWall).Set(transform.position));
        // }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        HitboxController hb = other.GetComponent<HitboxController>();
        if (hb == null) return;
        GameCollision col = new GameCollision(hb, this);
        Who.TouchEnd(col);
        Touching.Remove(hb.Who);
        
        // HitboxController hb = other.gameObject.GetComponent<HitboxController>();
        // if (hb != null)
        // {
        //     Touching.Remove(hb.Who);
        // }
    }

    public void Audit()
    {
        if(Who == null) Debug.Log("Hitbox Without Thing: " + this);
        if(Body == null) Debug.Log("Hitbox Without Body: " + this);
        if(Coll == null) Debug.Log("Hitbox Without Collider: " + this);
        if(Type == HitboxTypes.None) Debug.Log("Hitbox Without Type: " + this + " / " + Who.Info);
        if(Type == HitboxTypes.Body && Coll.isTrigger)
            Debug.Log("Body Hitbox Set To Trigger: " + Who.Info + " / " + this);
        if((Type == HitboxTypes.Attack || Type == HitboxTypes.Zone) && !Coll.isTrigger)
            Debug.Log("Intangible Hitbox Set To Collider: " + Who.Info + " / " + this);
    }
}

public enum HitboxTypes
{
    None=0,
    Body=1,       //A physical thing's form. The player's body, a pillar's collider.
    Zone=2,       //A non-solid trigger that does some effect when entered
    Attack=3,     //A hitbox that exists as part of an action to inflict an effect. Hits only other teams
    Projectile=4, //Like an attack, but for a stand-alone object. Hits only other teams
    Neutral=5,    //Ignores teams and always hits everything
    Friendly=6,   //Only hits things on the same team
}

public class GameCollision
{
    public ThingController Other {get{return HBOther.Who;}}
    public ThingController Me {get{return HBMe.Who;}}
    public HitboxController HBOther;
    public HitboxController HBMe;
    public Vector2 Where;
    
    public GameCollision(HitboxController hbo,HitboxController hbm,Vector2 where)
    {
        HBOther = hbo;
        HBMe = hbm;
        Where = where;
    }
    
    public GameCollision(HitboxController hbo,HitboxController hbm)
    {
        HBOther = hbo;
        HBMe = hbm;
        Where = Vector2.Lerp(hbo.transform.position,hbm.transform.position,0.5f);
    }
}

