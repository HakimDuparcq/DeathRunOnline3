using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewPlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public Animator animator;
    private float x;
    private float z;
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            x = 0;
            z = 0;
        }
        else
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

            

        if (x != 0 || z != 0)
        {
            animator.SetBool("running", true);
            animator.SetBool("idl", false);
        }
        else
        {
            animator.SetBool("running", false);
            animator.SetBool("idl", true);
        }

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude > 0.9f)
        {
            move = Vector3.Normalize(move);
        }
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("jump", true);
        }
        if (!isGrounded)
        {
            animator.SetBool("jump", false);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        if (move != Vector3.zero && isGrounded)  //for audio
        {
            AudioManager.instance.Play("Footstep");
        }


        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.Locked)
        {
            animator.SetBool("attack", true);
            StartCoroutine(WaitDisableAttackAnim(0.9f));
        }

            
    }

    IEnumerator WaitDisableAttackAnim(float sec)
    {
        yield return new WaitForSeconds(sec);
        animator.SetBool("attack", false);
    }

    
}
