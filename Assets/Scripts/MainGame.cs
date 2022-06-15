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

    public string LocalPlayerName;
    public string LocalPlayerId;


    [SyncVar]
    public bool GameOnServer = false;

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

    }
    
    public void OnLocalPlayerDeconnected()
    {
        Debug.Log("OnLocalPlayerDeconnected");
        CmdOnLocalPlayerDeconnect(LocalPlayerName, LocalPlayerId, PersonaliseCharacter.instance.playersCharacter);
        
    }


    [Command(requiresAuthority = false)]
    public void CmdOnLocalPlayerDeconnect(string LocalPlayerNamee, string LocalPlayerIdd, List<int> playersCharacter)
    {
        playersIsAliveServer.Remove(playersIsAliveServer[playersIdServeur.IndexOf(LocalPlayerIdd)]);
        playersNameServeur.Remove(LocalPlayerNamee);
        playersIdServeur.Remove(LocalPlayerIdd);
        playersCharacterServer.Remove(playersCharacter);
        
        Debug.Log("NameServeur " + LocalPlayerNamee);
        Debug.Log("NameServeur " + LocalPlayerIdd);
        RpcOnLocalPlayerDeconnect(LocalPlayerNamee, LocalPlayerIdd);
    }

    [ClientRpc]
    public void RpcOnLocalPlayerDeconnect(string LocalPlayerNameInFonction, string LocalPlayerIdInFonction)
    {
        if (LocalPlayerNameInFonction == LocalPlayerName)
        {
            Debug.Log("DeconnectRpc");
            NetworkManagerr.StopClient();
        }
        
    }

  

    [Command(requiresAuthority = false)]
    public void CmdStartGame()
    {
        GameOnServer = true;
        DoorManager.instance.RpcOnOpenDoor();
    }

    


}
