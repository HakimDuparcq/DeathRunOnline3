using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuView : View 
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _mouseSenSensitivity;
    [SerializeField] private TextMeshProUGUI _mouseSenSensitivityText;

    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _backButton.onClick.AddListener(() => MainGame.instance.LocalPlayer.GetComponent<MouseLook>().mouseSensitivity = 10 + 140*_mouseSenSensitivity.value);

        _mouseSenSensitivity.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        if (PlayerPrefs.HasKey("mouseSensitivity"))
        {
            _mouseSenSensitivity.value = (PlayerPrefs.GetFloat("mouseSensitivity") - 10) /140;
            Debug.Log("SensSlider" + _mouseSenSensitivity.value);
        }
    }

    private void ValueChangeCheck()
    {
        _mouseSenSensitivityText.text =( 10 + 140*_mouseSenSensitivity.value).ToString("F0");
    }
}
