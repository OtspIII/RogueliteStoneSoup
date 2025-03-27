using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    public ThingController Who;
    
    void Awake()
    {
        if (Who == null) Who = gameObject.GetComponentInParent<ThingController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Who.gameObject) return;
        HitboxController hit = other.GetComponent<HitboxController>();
        if (hit == null) return;
        if (Who.ActorTrait.Action == null)
        {
            Debug.Log("ERROR: Hurtbox existed while actor wasn't acting");
            return;
        }

        Who.ActorTrait.Action.HitBegin(hit.Who,this);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Who.gameObject) return;
        HitboxController hit = other.GetComponent<HitboxController>();
        if (hit == null) return;
        if (Who.ActorTrait.Action == null)
        {
            Debug.Log("ERROR: Hurtbox existed while actor wasn't acting");
            return;
        }

        Who.ActorTrait.Action.HitEnd(hit.Who,this);
    }
    
    public void SetPlayer(bool isPlayer=true)
    {
        if(isPlayer) gameObject.layer = LayerMask.NameToLayer("PHurtbox");
        else gameObject.layer = LayerMask.NameToLayer("MHurtbox");
    }
    
}
