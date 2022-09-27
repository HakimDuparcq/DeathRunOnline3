using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPosition : MonoBehaviour
{
    public static MapPosition instance;

    public Transform SpawnTrapper;
    public Transform SpawnAttacker;
    public Transform SpawnLobby;
    public Transform SpawnEndGameTrapper;
    public Transform SpawnEndGameAttacker;

    public void Awake()
    {
        instance = this;
    }
}
