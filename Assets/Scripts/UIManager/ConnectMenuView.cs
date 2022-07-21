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

        ClientLocalButton.onClick.AddListener(() => OnClickedLocalHost(isOnServer: false));

        ClientServerButton.onClick.AddListener(() => OnClickedLocalHost(isOnServer: true));

        StopClientButton.onClick.AddListener(() => OnClickedStopClient());

        StopHostButton.onClick.AddListener(() => OnClickedStopHost()); 
    }

    public void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
    }


    public void OnClickedLocalHost(bool isOnServer)
    {
        if (isOnServer)
        {
            NetworkManager.singleton.networkAddress = "13.38.74.252";
        }
        NetworkManager.singleton.StartClient();
        StopClientButton.gameObject.SetActive(true);
        StopHostButton.gameObject.SetActive(false);

        StartCoroutine(TryConnectClient(3 * 10));  //3s

        //ViewManager.Show<LobbyMenuView>();
        //NetworkManager.singleton.networkAddress = "localhost";

        
    }

    public IEnumerator TryConnectClient(float iterationToConnect)
    {
        
        while (MainGame.instance == null)
        {
            iterationToConnect--;
            if (iterationToConnect < 0)
            {
                OnClickedStopClient();
                Debug.Log("Can't Connect Client");
                yield return null;
            }
            

            yield return new WaitForSeconds(0.1f);
            Debug.Log("Wait Client To Connect");
        }
        if (MainGame.instance != null)
        {
            ViewManager.Show<LobbyMenuView>();
        }

    }

   
    public void OnClickedStopClient()
    {
        if (MainGame.instance != null)
        {
            MainGame.instance.OnLocalPlayerDeconnected();
        }
        else
        {
            NetworkManager.singleton.OnStopClient();
        }
        HostClientButton.gameObject.SetActive(true);
        ClientLocalButton.gameObject.SetActive(true);
        ClientServerButton.gameObject.SetActive(true);
        StopClientButton.gameObject.SetActive(false);
        StopHostButton.gameObject.SetActive(false);
        ViewManager.Show<ConnectMenuView>();


    }

    public void OnClickedStopHost()
    {
        //PlayerSetup : OnStopClient() : ViewManager.Show<ConnectMenuView>();
        NetworkManager.singleton.StopHost();
    }


    
}
