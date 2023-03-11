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
    public LayerMask playerMask;

    Vector3 velocity;
    bool isGrounded;
    bool isOnPlayer;
    public bool canMove = true;

    public Animator animator;
    private float x;
    private float z;


    private Vector3 lastPos; //for footstep
    private float _speed;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isOnPlayer = Physics.CheckSphere(groundCheck.position, groundDistance, playerMask);


        _speed = (transform.position - lastPos).magnitude / Time.deltaTime;
        lastPos = transform.position;
        if (isGrounded && _speed > 0.1f)  //for audio   
        {
            //AudioManager.instance.Play("Footstep");
            gameObject.GetComponent<PlayerReferences>().Audio.GetComponent<AudioPlayerManager>().Play("Footstep");
        }

        if (!gameObject.GetComponent<PlayerSetup>().isLocalPlayer)
        {
            return;
        }





        if (Cursor.lockState == CursorLockMode.None || ChatBehaviour.instance.inputField.isFocused)
        {
            x = 0;
            z = 0;
        }
        else
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        Vector2 direction = new Vector2 (controller.velocity.x, controller.velocity.z);
        if (direction.magnitude/8f>0.2f)
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
        /*if (canMove  && controller.enabled)
        {
            controller.Move(move * speed * Time.deltaTime);
        }*/
        
        



        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("jump", true);
        }
        if (!isGrounded  && Cursor.lockState == CursorLockMode.Locked && canMove)//  && MainGame.instance.playersIsAliveServer[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)])
        {
            animator.SetBool("jump", false);
        }

        if (isOnPlayer)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            /*if (controller.enabled)
            {
                controller.Move(velocity * Time.deltaTime);
            }*/
            
        }

        if (canMove && controller.enabled)
        {
            controller.Move((move * speed + velocity) * Time.deltaTime);

        }



        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.Locked &&  MainGame.instance.playersIsAliveServer[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)] && gameObject.GetComponent<Fight>().AllowToClick)
        {
            animator.SetBool("attack", true);
            gameObject.GetComponent<CrossHairs>().CrossHairsActivationAnim();
            StartCoroutine(WaitDisableAttackAnim(0.5f));
        }

    }

    IEnumerator WaitDisableAttackAnim(float sec)
    {
        yield return new WaitForSeconds(sec);
        animator.SetBool("attack", false);
    }

    
}
