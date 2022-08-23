using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugSystem : MonoBehaviour
{



    public TextMeshProUGUI logMessage;
    public Button Clear;

    void Start()
    {
        Clear.onClick.AddListener(() => OnClear());
    }

    void Update()
    {
        
    }



    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logMessage.text += "<#FFFFFF>" + logString + "<#0036FF>" + stackTrace + "<#06B000>" + type + "</color>" + "\n" + "\n"; 
    }


    public void OnClear()
    {
        logMessage.text = "";
    }


}
