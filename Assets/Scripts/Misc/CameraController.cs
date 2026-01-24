using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //The action Camera component
    public Camera Cam;
    //The thing that I'm following around
    public GameObject Target;
    //How far can the target move from the center of the screen before we follow?
    public float Leeway = 1f;

    private void Awake()
    {
        //Register myself to a static variable to be easy to find
        God.Cam = this;
    }

    void Update()
    {
        //If there's nobody I'm following, I can end my Update early
        if (Target == null) return;
        //I make a copy of where I am that I can change if I want to move
        Vector3 where = transform.position;
        //I record where my target is. . .
        Vector3 t = Target.transform.position;
        //If the target is too far to my right, move right
        if (where.x - t.x < -Leeway)
            where.x = t.x - Leeway;
        //If the target is too far to my left, move left
        else if (where.x - t.x > Leeway)
            where.x = t.x + Leeway;
        //If the target is too far above me, move up
        if (where.y - t.y < -Leeway)
            where.y = t.y - Leeway;
        //If the target is too far below me, move down
        else if (where.y - t.y > Leeway)
            where.y = t.y + Leeway;
        //Actually go where I calculated
        transform.position = where;
    }
}
