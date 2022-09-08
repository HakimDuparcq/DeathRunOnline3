using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPersonnalisationView : View
{
    [SerializeField] private Button _backButton;
    public override void Initialize()
    {
        PersonaliseCharacter.instance.PlayerChange(PersonaliseCharacter.instance.PlayerCharacer);
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _backButton.onClick.AddListener(() =>  PersonaliseCharacter.instance.OnSavePersonnalisation());
        _backButton.onClick.AddListener(() => ViewManager.GetView<EscapeMenuView>()._sceneCamera.gameObject.SetActive(false));


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
            PersonaliseCharacter.instance.OnSavePersonnalisation();
            ViewManager.GetView<EscapeMenuView>()._sceneCamera.gameObject.SetActive(false);
        }
    }
}
