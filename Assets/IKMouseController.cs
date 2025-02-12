using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKMouseController : MonoBehaviour
{
    public Transform IkTarget;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Control();
    }

    private void Control()
    {
        Vector3 Mouspos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Mouspos.z = 0;
        transform.position = Mouspos;
    }
}
