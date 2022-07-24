using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;



public class MainGame : NetworkBehaviour
{
    public static MainGame instance;


    public readonly  SyncList<string> playersNameServeur = new SyncList<string>();
    public readonly  SyncList<string> playersIdServeur = new SyncList<string>();
    public readonly  SyncList< List<int>>playersCharacterServer = new SyncList<List<int>>();
    public readonly  SyncList<bool> playersIsAliveServer = new SyncList<bool>();
    public readonly SyncList<bool> playersRole = new SyncList<bool>();
    public readonly SyncList<int> playersHealth = new SyncList<int>();


    public string LocalPlayerName;
    public string LocalPlayerId;
    public GameObject LocalPlayer;
    public int LocalHealth;

    [SyncVar]
    public int GameState = 0;

    public NetworkManager NetworkManagerr;
    public bool isTrapper;

    



    void Start()
    {
        
        instance = this;
        gameObject.SetActive(true);
        NetworkManagerr = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

    }

    void Update()
    {
        for (int i = 0; i < playersCharacterServer.Count; i++)
        {
            for (int ii = 0; ii < playersCharacterServer[i].Count; ii++)
            {
                //Debug.Log(playersCharacterServer[i][ii]);
            }
            //Debug.Log("_________");
        }
        //Debug.Log("_________");
        
        if (GameObject.Find("Role")!=null)
        {
            isTrapper = GameObject.Find("Role").GetComponent<Toggle>().isOn;   // SPAWN SELON LE ROLE
        }





    }

    public  void RegisterPlayer(string Name, string Id, List<int> playersCharacter, GameObject Player)
    {
        Player.transform.name = Name;
        CmdPlayerJoin(Name, Id, playersCharacter);
    }



    [Command(requiresAuthority = false)]
    public void CmdPlayerJoin(string Name, string Id, List<int> playersCharacter)
    {

        playersNameServeur.Add(Name);
        playersIdServeur.Add(Id);
        playersCharacterServer.Add(playersCharacter);
        playersIsAliveServer.Add(true);
        playersRole.Add(false);
        playersHealth.Add(100);

    }
    
    public void OnLocalPlayerDeconnected()
    {
        Debug.Log("OnLocalPlayerDeconnected");
        CmdOnLocalPlayerDeconnect(LocalPlayerName, LocalPlayerId, PersonaliseCharacter.instance.playersCharacter);
        
    }


    [Command(requiresAuthority = false)]
    public void CmdOnLocalPlayerDeconnect(string LocalPlayerNamee, string LocalPlayerIdd, List<int> playersCharacter)
    {
        playersRole.Remove(playersRole[playersIdServeur.IndexOf(LocalPlayerIdd)]);
        playersIsAliveServer.Remove(playersIsAliveServer[playersIdServeur.IndexOf(LocalPlayerIdd)]);
        playersNameServeur.Remove(LocalPlayerNamee);
        playersIdServeur.Remove(LocalPlayerIdd);
        playersCharacterServer.Remove(playersCharacter);
        playersHealth.Remove(playersHealth[playersIdServeur.IndexOf(LocalPlayerIdd)]);

        Debug.Log("NameServeur " + LocalPlayerNamee);
        Debug.Log("NameServeur " + LocalPlayerIdd);
        RpcOnLocalPlayerDeconnect(LocalPlayerNamee, LocalPlayerIdd);
    }

    [ClientRpc]
    public void RpcOnLocalPlayerDeconnect(string LocalPlayerNameInFonction, string LocalPlayerIdInFonction)
    {
        if (LocalPlayerIdInFonction == LocalPlayerId)
        {
            Debug.Log("DeconnectRpc");
            NetworkManagerr.StopClient();
        }

    }



    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        GameState = 1;
        DoorManager.instance.RpcOnOpenDoor();
        RpcStartGame();

    }



    [ClientRpc]
    public void RpcStartGame()
    {
        LocalPlayer.GetComponent<PlayerSetup>().SetupRole(isTrapper);
        if (isTrapper)
        {
            LocalPlayer.GetComponent<NewPlayerMovement>().speed = 10;
        }
    }






}
