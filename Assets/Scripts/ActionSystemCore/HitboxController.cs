using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public ActorController Who;

    void Awake()
    {
        if (Who == null) Who = gameObject.GetComponentInParent<ActorController>();
    }
}
