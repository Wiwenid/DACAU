using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDistanceDebugger : MonoBehaviour
{
    private DistanceJoint2D joint;
    
    void Start()
    {
        joint = GetComponent<DistanceJoint2D>();
    }

    private void Update()
    {
        Debug.Log("Distance: " + joint.distance);
    }
}
