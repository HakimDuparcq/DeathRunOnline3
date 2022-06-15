using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class death : MonoBehaviour
{
    public GameObject player;
    public GameObject deathZone;
    void Start()
    {
        deathZone.SetActive(false);
    }

    void Update()
    {
        
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == deathZone)
        {
            Debug.Log("Collision");
        }
        Debug.Log("Collisionnnnnnnnn");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == deathZone)
        {
            Debug.Log("Entrer");
        }
        Debug.Log("Entrerrrrrrr");
    }

}
