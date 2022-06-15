                                        using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZombieController : MonoBehaviour
{
    public Animator Animator;
    public int xmove = Animator.StringToHash("xmove");
    public int ymove = Animator.StringToHash("ymove");

    public float un;
    public float deux;
    void Start()
    {
        
    }

    void Update()
    {
        Animator.SetFloat(xmove, un);
        Animator.SetFloat(ymove, deux);

    }
}
