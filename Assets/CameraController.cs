using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;
    
    void Update()
    {
        if (Target == null) return;
        Vector3 where = Target.transform.position;
        where.z = -1;
        transform.position = where;
    }
}
