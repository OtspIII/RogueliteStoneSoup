using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    public ThingController Who;
    public Collider2D Coll;
    
    void Awake()
    {
        if (Who == null) Who = gameObject.GetComponentInParent<ThingController>();
    }

    public void StartHit(ThingController other)
    {
        if (Who.ActorTrait != null)
        {
            if(Who.ActorTrait.Action != null) 
                Who.ActorTrait.Action.HitBegin(other,this);
        }
        
        Who.TakeEvent(God.E(EventTypes.OnHit).Set(other.Info));
    }

    public void EndHit(ThingController other)
    {
        if (Who.ActorTrait != null)
        {
            if(Who.ActorTrait.Action != null) 
                Who.ActorTrait.Action.HitEnd(other,this);
        }
        
    }

    public void StartHitWall()
    {
        //Maybe this should be something? I don't think it works though yet
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Who.gameObject ) return; //|| (Who.Info.ChildOf != null && other.gameObject == Who.Info.ChildOf.Thing.gameObject)
        HitboxController hit = other.GetComponent<HitboxController>();
        Debug.Log("OTE HURTBOX: " + Who + " / " + hit);
        if (hit) StartHit(hit.Who);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == Who.gameObject) return;
        ThingController who = other.gameObject.GetComponent<ThingController>();
        if (who) StartHit(who);
        else StartHitWall();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Who.gameObject ) return; //|| (Who.Info.ChildOf != null && other.gameObject == Who.Info.ChildOf.Thing.gameObject)
        HitboxController hit = other.GetComponent<HitboxController>();
        if (hit != null) EndHit(hit.Who);
        
    }
    
    public void SetPlayer(bool isPlayer=true)
    {
        if(isPlayer) gameObject.layer = LayerMask.NameToLayer("PHurtbox");
        else gameObject.layer = LayerMask.NameToLayer("MHurtbox");
    }
    
}
