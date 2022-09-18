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

    public TextMeshProUGUI PlayerWatchingName;

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


    

    public void OnClickNextSpectator(int addNumber)
    {
        bool continu = true;
        for (int i = WatchPlayerNumber; i < Players.Count + WatchPlayerNumber && continu; i++)
        {
            if (MainGame.instance.playersIsAliveServer[i%(Players.Count )])
            {
                Debug.Log("playerNumber " + i % (Players.Count));
                Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineFreeLook>().Follow
                                                                                  = Players[i % (Players.Count)].transform;
                Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.GetComponent<CinemachineFreeLook>().LookAt
                                                                                          = Players[i % (Players.Count)].transform;

                PlayerWatchingName.text = Players[i % (Players.Count)].name;

                //WatchPlayerNumber = i % (Players.Count);
                continu = false;
            }
        }

        WatchPlayerNumber += addNumber;
        

    }

    public IEnumerator ActiveSpectatorMode()
    {
        CmdActiveDisableSpectatorMode(MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId) , false); // Death syncvar 
        //MainGame.instance.playersIsAliveServer[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)] = false;

        ViewManager.Show<SpectatorMenuView>();

       

        CmdDisableDeadPlayerCollider(MainGame.instance.LocalPlayerId);


        Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.SetActive(true); //Active Cinemachine
        yield return new WaitForSeconds(2);

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


                PlayerWatchingName.text = Players[i].name;

                continu = false;
            }
        }
    }



    public void DisableSpectatorMode()
    {
        //ViewManager.Show<NoUIView>();

        //CmdActiveDeadPlayerCollider(MainGame.instance.LocalPlayerId);

        Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).GetChild(0).gameObject.SetActive(false); //Disable Cinemachine
        Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).gameObject.transform.localPosition = new Vector3(0, 0.73f, 0);
        Players[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)].transform.GetChild(2).gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }



    [Command(requiresAuthority = false)]
    public void CmdActiveDisableSpectatorMode(int numberPlayer, bool boolean)
    {
        //MainGame.instance.playersIsAliveServer[numberPlayer] = boolean;
    }


    [Command(requiresAuthority = false)]
    public void CmdDisableDeadPlayerCollider(uint ID)
    {
        RpcDisableDeadPlayerCollider(ID);
    }

    [ClientRpc]
    public void RpcDisableDeadPlayerCollider(uint ID)
    {
        Players[MainGame.instance.playersIdServeur.IndexOf(ID)].GetComponent<CharacterController>().enabled = false;
        Players[MainGame.instance.playersIdServeur.IndexOf(ID)].transform.GetChild(1).GetComponent<CapsuleCollider>().enabled = false;
    }

    [Command(requiresAuthority = false)]
    public void CmdActiveDeadPlayerCollider(uint ID)
    {
        RpcActiveDeadPlayerCollider(ID);
    }

    [ClientRpc]
    public void RpcActiveDeadPlayerCollider(uint ID)
    {
        //Players[MainGame.instance.playersIdServeur.IndexOf(ID)].GetComponent<CharacterController>().enabled = true;
        Players[MainGame.instance.playersIdServeur.IndexOf(ID)].transform.GetChild(1).gameObject.SetActive(true);
        Players[MainGame.instance.playersIdServeur.IndexOf(ID)].transform.GetChild(0).gameObject.SetActive(false);
    }

}
