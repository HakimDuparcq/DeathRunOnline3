using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class LobbyNameDisplay : NetworkBehaviour
{
    public static LobbyNameDisplay instance;

    public TextMeshProUGUI[] DisplayNames;
    public Toggle[] Roles ;

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

#if !UNITY_SERVER
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

        
        if (MainGame.instance.playersIdServeur.Count>0 &&  MainGame.instance.playersIdServeur[0] == MainGame.instance.LocalPlayerId && MainGame.instance.GameState ==0)
        {
            start.gameObject.SetActive(true);
            startFake.gameObject.SetActive(true);
        }
        else
        {
            start.gameObject.SetActive(false);
            startFake.gameObject.SetActive(false);
        }

        if (MainGame.instance.GameState == 1 || MainGame.instance.GameState == 2 )
        {
            YourRole.gameObject.SetActive(false);
        }
        else
        {
            YourRole.gameObject.SetActive(true);
        }
    }
#endif

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
    public void CmdLobbyRole(uint netId, bool role)
    {
        MainGame.instance.playersRole[MainGame.instance.playersIdServeur.IndexOf(netId)] = role;
        
        RpcLobbyRole( netId,  role);
    }

    [ClientRpc]
    public void RpcLobbyRole(uint netId, bool role)
    {
        Debug.Log("client Number " + MainGame.instance.playersIdServeur.IndexOf(netId));
        if (MainGame.instance.playersIdServeur.Contains(netId))
        {
            Debug.Log("client  " + netId + " in playersIdServeur"  );
        }
        else
        {
            Debug.Log("client  " + netId + " NOT NOT in playersIdServeur");
        }
        Roles[MainGame.instance.playersIdServeur.IndexOf(netId)].isOn = role;

    }

    
   




}
