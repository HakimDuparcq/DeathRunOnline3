using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EndGame : NetworkBehaviour
{
    public static EndGame instance;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (MainGame.instance.GameState != 0 )
        {
            if (isTeamDead(true))
            {
                CmdGameEnd(false);
            }
            if (isTeamDead(false))
            {
                CmdGameEnd(true);
            }
        }
        
    }

    public bool isTeamDead(bool trapperTeam)
    {
        for (int i = 0; i < MainGame.instance.playersIdServeur.Count; i++)
        {
            if (MainGame.instance.playersIsAliveServer[i] == true && MainGame.instance.playersRole[i] == trapperTeam)
            {
                return false;
            }
        }
        return true;
    }

    [Command(requiresAuthority =false)]
    public void CmdGameEnd(bool isTrapperWinner)
    {
        MainGame.instance.GameState = 0;

        for (int i = 0; i < MainGame.instance.playersIdServeur.Count; i++)
        {
            MainGame.instance.playersHealth[i] = 100;
            MainGame.instance.playersIsAliveServer[i] = true;
            Debug.Log("relive"+i);
        }

        

        RpcGameEnd();
        Debug.Log(MainGame.instance.playersIsAliveServer[0] +" "+ MainGame.instance.playersIsAliveServer[1]);
    }


    [ClientRpc]
    public void RpcGameEnd()
    {

        
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log(MainGame.instance.playersIdServeur.Count);
        MainGame.instance.LocalPlayer.GetComponent<PlayerReferences>().PlayerCamera.GetComponent<Camera>().fieldOfView = 91.7f;

        StartCoroutine(TpPlayers());

        for (int i = 0; i < Spectator.instance.Players.Count; i++)
        {
            Spectator.instance.Players[i].GetComponent<NewPlayerMovement>().animator.SetBool("death", false);
        }

        SetupTrap.instance.ResetTraps();        // Traps

        ViewManager.Show<LobbyMenuView>();     // View 

        Spectator.instance.DisableSpectatorMode();   //Camera

        MainGame.instance.LocalPlayer.GetComponent<NewPlayerMovement>().canMove = true;  //Movement
        MainGame.instance.LocalPlayer.GetComponent<MouseLook>().canRotate = true;

        for (int i = 0; i < Spectator.instance.Players.Count; i++)
        {
            Spectator.instance.Players[i].GetComponent<CharacterController>().enabled = false;
            //Spectator.instance.Players[i].GetComponent<PlayerReferences>().Character.SetActive(true);
            //Spectator.instance.Players[i].GetComponent<PlayerReferences>().Character.transform.position = new Vector3(0, 0, 0);

            Spectator.instance.Players[i].GetComponent<PlayerReferences>().Capsule.SetActive(true);
            Spectator.instance.Players[i].GetComponent<PlayerReferences>().Capsule.GetComponent<CapsuleCollider>().enabled = true;
            Spectator.instance.Players[i].GetComponent<PlayerReferences>().Capsule.GetComponent<MeshRenderer>().enabled = false;
        }

        MainGame.instance.LocalPlayer.GetComponent<CharacterController>().enabled = true;
        //MainGame.instance.LocalPlayer.GetComponent<PlayerReferences>().Character.SetActive(false);
        //MainGame.instance.LocalPlayer.GetComponent<PlayerReferences>().Character.transform.position = new Vector3(0, -0.65f, -0.5f);
        MainGame.instance.LocalPlayer.GetComponent<PlayerReferences>().Capsule.GetComponent<MeshRenderer>().enabled = true;

        MainGame.instance.LocalPlayer.GetComponent<NewPlayerMovement>().speed = 8;

        DoorManager.instance.CloseDoorNumber(1);
    }

    public IEnumerator TpPlayers()
    {
        float tempo = 0.5f;
        while (tempo>0)
        {
            tempo -= Time.deltaTime;
            for (int i = 0; i < Spectator.instance.Players.Count; i++)     // TP Lobby
            {
                Spectator.instance.Players[i].transform.position = Spectator.instance.Players[i].GetComponent<PlayerSetup>().SpawnLobby.position;
                Debug.Log("TP");
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    




}
