using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public ThingController Who;
    public Collider2D Coll;
    public List<ThingController> Touching;

    void Awake()
    {
        if (Who == null) Who = gameObject.GetComponentInParent<ThingController>();
        if (Coll == null) Coll = GetComponent<Collider2D>();
    }

    void Update()
    {
        foreach (ThingController t in Touching)
        {
            Who.TakeEvent(God.E(EventTypes.OnTouchInside).Set(t));   
        }
    }

    public void SetPlayer()
    {
        gameObject.layer = LayerMask.NameToLayer("PHitbox");
        // else if(!Coll.isTrigger) gameObject.layer = LayerMask.NameToLayer("MHitbox");
    }

    private void OnValidate()
    {
        if (Coll == null) Coll = GetComponent<Collider2D>();
        if (transform.parent != null)
        {
            BodyController bc = transform.parent.gameObject.GetComponent<BodyController>();
            if (bc != null) bc.Hitbox = this;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        ThingController tc = other.gameObject.GetComponent<ThingController>();
        if (tc != null)
        {
            Who.TakeEvent(God.E(EventTypes.OnTouch).Set(tc));
            if(!Touching.Contains(tc)) Touching.Add(tc);
            //Only one because they'll call their own version
        }
        else
        {
            Who.TakeEvent(God.E(EventTypes.OnTouchWall).Set(other.GetContact(0).point));
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        ThingController tc = other.gameObject.GetComponent<ThingController>();
        if (tc != null)
        {
            Touching.Remove(tc);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        HitboxController hb = other.gameObject.GetComponent<HitboxController>();
        if (hb != null && hb.Who != null)
        {
            // Debug.Log("COLL: " + hb.Who);
            Who.TakeEvent(God.E(EventTypes.OnTouch).Set(hb.Who));
            if(!Touching.Contains(hb.Who)) Touching.Add(hb.Who);
            //Only one because they'll call their own version
        }
        else
        {
            Who.TakeEvent(God.E(EventTypes.OnTouchWall).Set(transform.position));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        HitboxController hb = other.gameObject.GetComponent<HitboxController>();
        if (hb != null)
        {
            Touching.Remove(hb.Who);
        }
    }
}
