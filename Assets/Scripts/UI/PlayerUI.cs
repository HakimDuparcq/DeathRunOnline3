using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [Header("Help")]
    public GameObject helpUI;
    public TextMeshProUGUI helpUIText;
    public bool isDisplayHelp;
    public string texthelp;

  
    void Start()
    {
        helpUI.SetActive(false);


    }

    void Update()
    {
        if (isDisplayHelp)
        {
            helpUI.SetActive(true);
            helpUIText.SetText(texthelp);
        }
        else
        {
            helpUI.SetActive(false);
        }


    }






}
