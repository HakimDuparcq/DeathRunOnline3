using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ValidateName : MonoBehaviour
{
    public Text MyInputfield;
    public Canvas canva;
    public void OnConfirmClicked()
    {
        if (MyInputfield.text!=null && !MyInputfield.text.Contains(" "))
        {
            Destroy(canva);
            //Debug.Log(MyInputfield.text);
            PlayerPrefs.SetString("PlayerName", MyInputfield.text);
            SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
        }
    }
}
