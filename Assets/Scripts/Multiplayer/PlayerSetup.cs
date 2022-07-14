using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;


public class  PlayerSetup : NetworkBehaviour
{
    public string Name;
    public string netId;

    public bool isTrapper;

    public Transform SpawnTrapper;
    public Transform SpawnAttacker;
    public Transform SpawnLobby;
    public Transform SpawnEndGameTrapper;
    public Transform SpawnEndGameAttacker;

    private int spawnTenTime1 = 20;
    private int spawnTenTime2 = 60;

    public Camera CameraPlayer;
    public GameObject CameraScene;

    

    public void Start()
    {

    }

    public void Update()
    {
        if (isLocalPlayer &&  MainGame.instance.GameState == 1 && spawnTenTime1 > 0 )
        {
            if (MainGame.instance.playersRole[MainGame.instance.playersIdServeur.IndexOf(netId)])
            {
                transform.position = SpawnTrapper.position;
                //Debug.Log("TeleportTrapper " + gameObject.name);
            }
            else
            {
                transform.position = SpawnAttacker.position;
                //Debug.Log("TeleportAttack " + gameObject.name);
            }
            spawnTenTime1 -= 1;
        }
       


        if (isLocalPlayer && MainGame.instance.GameState == 2 && spawnTenTime2 > 0)
        {
            if (MainGame.instance.playersRole[MainGame.instance.playersIdServeur.IndexOf(netId)])
            {
                transform.position = SpawnEndGameTrapper.position;
            }
            else
            {
                transform.position = SpawnEndGameAttacker.position;
            }
            spawnTenTime2 -= 1;
        }


    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        MainGame.instance = GameObject.Find("MainGame").GetComponent<MainGame>();

        SpawnTrapper = GameObject.Find("Spawn Trapper").transform;
        SpawnAttacker = GameObject.Find("Spawn Attacker").transform;
        SpawnLobby = GameObject.Find("Spawn Lobby").transform;
        SpawnEndGameTrapper = GameObject.Find("Spawn EndGame Trapper").transform;
        SpawnEndGameAttacker = GameObject.Find("Spawn EndGame Attacker").transform;

        CameraScene = GameObject.Find("CameraScene");
        Spectator.instance.Players.Add(gameObject);
        if (CameraScene!=null)
        {
            CameraScene.SetActive(false);
        }



        if (isLocalPlayer)
        {
            netId = GetComponent<NetworkIdentity>().netId.ToString();
            Name = PlayerPrefs.GetString("PlayerName");
            MainGame.instance.RegisterPlayer(Name, netId, PersonaliseCharacter.instance.playersCharacter,  gameObject);

            MainGame.instance.LocalPlayerName = Name;
            MainGame.instance.LocalPlayerId = netId;
            MainGame.instance.LocalPlayer = gameObject;

            CameraPlayer.GetComponent<Camera>().depth = 1; //Keep camera player
            transform.position = SpawnLobby.position; // Spawn Position

            gameObject.GetComponent<CharacterController>().enabled = true;
            SetupSkin(true);
            //StartCoroutine(LobbyNameDisplay.instance.SetupRole());
            PersonaliseCharacter.instance.OnLoadSelfPersonnalisation();
            PersonaliseCharacter.instance.OnLoadAllPersonalisation();
        }
        else
        {
            gameObject.GetComponent<CharacterController>().enabled = false;
            gameObject.GetComponent<NewPlayerMovement>().enabled = false;
            gameObject.GetComponent<MouseLook>().enabled = false;

            CameraPlayer.gameObject.SetActive(false);
            netId = GetComponent<NetworkIdentity>().netId.ToString();
            
            StartCoroutine(WaitConnexion());
            
            SetupSkin(false);
            
        }

        //StartCoroutine(SetGameCharacters());

        

        


    }





    IEnumerator WaitConnexion()
    {
        yield return new WaitForSeconds(1);
        if (MainGame.instance.playersIdServeur.Contains(GetComponent<NetworkIdentity>().netId.ToString()))
        {
            int indice = MainGame.instance.playersIdServeur.IndexOf(GetComponent<NetworkIdentity>().netId.ToString());
            gameObject.transform.name = MainGame.instance.playersNameServeur[indice];
            Name = MainGame.instance.playersNameServeur[indice];
        }
        else
        {
            StartCoroutine(WaitConnexion());
        }
    }

   


    public override void OnStopClient()
    {
        base.OnStopClient();
        Spectator.instance.Players.Remove(gameObject);
        if (isLocalPlayer)
        {
            StartCoroutine(WaitForSecond(3.0f));
            Debug.Log("STOP");
            MainGame.instance.CmdOnLocalPlayerDeconnect(Name, netId, PersonaliseCharacter.instance.playersCharacter) ;
            CameraScene.SetActive(true);
        }
    }

    public IEnumerator WaitForSecond(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    

    /*
    public void SetGameCharacters()
    {
        //yield return new WaitForSeconds(1);
        int indice = MainGame.instance.playersIdServeur.IndexOf(GetComponent<NetworkIdentity>().netId.ToString());
        //Debug.Log("NetId "+ GetComponent<NetworkIdentity>().netId.ToString() +"     " + "indice " + indice);
        List<int> ConfigCharacter = MainGame.instance.playersCharacterServer[indice];
        for (int i = 0; i < ConfigCharacter.Count; i++)
        {
            //Debug.Log(ConfigCharacter[i]);
        }
        PersonaliseCharacter.instance.PlayerChange(gameObject.transform.GetChild(0).gameObject);
        PersonaliseCharacter.instance.CharacterUpdate(ConfigCharacter);
    }*/


    public void SetupSkin(bool islocalPlayerrr)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(!islocalPlayerrr);
        gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = islocalPlayerrr;
        //Debug.Log(islocalPlayerrr, gameObject);
    }



  



    [Command(requiresAuthority = false)]
    public void CmdSetCharacter(List<int> playersCharacter, string IdPlayer)
    {
        MainGame.instance.playersCharacterServer[MainGame.instance.playersIdServeur.IndexOf(IdPlayer)] = playersCharacter;

        RpcReceiveSetCharacter(IdPlayer);
    }






    [ClientRpc]
    public void RpcReceiveSetCharacter(string IdPlayer)
    {
        if (netId == IdPlayer)
        {
            //Debug.Log(gameObject);
            StartCoroutine( PersonaliseCharacter.instance.CharacterUpdate2(IdPlayer));
        }
    }


    public void SetupRole(bool isTrapper)
    {
        if (isTrapper)  //Desactivate Wall 
        {
            foreach (GameObject Wall in GameObject.FindGameObjectsWithTag("WallRendererOffTrapper"))
            {
                Wall.GetComponent<MeshRenderer>().enabled = false;
            }
            foreach (GameObject Wall in GameObject.FindGameObjectsWithTag("WallColliderOffTrapper"))
            {
                Wall.GetComponent<MeshCollider>().enabled = false;
                Wall.GetComponent<MeshRenderer>().enabled = false;
            }
            
        }


    }




   
}
