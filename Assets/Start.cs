using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : MonoBehaviour
{
    [SerializeField] private GameObject JianziPrefab;
    [SerializeField] private GameObject space;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            JianziPrefab.SetActive(true);
            space.SetActive(false);
            
        }
    }
}
