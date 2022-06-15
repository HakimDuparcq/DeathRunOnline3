using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum FallType
{
    None,
    Down,
    Up
}

[System.Serializable]
public class TrapClass
{
    public bool isActivable;
    public GameObject Trigger;
    public FallType FallType;
    public GameObject Trap;
    public ParticleSystem particles;
    public Transform[] positions;
    public GameObject[] DeathZone;

    public float TimeBeforeSpawn = 1;
    public float TimeToStopAnimation = 2;
    public float TimeParticleVisible = 4;
    public float TimeParticleDisappear = 1;



}
