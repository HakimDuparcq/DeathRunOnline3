using UnityEngine;
using UnityEngine.UI;

namespace Mirror
{
    /// <summary>Shows NetworkManager controls in a GUI at runtime.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkManagerButton")]
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkManagerButton : MonoBehaviour
    {
        NetworkManager manager;

        [Header("NameMenu")]
        public GameObject NameMenu;
        public InputField InputFieldName;
        public Text InputName;
        public GameObject ConfirmNameButton;

        [Header("ConnectMenu")]
        public GameObject HostClientButton;
        public GameObject LocalServerButton;
        public GameObject ClientLocalButton;
        public GameObject ClientServerButton;
        public GameObject StopClientButton;
        public GameObject StopHostButton;
        public GameObject StopServeurButton;

        public Button StartGameButton;
        


        void Awake()
        {
            manager = GetComponent<NetworkManager>();

            NameMenu.SetActive(true);
            InputFieldName.enabled = true;
            ConfirmNameButton.SetActive(true);

            HostClientButton.SetActive(false);
            LocalServerButton.SetActive(false);
            ClientLocalButton.SetActive(false);
            ClientServerButton.SetActive(false);
            StopClientButton.SetActive(false);
            StopHostButton.SetActive(false);
            StopServeurButton.SetActive(false);
            StartGameButton.enabled = true;
    }

        public void Start()
        {
            PreviousName();
        }

        public void Update()
        {
            

            
        }

        public void ActiveButtonMenu()
        {
            HostClientButton.SetActive(true);
            LocalServerButton.SetActive(true);
            ClientLocalButton.SetActive(true);
            ClientServerButton.SetActive(true);

            StopClientButton.SetActive(false);
            StopHostButton.SetActive(false);
            StopServeurButton.SetActive(false);
        }
        


        public void DisableButtonApart(string buttonName)
        {
            HostClientButton.SetActive(false);
            LocalServerButton.SetActive(false);
            ClientLocalButton.SetActive(false);
            ClientServerButton.SetActive(false);

            StopClientButton.SetActive(false);
            StopHostButton.SetActive(false);
            StopServeurButton.SetActive(false);

            if (buttonName == "client")
            {
                StopClientButton.SetActive(true);
            }
            if (buttonName == "host")
            {
                StopHostButton.SetActive(true);
            }
            if (buttonName == "server")
            {
                StopServeurButton.SetActive(true);
            }



        }


        public void OnConfirmClicked()
        {
            NameMenu.SetActive(false);
            ConfirmNameButton.SetActive(false);
            PlayerPrefs.SetString("PlayerName", InputName.text);

            HostClientButton.SetActive(true);
            LocalServerButton.SetActive(true);
            ClientLocalButton.SetActive(true);
            ClientServerButton.SetActive(true);
            

        }

        public void OnClickedLocalHost()
        {
            manager.networkAddress = "localhost";
            /*
            HostClientButton.SetActive(false);
            LocalServerButton.SetActive(false);
            ClientLocalButton.SetActive(false);
            ClientServerButton.SetActive(false);
            */
        }
        public void OnClickedServerHost()
        {
            manager.networkAddress = "13.38.74.252";
            /*
            HostClientButton.SetActive(false);
            LocalServerButton.SetActive(false);
            ClientLocalButton.SetActive(false);
            ClientServerButton.SetActive(false);
            */
        }

        public void PreviousName()
        {
            if (PlayerPrefs.GetString("PlayerName") != null)
            {
                InputFieldName.text = PlayerPrefs.GetString("PlayerName");
                //Debug.Log(PlayerPrefs.GetString("PlayerName"));
            }
        }

        public void Default()
        {
            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            // client ready
            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                //if (GUILayout.Button("Client Ready"))
                // {
                //   NetworkClient.Ready();
                //   if (NetworkClient.localPlayer == null)
                //  {
                //      NetworkClient.AddPlayer();
                //   }
                // }
            }

            StopButtons();
        }
        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    HostClientButton.SetActive(true);
                    //if (GUILayout.Button("Host (Server + Client)"))
                    //{
                    //    manager.StartHost();
                    //}
                }

                ClientServerButton.SetActive(true);
                // Client + IP
                //GUILayout.BeginHorizontal();
                //if (GUILayout.Button("Client"))
                //{
                //manager.StartClient();
                //}

                // This updates networkAddress every frame from the TextField
                //manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                //manager.networkAddress = "35.180.12.188";
                //GUILayout.EndHorizontal();







                // Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    //GUILayout.Box("(  WebGL cannot be server  )");
                    Debug.LogWarning("(  WebGL cannot be server  )");
                }
                else
                {
                    //if (GUILayout.Button("Server Only")) manager.StartServer();
                }
            }
            else
            {
                // Connecting
                //GUILayout.Label($"Connecting to {manager.networkAddress}..");
                Debug.LogWarning("(Connecting to {manager.networkAddress}..");
                StopClientButton.SetActive(true);

                //if (GUILayout.Button("Cancel Connection Attempt"))
                //{
                //   manager.StopClient();
                //}
            }
        } //n'est pas executé

        void StatusLabels()//n'est pas executé
        {
            // host mode
            // display separately because this always confused people:
            //   Server: ...
            //   Client: ...
            if (NetworkServer.active && NetworkClient.active)
            {
                //GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");
                Debug.LogWarning("<b>Host</b>: running via {Transport.activeTransport}");
            }
            // server only
            else if (NetworkServer.active)
            {
                Debug.LogWarning("<b>Server</b>: running via {Transport.activeTransport}");
                //GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
            }
            // client only
            else if (NetworkClient.isConnected)
            {
                Debug.LogWarning("<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
                //GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
            }
        }
        void StopButtons()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                StopHostButton.SetActive(true);
                //if (GUILayout.Button("Stop Host"))
                // {
                //    manager.StopHost();
                //}
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                StopClientButton.SetActive(true);
                //if (GUILayout.Button("Stop Client"))
                // {
                //  manager.StopClient();
                //}
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                StopServeurButton.SetActive(true);
                // if (GUILayout.Button("Stop Server"))
                // {
                //     manager.StopServer();
                // }
            }
        }

        public void PressEnterName()
        {
            if (!string.IsNullOrWhiteSpace(InputFieldName.text) && Input.GetKeyDown(KeyCode.Return))
            {
                OnConfirmClicked();
            }
            
        }


    }
}
