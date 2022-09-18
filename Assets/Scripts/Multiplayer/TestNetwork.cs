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
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.A))
            {
                CmdTest(gameObject.name, gameObject.GetComponent<NetworkIdentity>(), netId);
            }
        }


    }


    [Command(requiresAuthority = false)]
    public void CmdTest(string fromPlayer, NetworkIdentity NetPlayer, uint NetOk)
    {
        RpcOnTest(fromPlayer, NetPlayer, NetOk);
    }

    [ClientRpc]
    public void RpcOnTest(string fromPlayer, NetworkIdentity NetPlayer, uint NetOk)
    {
        //Debug.Log("From " + fromPlayer + "  to " + gameObject.name);
        Debug.Log(NetPlayer.netId + "    " + NetPlayer.ToString() + "    " + NetOk);

    }

 
 


}