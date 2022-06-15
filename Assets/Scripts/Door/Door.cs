using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Door : MonoBehaviour
{
    public int DoorNumber;
    public LeftOrRight side;
    public Animator lanimator;
}

public enum LeftOrRight
{
    Left,
    Right
}