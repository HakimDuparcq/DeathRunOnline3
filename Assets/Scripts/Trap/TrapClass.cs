using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TrapType
{
    Particle,
    Fall,
    Stand
}

[System.Serializable]
public class TrapClass
{
    public bool isActivable;
    public GameObject Trigger;
    public TrapType TrapType;
    public GameObject Trap;
    public ParticleSystem particles;
    public Transform[] positions;
    public GameObject[] DeathZone;


    public AudioSource AudioSource;
    [Space(10)]


    public float TimeBeforeSpawn = 1;
    public float TimeToStopAnimation = 2;
    public float TimeParticleVisible = 4;
    public float TimeParticleDisappear = 1;



}
