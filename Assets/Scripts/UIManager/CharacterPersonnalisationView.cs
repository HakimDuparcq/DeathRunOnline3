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
}
