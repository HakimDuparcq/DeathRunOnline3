using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadPreviousName : MonoBehaviour
{

    public InputField InputName;

    void Start()
    {
        InputName = InputName.GetComponent<InputField>();
        PreviousName();
    }

    void Update()
    {
        
    }

    public void PreviousName()
    {
        if (PlayerPrefs.GetString("PlayerName") != null)
        {
            InputName.text = PlayerPrefs.GetString("PlayerName");
            //Debug.Log("ok");
        }
        //Debug.Log(PlayerPrefs.GetString("PlayerName"));
    }

}
