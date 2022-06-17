using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class ConnectMenuView : View
{
    public Button HostClientButton;
    public Button ClientLocalButton;
    public Button ClientServerButton;
    public Button StopClientButton;
    public Button StopHostButton;

    public override void Initialize()
    {
        HostClientButton.onClick.AddListener(() => NetworkManager.singleton.StartHost());
        HostClientButton.onClick.AddListener(() => StopClientButton.gameObject.SetActive(false)) ;
        HostClientButton.onClick.AddListener(() => StopHostButton.gameObject.SetActive(true));

        ClientLocalButton.onClick.AddListener(() => NetworkManager.singleton.StartClient());
        ClientLocalButton.onClick.AddListener(() => OnClickedLocalHost());
        HostClientButton.onClick.AddListener(() => StopClientButton.gameObject.SetActive(true));
        HostClientButton.onClick.AddListener(() => StopHostButton.gameObject.SetActive(false));

        ClientServerButton.onClick.AddListener(() => NetworkManager.singleton.StartClient());
        ClientServerButton.onClick.AddListener(() => OnClickedServerHost());
        HostClientButton.onClick.AddListener(() => StopClientButton.gameObject.SetActive(true));
        HostClientButton.onClick.AddListener(() => StopHostButton.gameObject.SetActive(false));

        StopClientButton.onClick.AddListener(() => MainGame.instance.OnLocalPlayerDeconnected());

        StopHostButton.onClick.AddListener(() => NetworkManager.singleton.StopHost());
    }

    public void OnClickedLocalHost()
    {
        NetworkManager.singleton.networkAddress = "localhost";
    }

    public void OnClickedServerHost()
    {
        NetworkManager.singleton.networkAddress = "13.38.74.252";
    }


}
