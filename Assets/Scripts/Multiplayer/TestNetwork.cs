using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class TestNetwork : NetworkBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                //CmdTestTp();
                Test();
            }
        }


    }


    [Command(requiresAuthority = false)]
    public void CmdTestTp()
    {
        RpcOnTest();
    }

    [ClientRpc]
    public void RpcOnTest()
    {
        //Debug.Log("From " + fromPlayer + "  to " + gameObject.name);
        MainGame.instance.LocalPlayer.GetComponent<CharacterController>().enabled = false;
        MainGame.instance.LocalPlayer.GetComponent<NetworkTransform>().CmdTeleport(MapPosition.instance.SpawnAttacker.position, Quaternion.identity);
        Debug.Log("TEST");

    }

    public void Test()
    {
        //Debug.Log("From " + fromPlayer + "  to " + gameObject.name);
        MainGame.instance.LocalPlayer.GetComponent<CharacterController>().enabled = false;
        MainGame.instance.LocalPlayer.GetComponent<NetworkTransform>().CmdTeleport(MapPosition.instance.SpawnAttacker.position, Quaternion.identity);

    }



}