using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class MoveLayerNumber : MonoBehaviour
{
    public GameObject Map;


    void Awake()
    {
        Debug.Log("Editor causes this Awake");
        foreach (Transform child in transform)
        {
            if (child.gameObject.layer == 8)
            {
                child.gameObject.layer = 9;
            }

        }
    }

    void Update()
    {
        Debug.Log("Editor causes this Update");
    }
}
