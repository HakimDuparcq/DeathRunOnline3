using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class raycastestName : MonoBehaviour
{
    public Text playerName;
    void Start()
    {
        
    }

    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        playerName.text ="";
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            Debug.Log("touch" + hit.transform.name);
            playerName.text = hit.transform.name;
        }
    }
}
