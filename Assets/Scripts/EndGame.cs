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

    [ClientCallback]
    void Update()
    {
        if (MainGame.instance.GameState != 0 )
        {
            if (isTeamDead(trapperTeam :true))
            {
                CmdGameEnd(false);

            }
            if (isTeamDead(trapperTeam : false))
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

        RpcGameEnd(isTrapperWinner);
    }


    [ClientRpc]
    public void RpcGameEnd(bool isTrapperWinner)
    {
        SendWinner(isTrapperWinner);
        TpPlayerMirror(state:2);
        //StartCoroutine(Restart(isTrapperWinner));
    }

    private IEnumerator Restart(bool isTrapperWinner)
    {
        yield return new WaitForSeconds(5f);
        
        MainGame.instance.LocalPlayer.GetComponent<PlayerReferences>().PlayerCamera.GetComponent<Camera>().fieldOfView = 91.7f;

        //StartCoroutine(TpPlayers());
        TpPlayerMirror(10);

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
    /*
    public IEnumerator TpPlayers()
    {
        float tempo = 0.5f;
        while (tempo>0)
        {
            tempo -= Time.deltaTime;
            for (int i = 0; i < Spectator.instance.Players.Count; i++)     // TP Lobby
            {
                Spectator.instance.Players[i].transform.position = Spectator.instance.Players[i].GetComponent<PlayerSetup>().SpawnLobby.position;
            }
            Debug.Log("TP");
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    */

    public void TpPlayerMirror(int state)
    {
        Debug.Log("TP Game State ");
        GameObject LocalPlayer = MainGame.instance.LocalPlayer;
        LocalPlayer.GetComponent<CharacterController>().enabled = false;

        if (state == 1)
        {
            LocalPlayer.GetComponent<NetworkTransform>().CmdTeleport(new Vector3(-0.39f, 9.98f, -10.23f), Quaternion.identity);
        }
        else if (true)
        {

        }
        else if (true)
        {

        }
        
        /*
        for (int i = 0; i < Spectator.instance.Players.Count; i++)
        {
            Spectator.instance.Players[i].GetComponent<NetworkTransform>().CmdTeleport(
                                   Spectator.instance.Players[i].GetComponent<PlayerSetup>().SpawnLobby.position  , 
                                   Quaternion.identity);
        }*/

    }


    
    public void SendWinner(bool isTrapperWinner)
    {
        if (isTrapperWinner)
        {
            //ChatBehaviour.instance.RpcHandleMessage("Trappers Win This Round", 100);
            ChatBehaviour.instance.HandleNewMessage("Trappers Win This Round", 100);
        }
        else
        {
            //ChatBehaviour.instance.RpcHandleMessage("Attackers Win This Round", 100);
            ChatBehaviour.instance.HandleNewMessage("Attackers Win This Round", 100);
        }
    }

}
