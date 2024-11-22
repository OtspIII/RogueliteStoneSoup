using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : ActorController
{
    public ActorController Target;
    public float AttackRange = 1.5f;
    public float VisionRange = 4;

    public override void OnAwake()
    {
        base.OnAwake();
        Speed = 3;
        DefaultAction = new PatrolAction(this);
    }
    
    public override void OnStart()
    {
        base.OnStart();
        if (Target == null) Target = GameObject.FindObjectOfType<PlayerController>();
    }
    
    
}
