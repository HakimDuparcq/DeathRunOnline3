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
    public Button _startGame;
    public Toggle Role;
    [SerializeField] Camera _sceneCamera;

    public override void Initialize()
    {
        PersonnalisationButton.onClick.AddListener(() => ViewManager.Show<CharacterPersonnalisationView>() );
        PersonnalisationButton.onClick.AddListener(() => _sceneCamera.gameObject.SetActive(true));
        PersonnalisationButton.onClick.AddListener(() => _sceneCamera.depth = 2 );
        PersonnalisationButton.onClick.AddListener(() => PersonaliseCharacter.instance.PlayerChange());    
        
        Settings.onClick.AddListener(() => ViewManager.Show<SettingsMenuView>() );
       
        Quit.onClick.AddListener(() => OnClickQuit());

        _startGame.gameObject.SetActive(true);
        _startGame.onClick.AddListener(() => MainGame.instance.CmdStartGame());
        _startGame.onClick.AddListener(() => ViewManager.Show<NoUIView>());
        _startGame.onClick.AddListener(() => Cursor.lockState = CursorLockMode.Locked);
        _startGame.onClick.AddListener(() => _startGame.gameObject.SetActive(false));
        


    }

    public void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            
            if (MainGame.instance.GameOnServer)
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

    public void OnClickQuit()
    {
        Application.Quit();
    }

}
