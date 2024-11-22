using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Cam;
    public GameObject Target;

    private void Awake()
    {
        God.Cam = this;
    }

    void Update()
    {
        if (Target == null) return;
        Vector3 where = Target.transform.position;
        where.z = -1;
        transform.position = where;
    }
}
