using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class LobbyNameDisplay : NetworkBehaviour
{
    public static LobbyNameDisplay instance;

    public TextMeshProUGUI[] DisplayNames = new TextMeshProUGUI[3];
    public Toggle[] Roles = new Toggle[3];

    public Button start;
    public Button startFake;

    public Toggle YourRole;
    public Toggle YourRoleFake;

    public void Start()
    {
        instance = this;
        YourRole.onValueChanged.AddListener(delegate { CmdLobbyRole(MainGame.instance.LocalPlayerId, YourRole.isOn); });
        YourRole.onValueChanged.AddListener(delegate { UpdateFakeToggle(); });
        StartCoroutine(SetupRole());
    }
    /*
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer)
        {
            StartCoroutine(SetupRole());
            Debug.Log("initRole");
        }
    }*/

    void Update()
    {
        for (int i = 0; i < MainGame.instance.playersNameServeur.Count; i++)
        {
            DisplayNames[i].text = MainGame.instance.playersNameServeur[i];
        }
        for (int i = MainGame.instance.playersNameServeur.Count; i < DisplayNames.Length; i++)
        {
            DisplayNames[i].text = "Waiting for players..";
        }

        
        if (MainGame.instance.playersIdServeur[0] == MainGame.instance.LocalPlayerId && !MainGame.instance.GameOnServer)
        {
            start.gameObject.SetActive(true);
            startFake.gameObject.SetActive(true);
        }
        else
        {
            start.gameObject.SetActive(false);
            startFake.gameObject.SetActive(false);
        }

        if (MainGame.instance.GameOnServer)
        {
            YourRole.gameObject.SetActive(false);
        }
        else
        {
            YourRole.gameObject.SetActive(true);
        }
    }

    
    public void UpdateFakeToggle()
    {
        YourRoleFake.isOn = YourRole.isOn;
    }
    

    public IEnumerator SetupRole()
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < MainGame.instance.playersRole.Count; i++)
        {
            Roles[i].isOn = MainGame.instance.playersRole[i];
        }
    }



    [Command(requiresAuthority = false)]
    public void CmdLobbyRole(string netId, bool role)
    {
        MainGame.instance.playersRole[MainGame.instance.playersIdServeur.IndexOf(netId)] = role;
        
        RpcLobbyRole( netId,  role);
    }

    [ClientRpc]
    public void RpcLobbyRole(string netId, bool role)
    {
        Roles[MainGame.instance.playersIdServeur.IndexOf(netId)].isOn = role;

    }

    
   




}
