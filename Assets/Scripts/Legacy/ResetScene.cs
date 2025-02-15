using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    public GameObject ball;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(ball, transform.position, Quaternion.identity);
            HideCursor();
        }
    }

    private void HideCursor()
    {
        Cursor.visible = false;
    }
}