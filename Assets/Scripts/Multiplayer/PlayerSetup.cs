using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;


public class  PlayerSetup : NetworkBehaviour
{
    public string Name;

    public bool isTrapper;

    

    
    private bool spawnGS1 = true;
    private bool spawnGS2 = true;
    
    public Camera CameraPlayer;
    public Vector3 posLocalPlayer;
    public Vector3 posOtherPlayers;


    public void Start()
    {

    }

    public void Update()
    {

    }


    public void TPlayerGameStateChange()
    {
        /*
        if (isLocalPlayer && MainGame.instance.GameState == 1 && spawnGS1 == true)
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
            spawnGS1 = false;
            Debug.Log("TP Local Player GameState 1");
        }
        if (isLocalPlayer && MainGame.instance.GameState == 2 && spawnGS2 == true)
        {
            if (MainGame.instance.playersRole[MainGame.instance.playersIdServeur.IndexOf(netId)])
            {
                transform.position = SpawnEndGameTrapper.position;
            }
            else
            {
                transform.position = SpawnEndGameAttacker.position;
            }
            spawnGS2 = false;
            Debug.Log("TP Local Player GameState 2");
        }
        if (MainGame.instance.GameState == 0)
        {
            spawnGS1 = true;
            spawnGS2 = true;
        }*/




    }

    public void TPlayerGameStateChange(int state)
    {
        EndGame.instance.TpPlayerMirror(1);
        /*
        if (state == 1) 
        {
            EndGame.instance.TpPlayerMirror();
            //MainGame.instance.LocalPlayer.GetComponent<CharacterController>().enabled = false;
            //MainGame.instance.LocalPlayer.GetComponent<NetworkTransform>().CmdTeleport(new Vector3(-0.39f, 9.98f, -10.23f), Quaternion.identity);
        }
        else if (state == 2)
        {
            EndGame.instance.TpPlayerMirror();
            //MainGame.instance.LocalPlayer.GetComponent<CharacterController>().enabled = false;
            //MainGame.instance.LocalPlayer.GetComponent<NetworkTransform>().CmdTeleport(new Vector3(-0.39f, 9.98f, -10.23f), Quaternion.identity);
        }
        */
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        //MainGame.instance = GameObject.Find("MainGame").GetComponent<MainGame>();

        

        
        Spectator.instance.Players.Add(gameObject);

        
        ViewManager.GetView<EscapeMenuView>()._sceneCamera.gameObject.SetActive(false);





        if (isLocalPlayer)
        {
            Name = PlayerPrefs.GetString("PlayerName");
            MainGame.instance.RegisterPlayer(Name, netId, PersonaliseCharacter.instance.playersCharacter,  gameObject);

            MainGame.instance.LocalPlayerName = Name;
            MainGame.instance.LocalPlayerId = netId;
            MainGame.instance.LocalPlayer = gameObject;

            CameraPlayer.GetComponent<Camera>().depth = 1; //Keep camera player
            //transform.position = SpawnLobby.position; // Spawn Position

            gameObject.GetComponent<CharacterController>().enabled = true;
            SetupSkin(true);
            
            PersonaliseCharacter.instance.OnLoadSelfPersonnalisation();
            PersonaliseCharacter.instance.OnLoadAllPersonalisation();
            gameObject.layer = 7;
            gameObject.transform.GetChild(1).gameObject.layer = 7;
        }
        else
        {
            gameObject.GetComponent<CharacterController>().enabled = false;
            //gameObject.GetComponent<NewPlayerMovement>().enabled = false;
            gameObject.GetComponent<MouseLook>().enabled = false;

            CameraPlayer.gameObject.SetActive(false);
            
            StartCoroutine(WaitConnexion());
            
            SetupSkin(false);

            gameObject.layer = 8;
            gameObject.transform.GetChild(1).gameObject.layer = 8;


        }

        //StartCoroutine(SetGameCharacters());

    }





    IEnumerator WaitConnexion()
    {
        yield return new WaitForSeconds(1);
        if (MainGame.instance.playersIdServeur.Contains(netId))
        {
            int indice = MainGame.instance.playersIdServeur.IndexOf(netId);
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
            ViewManager.Show<ConnectMenuView>();


            ViewManager.GetView<EscapeMenuView>()._sceneCamera.gameObject.SetActive(true);

            
            Debug.Log("STOP");
            MainGame.instance.CmdOnLocalPlayerDeconnect(netId) ;
            
        }
    }



    

    public void SetupSkin(bool islocalPlayerrr)
    {
        //gameObject.transform.GetChild(0).gameObject.SetActive(!islocalPlayerrr);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        if (islocalPlayerrr)
        {
            gameObject.transform.GetChild(0).transform.localPosition = posLocalPlayer; //new Vector3(0, -0.65f, -0.5f);
        }
        else
        {
            gameObject.transform.GetChild(0).transform.localPosition = posOtherPlayers;//new Vector3(0, -0.8168f, 0);
        }
        gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = islocalPlayerrr;
        //Debug.Log(islocalPlayerrr, gameObject);
    }



  



    [Command(requiresAuthority = false)]
    public void CmdSetCharacter(List<int> playersCharacter, uint IdPlayer)
    {
        MainGame.instance.playersCharacterServer[MainGame.instance.playersIdServeur.IndexOf(IdPlayer)] = playersCharacter;

        RpcReceiveSetCharacter(IdPlayer);
    }






    [ClientRpc]
    public void RpcReceiveSetCharacter(uint IdPlayer)
    {
        if (netId == IdPlayer)
        {
            //Debug.Log(gameObject);
            StartCoroutine( PersonaliseCharacter.instance.CharacterUpdateAllPlayer(IdPlayer));
        }
    }


    public void SetupRole(bool isTrapper)
    {
        if (isTrapper)  //Desactivate Wall 
        {
            foreach (GameObject Wall in GameObject.FindGameObjectsWithTag("WallRendererOffTrapper"))
            {
                //Wall.GetComponent<MeshRenderer>().enabled = false;
            }
            foreach (GameObject Wall in GameObject.FindGameObjectsWithTag("WallColliderOffTrapper"))
            {
                Wall.GetComponent<MeshCollider>().enabled = false;
                Wall.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            foreach (GameObject Wall in GameObject.FindGameObjectsWithTag("WallRendererOffTrapper"))
            {
                //Wall.GetComponent<MeshRenderer>().enabled = true;
            }
            foreach (GameObject Wall in GameObject.FindGameObjectsWithTag("WallColliderOffTrapper"))
            {
                Wall.GetComponent<MeshCollider>().enabled = true;
                Wall.GetComponent<MeshRenderer>().enabled = true;
            }
        }


    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        CameraPlayer.gameObject.SetActive(false);

    }


}
