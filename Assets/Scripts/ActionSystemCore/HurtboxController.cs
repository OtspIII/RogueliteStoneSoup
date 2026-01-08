using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    public ThingController Who;
    public Collider2D Coll;
    public float Timer = 0;
    public List<ThingController> Inside;
    
    void Awake()
    {
        if (Who == null) Who = gameObject.GetComponentInParent<ThingController>();
    }

    private void Update()
    {
        if (Inside.Count > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                Who.TakeEvent(God.E(EventTypes.OnHitInside).Set(this));
            }
        }
    }

    public void StartHit(ThingController other)
    {
        if (Who.ActorTrait != null)
        {
            if(Who.ActorTrait.Action != null) 
                Who.ActorTrait.Action.HitBegin(other,this);
        }
        
        Who.TakeEvent(God.E(EventTypes.OnHit).Set(other.Info));
        if(!Inside.Contains(other))
            Inside.Add(other);
    }

    public void EndHit(ThingController other)
    {
        if (Who.ActorTrait != null)
        {
            if(Who.ActorTrait.Action != null) 
                Who.ActorTrait.Action.HitEnd(other,this);
        }
        Inside.Remove(other);
    }

    public void StartHitWall()
    {
        Who.TakeEvent(God.E(EventTypes.OnHitWall).Set());
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Who.gameObject ) return; //|| (Who.Info.ChildOf != null && other.gameObject == Who.Info.ChildOf.Thing.gameObject)
        HitboxController hit = other.GetComponent<HitboxController>();
        // Debug.Log("OTE HURTBOX: " + Who + " / " + hit);
        if (hit)
        {
            StartHit(hit.Who);
            return;
        }

        HurtboxController hurt = other.GetComponent<HurtboxController>();
        if (hurt != null)
        {
            Who.TakeEvent(God.E(EventTypes.OnClash).Set(hurt));
        }
        else if (!other.isTrigger) StartHitWall();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Who.gameObject) return;
        ThingController who = other.gameObject.GetComponent<ThingController>();
        if (who) StartHit(who);
        else StartHitWall();
        //I don't think we need a Hurt on Hurt test because hurtboxes should always be triggers
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Who.gameObject ) return; //|| (Who.Info.ChildOf != null && other.gameObject == Who.Info.ChildOf.Thing.gameObject)
        HitboxController hit = other.GetComponent<HitboxController>();
        if (hit != null) EndHit(hit.Who);
        
    }
    
    public void SetPlayer()
    {
        gameObject.layer = LayerMask.NameToLayer("PHurtbox");
        // else gameObject.layer = LayerMask.NameToLayer("MHurtbox");
    }
    
    private void OnValidate()
    {
        if (Coll == null) Coll = GetComponent<Collider2D>();
        if (transform.parent != null)
        {
            BodyController bc = transform.parent.gameObject.GetComponent<BodyController>();
            if (bc != null) bc.Hurtbox = this;
        }
    }
}
