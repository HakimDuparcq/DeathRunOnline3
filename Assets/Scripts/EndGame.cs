using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public static EndGame instance;

    public Transform endGamePosAttack;
    public Transform endGamePosTrapper;


    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void TpPlayersToEndGame()
    {
        for (int i = 0; i < Spectator.instance.Players.Count; i++)
        {
            if (MainGame.instance.playersIsAliveServer[i])
            {
                if (Spectator.instance.Players[i].GetComponent<PlayerSetup>().isTrapper)
                {
                    Spectator.instance.Players[i].transform.position = endGamePosTrapper.position;
                }
                else
                {
                    Spectator.instance.Players[i].transform.position = endGamePosAttack.position;
                }
            }
        }
    }


}
