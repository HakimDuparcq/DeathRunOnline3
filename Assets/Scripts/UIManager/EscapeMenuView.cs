using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuView : View
{
    public Button StopHost;   //Define in ConnectMenuView
    public Button StopClient;  //Define in ConnectMenuView
    public Button PersonnalisationButton;
    public Button Settings;
    public Button Quit;
    public GameObject Quitting;
    public Button _startGame;
    public Button BackButton;
    
    public Toggle Role;

    public bool startOnDebugMode;
    public GameObject DebugModeUI;
    public Toggle DebugMode;
    public Camera _sceneCamera;


    public override void Initialize()
    {
        PersonnalisationButton.onClick.AddListener(() => ViewManager.Show<CharacterPersonnalisationView>() );
        PersonnalisationButton.onClick.AddListener(() => _sceneCamera.gameObject.SetActive(true));
        PersonnalisationButton.onClick.AddListener(() => _sceneCamera.depth = 2 );
        PersonnalisationButton.onClick.AddListener(() => PersonaliseCharacter.instance.PlayerChange());    
        
        Settings.onClick.AddListener(() => ViewManager.Show<SettingsMenuView>() );

        Quitting.SetActive(false);
        Quit.onClick.AddListener( () => OnClickQuit() );

        _startGame.gameObject.SetActive(true);
        _startGame.onClick.AddListener(() => MainGame.instance.CmdStartGame());
        _startGame.onClick.AddListener(() => ViewManager.Show<NoUIView>());
        _startGame.onClick.AddListener(() => Cursor.lockState = CursorLockMode.Locked);
        _startGame.onClick.AddListener(() => _startGame.gameObject.SetActive(false));

        BackButton.onClick.AddListener(() => Cursor.visible = false);
        BackButton.onClick.AddListener(() => Cursor.lockState = CursorLockMode.Locked);
        BackButton.onClick.AddListener(() => ViewManager.ShowLast());

        
        DebugMode.onValueChanged.AddListener( delegate { OnValueDebugModeChange(); });
        DebugModeUI.gameObject.SetActive(startOnDebugMode);
        DebugMode.isOn = startOnDebugMode;
    }

    public void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            
            if (MainGame.instance.GameState == 1 || MainGame.instance.GameState == 2)
            {
                ViewManager.Show<NoUIView>();
                //Debug.Log("escapeToNoUI");
            }
            else
            {
                ViewManager.Show<LobbyMenuView>();
                //Debug.Log("escapeToLobby");

            }
        }
    }


    public void OnValueDebugModeChange()
    {
        if (DebugMode.isOn)
        {
            DebugModeUI.SetActive(true);
        }
        else
        {
            DebugModeUI.SetActive(false);
        }
    }



    public void OnClickQuit()
    {
        Quitting.SetActive(true);
        Application.Quit();
    }

    

}
