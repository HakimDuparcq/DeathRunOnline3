using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;



public class MainGame : NetworkBehaviour
{
    public static MainGame instance;


    public readonly  SyncList<string> playersNameServeur = new SyncList<string>();
    public readonly  SyncList<uint> playersIdServeur = new SyncList<uint>();
    public readonly  SyncList< List<int>>playersCharacterServer = new SyncList<List<int>>();
    public readonly  SyncList<bool> playersIsAliveServer = new SyncList<bool>();
    public readonly SyncList<bool> playersRole = new SyncList<bool>();
    public readonly SyncList<int> playersHealth = new SyncList<int>();


    public string LocalPlayerName;
    public uint LocalPlayerId;
    public GameObject LocalPlayer;
    public int LocalHealth;

    [SyncVar]
    public int GameState = 0;

    public bool isTrapper;

    



    void Start()
    {
        
        instance = this;
        gameObject.SetActive(true);
        Deconnexion.OnExit += OnLocalPlayerDeconnected;
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

    public  void RegisterPlayer(string Name, uint Id, List<int> playersCharacter, GameObject Player)
    {
        Player.transform.name = Name;
        CmdPlayerJoin(Name, Id, playersCharacter);
    }



    [Command(requiresAuthority = false)]
    public void CmdPlayerJoin(string Name, uint Id, List<int> playersCharacter)
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
        CmdOnLocalPlayerDeconnect(LocalPlayerId);
        
    }


    [Command(requiresAuthority = false)]
    public void CmdOnLocalPlayerDeconnect(uint LocalPlayerIdd)
    {
        int indexPlayer = playersIdServeur.IndexOf(LocalPlayerIdd);
        playersRole.RemoveAt(indexPlayer);
        playersIsAliveServer.RemoveAt(indexPlayer);
        playersNameServeur.RemoveAt(indexPlayer);
        playersIdServeur.RemoveAt(indexPlayer);
        playersCharacterServer.RemoveAt(indexPlayer);
        playersHealth.RemoveAt(indexPlayer);

        //Debug.Log("NameServeur " + LocalPlayerNamee);
       // Debug.Log("NameServeur " + LocalPlayerIdd);
        RpcOnLocalPlayerDeconnect(LocalPlayerIdd);
    }

    [ClientRpc]
    public void RpcOnLocalPlayerDeconnect(uint LocalPlayerIdInFonction)
    {
        if (LocalPlayerId ==LocalPlayerIdInFonction)
        {
            NetworkManager.singleton.StopClient();

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
