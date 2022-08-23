using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class MoveLayerNumber : MonoBehaviour
{
    public GameObject Map;

    public List<GameObject> listOfChildren;

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
        foreach (Transform child in Map.transform)
        {
            Debug.Log(child.name);
            if (child.gameObject.layer == 8)
            {
                child.gameObject.layer = 9;

            }

        }

        if (listOfChildren.Count == 0)
        {
            GetChildRecursive(Map);
        }
        foreach (GameObject item in listOfChildren)
        {
            if (item.gameObject.layer == 8)
            {
                item.gameObject.layer = 9;

            }
        }

    }

    private void GetChildRecursive(GameObject obj)
    {
        if (null == obj)
            return;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            listOfChildren.Add(child.gameObject);
            GetChildRecursive(child.gameObject);
        }
    }

}
