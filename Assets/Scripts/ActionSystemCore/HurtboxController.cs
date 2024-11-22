using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxController : MonoBehaviour
{
    public ActorController Who;
    
    void Awake()
    {
        if (Who == null) Who = gameObject.GetComponentInParent<ActorController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Who.gameObject) return;
        HitboxController hit = other.GetComponent<HitboxController>();
        if (hit == null) return;
        if (Who.CurrentAction == null)
        {
            Debug.Log("ERROR: Hurtbox existed while actor wasn't acting");
            return;
        }

        Who.CurrentAction.HitBegin(hit.Who,this);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Who.gameObject) return;
        HitboxController hit = other.GetComponent<HitboxController>();
        if (hit == null) return;
        if (Who.CurrentAction == null)
        {
            Debug.Log("ERROR: Hurtbox existed while actor wasn't acting");
            return;
        }

        Who.CurrentAction.HitEnd(hit.Who,this);
    }
    
}
