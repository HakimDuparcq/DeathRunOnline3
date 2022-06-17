using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameMenuView : View
{
    [SerializeField] private Button _confirmButton;
    [SerializeField] private TMP_InputField InputFieldName;
    public override void Initialize()
    {
        _confirmButton.onClick.AddListener(() => OnClickConfirm());
        InputFieldName.onEndEdit.AddListener(delegate { OnEnterConfirm(); });
        PreviousName();
    }



    public void OnClickConfirm()
    {
        if (!string.IsNullOrWhiteSpace(InputFieldName.text))
        {
            ViewManager.Show<ConnectMenuView>();
            PlayerPrefs.SetString("PlayerName", InputFieldName.text);
        }
    }

    public void OnEnterConfirm()
    {
        if (!string.IsNullOrWhiteSpace(InputFieldName.text) && Input.GetKeyDown(KeyCode.Return))
        {
            ViewManager.Show<ConnectMenuView>();
            PlayerPrefs.SetString("PlayerName", InputFieldName.text);
        }
    }
    public void PreviousName()
    {
        if (PlayerPrefs.GetString("PlayerName") != null)
        {
            InputFieldName.text = PlayerPrefs.GetString("PlayerName");
        }
    }

}
