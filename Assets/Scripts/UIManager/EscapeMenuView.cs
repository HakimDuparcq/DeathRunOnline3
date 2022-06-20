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
    [SerializeField] Camera _sceneCamera;

    public override void Initialize()
    {
        PersonnalisationButton.onClick.AddListener(() => ViewManager.Show<CharacterPersonnalisationView>() );
        PersonnalisationButton.onClick.AddListener(() => _sceneCamera.gameObject.SetActive(true));
        PersonnalisationButton.onClick.AddListener(() => _sceneCamera.depth = 2 );
        PersonnalisationButton.onClick.AddListener(() => PersonaliseCharacter.instance.PlayerChange());

        Settings.onClick.AddListener(() => ViewManager.Show<SettingsMenuView>() );

        Quit.onClick.AddListener(() => OnClickQuit());



    }

    public void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            ViewManager.Show<NoUIView>();
        }
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

}
