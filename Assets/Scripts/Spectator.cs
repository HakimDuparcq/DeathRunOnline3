using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Cinemachine;

public class Spectator : NetworkBehaviour
{
    public static Spectator instance;

    public List<GameObject> Players = new List<GameObject>();

    public Button LeftArrowSpec;
    public Button RightArrowSpec;

    public int WatchPlayerNumber;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    /*public void CameraSpectator()
    {
        foreach (GameObject Player in Players)
        {
            Player.transform.GetChild(2).gameObject.SetActive(true);// Active Spectator Cam
            Player.transform.GetChild(2).GetChild(0).gameObject.SetActive(true); 

            Player.transform.GetChild(2).GetChild(0).gameObject.SetActive(false); // Disable Spectator Cam
            Player.transform.GetChild(2).gameObject.transform.position = new Vector3(0, 0.73f, 0);
        }
    }*/

    public void OnClickNextSpectator(int addNumber)
    {
        bool continu = true;
        for (int i = WatchPlayerNumber; i < Players.Count+ WatchPlayerNumber && continu; i++)
        {
            if (MainGame.instance.playersIsAliveServer[i%(Players.Count )])
            {
                Debug.Log("playerNumber " + i % (Players.Count));
                Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineFreeLook>().Follow
                                                                                  = Players[i % (Players.Count)].transform;
                Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineFreeLook>().LookAt
                                                                                          = Players[i % (Players.Count)].transform;
                //WatchPlayerNumber = i % (Players.Count);
                continu = false;
            }
        }

        WatchPlayerNumber += addNumber;
        

    }

    public void ActiveSpectatorMode()
    {
        CmdActiveDisableSpectatorMode(MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId) , false); // Death syncvar 

        Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.SetActive(true); //Active Cinemachine

        bool continu = true;
        for (int i = 0; i < Players.Count && continu; i++)
        {
            if (MainGame.instance.playersIsAliveServer[i])
            {
                Debug.Log("playerNumber " + i);
                Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineFreeLook>().Follow
                                                                                  = Players[i].transform;
                Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineFreeLook>().LookAt
                                                                                          = Players[i].transform;
                continu = false;
            }
        }
    }



    public void DisableSpectatorMode()
    {
        CmdActiveDisableSpectatorMode(MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)  ,  true); // Death syncvar 
        Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.SetActive(false); //Disable Cinemachine
        Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).gameObject.transform.localPosition = new Vector3(0, 0.73f, 0);
        Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }



    [Command(requiresAuthority = false)]
    public void CmdActiveDisableSpectatorMode(int numberPlayer, bool boolean)
    {
        MainGame.instance.playersIsAliveServer[numberPlayer] = boolean;
    }






}
