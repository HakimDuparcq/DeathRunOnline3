using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPersonnalisationView : View
{
    [SerializeField] private Button _backButton;
    [SerializeField] Camera _sceneCamera;
    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _backButton.onClick.AddListener(() =>  PersonaliseCharacter.instance.OnSavePersonnalisation());
        _backButton.onClick.AddListener(() => _sceneCamera.gameObject.SetActive(false));
        _backButton.onClick.AddListener(() =>  MainGame.instance.LocalPlayer.GetComponent<PlayerSetup>().SetGameCharacters()) ;

    }
}
