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


    [SerializeField] private Button _fullScreen;
    [SerializeField] private Button _halfScreen;

    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _backButton.onClick.AddListener(() => MainGame.instance.LocalPlayer.GetComponent<MouseLook>().mouseSensitivity = 10 + 140*_mouseSenSensitivity.value);

        _mouseSenSensitivity.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        if (PlayerPrefs.HasKey("mouseSensitivity"))
        {
            _mouseSenSensitivity.value = (PlayerPrefs.GetFloat("mouseSensitivity") - 10) /140;
            //Debug.Log("SensSlider" + _mouseSenSensitivity.value);
        }

        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen, 0);
        _fullScreen.onClick.AddListener(() => Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen, 0));
        _halfScreen.onClick.AddListener(() => Screen.SetResolution(960, 540, FullScreenMode.Windowed, 0));

    }

    private void ValueChangeCheck()
    {
        _mouseSenSensitivityText.text =( 10 + 140*_mouseSenSensitivity.value).ToString("F0");
    }

    public void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            MainGame.instance.LocalPlayer.GetComponent<MouseLook>().mouseSensitivity = 10 + 140 * _mouseSenSensitivity.value;
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
}
