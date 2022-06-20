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

    private bool spawnOneTime = true;
    public Camera CameraPlayer;
    public GameObject CameraScene;

    

    public void Start()
    {

    }

    public void Update()
    {
        if (MainGame.instance.GameOnServer && spawnOneTime)
        {
            if (MainGame.instance.isTrapper)
            {
                transform.position = SpawnTrapper.position;
            }
            else
            {
                transform.position = SpawnAttacker.position;
            }
            spawnOneTime = false;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        MainGame.instance = GameObject.Find("MainGame").GetComponent<MainGame>();

        SpawnTrapper = GameObject.Find("Spawn Trapper").transform;
        SpawnAttacker = GameObject.Find("Spawn Attacker").transform;
        SpawnLobby = GameObject.Find("Spawn Lobby").transform;
        if (GameObject.Find("Role")!=null)
        {
            isTrapper = GameObject.Find("Role").GetComponent<Toggle>().isOn;
        }
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

        DesactivateWallRenderer();

        ActiveStartButtonForHost();


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

    public void ActiveStartButtonForHost()
    {
        if (isLocalPlayer && MainGame.instance.playersNameServeur[0]== Name)
        {
            Button StartButton = GameObject.Find("StartGame").GetComponent<Button>();
            StartButton.transform.position = new Vector3(StartButton.transform.position.x - 500, StartButton.transform.position.y, StartButton.transform.position.z);
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

    public void DesactivateWallRenderer()
    {
        if (isTrapper)
        {
            //Debug.Log("trapperhaha");
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
    }


    public void SetupSkin(bool islocalPlayerrr)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(!islocalPlayerrr);
        gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = islocalPlayerrr;
        //Debug.Log(islocalPlayerrr, gameObject);
    }







    public void OnDisable()
    {
        if (!isLocalPlayer)
        {
            Debug.Log("Disable" + Name + netId);
            MainGame.instance.CmdOnLocalPlayerDeconnect(Name, netId, PersonaliseCharacter.instance.playersCharacter);
            Debug.Log("DisableFinish");

        }

    }
}
