using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public ThingController Who;
    public Collider2D Coll;

    void Awake()
    {
        if (Who == null) Who = gameObject.GetComponentInParent<ThingController>();
        if (Coll == null) Coll = GetComponent<Collider2D>();
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
}
