using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ActorController
{
    private Camera Cam;

    public override void OnAwake()
    {
        base.OnAwake();
        Speed = 5;
        DefaultAction = new IdleAction(this);
        Cam = Camera.main;
    }


    public override void OnUpdate()
    {
        base.OnUpdate();
        Vector2 vel = Vector2.zero;
        if (Input.GetKey(KeyCode.D)) vel.x = 1;
        if (Input.GetKey(KeyCode.A)) vel.x = -1;
        if (Input.GetKey(KeyCode.W)) vel.y = 1;
        if (Input.GetKey(KeyCode.S)) vel.y = -1;
        DesiredMove = vel;
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
            DoAction(new SwingAction(this));
        
        if(CurrentAction.CanRotate) LookAt(Cam.ScreenToWorldPoint(Input.mousePosition),0.1f);
    }
}
