using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }


    [Client]
    public void  Update()
    {
        if (hasAuthority)
        {
            Vector3 Control = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Control.Normalize();
            if (Control.magnitude > 0.01f)
            {
                //rb.velocity = Vector3.Lerp(rb.velocity, Control, Time.deltaTime * 500f * moveSpeed);
                gameObject.transform.position += Control * Time.deltaTime * 5f;
            }

            //Debug.Log(Input.GetAxis("Horizontal")+ " " + Input.GetAxis("Vertical") );
        }



    }
}
