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
        HostClientButton.onClick.AddListener(() => ViewManager.Show<LobbyMenuView>());

        ClientLocalButton.onClick.AddListener(() => NetworkManager.singleton.StartClient());
        ClientLocalButton.onClick.AddListener(() => OnClickedLocalHost());
        ClientLocalButton.onClick.AddListener(() => StopClientButton.gameObject.SetActive(true));
        ClientLocalButton.onClick.AddListener(() => StopHostButton.gameObject.SetActive(false));
        ClientLocalButton.onClick.AddListener(() => ViewManager.Show<LobbyMenuView>());

        ClientServerButton.onClick.AddListener(() => NetworkManager.singleton.StartClient());
        ClientServerButton.onClick.AddListener(() => OnClickedServerHost());
        ClientServerButton.onClick.AddListener(() => StopClientButton.gameObject.SetActive(true));
        ClientServerButton.onClick.AddListener(() => StopHostButton.gameObject.SetActive(false));
        ClientServerButton.onClick.AddListener(() => ViewManager.Show<LobbyMenuView>());

        StopClientButton.onClick.AddListener(() => MainGame.instance.OnLocalPlayerDeconnected());
        StopClientButton.onClick.AddListener(() => ViewManager.Show<ConnectMenuView>());

        StopHostButton.onClick.AddListener(() => NetworkManager.singleton.StopHost());
        StopHostButton.onClick.AddListener(() => ViewManager.Show<ConnectMenuView>());
    }

    public void OnClickedLocalHost()
    {
        //NetworkManager.singleton.networkAddress = "localhost";
    }

    public void OnClickedServerHost()
    {
        NetworkManager.singleton.networkAddress = "13.38.74.252";
    }


}
