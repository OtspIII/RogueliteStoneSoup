using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public ActorController Who;
    public Collider2D Coll;

    void Awake()
    {
        if (Who == null) Who = gameObject.GetComponentInParent<ActorController>();
    }

    public void SetPlayer(bool isPlayer=true)
    {
        if(isPlayer) gameObject.layer = LayerMask.NameToLayer("PHitbox");
        else gameObject.layer = LayerMask.NameToLayer("MHitbox");
    }

}
