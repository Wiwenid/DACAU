using UnityEngine;

public class HideCursor : MonoBehaviour
{
    private bool cursorHidden = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            cursorHidden = !cursorHidden; // Toggle state

            if (cursorHidden)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }
        }
    }
}
