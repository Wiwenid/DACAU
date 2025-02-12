using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class Flip : MonoBehaviour
{
    public LimbSolver2D limbSolver;

    private void Start()
    {
        limbSolver = GetComponentInChildren<LimbSolver2D>();
    }

    private void Update()
    {
        FlipObject();
    }

    private void FlipObject()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            limbSolver.flip = !limbSolver.flip;
        }
    }
}
