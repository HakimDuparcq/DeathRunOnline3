using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatBehaviour : NetworkBehaviour
{
    public static ChatBehaviour instance;

    public GameObject chatUI = null;
    public TMP_Text chatText = null;
    public TMP_Text chatAllText = null;
    public TMP_InputField inputField = null;


    private static event Action<string, string> OnMessage;


    public int inputState = 1;
    public Button activeInputfield;

    public override void OnStartAuthority()
    {
        instance = this;
        chatUI.SetActive(true);

        OnMessage += HandleNewMessage;

        activeInputfield.onClick.AddListener(() => inputState=0);

    }

    public override void OnStartClient()
    {
        if (!hasAuthority)
        {

            chatUI.SetActive(false);
            this.enabled = false;
        }
    }

    public void Update()
    {
        if (inputState == 0)   // !inputField.isFocused  == n'est pas en ecriture inputfield          
        {

            inputField.enabled = true;
            inputField.ActivateInputField();
        }
        else if (inputState == 1)   // !inputField.isFocused  == n'est pas en ecriture inputfield          
        {
            inputField.enabled = false;
        }
        else if (inputState == 2)   // !inputField.isFocused  == n'est pas en ecriture inputfield          
        {
            inputField.enabled = false;
        }


        

        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(Delay());

        }
    }


    public IEnumerator Delay()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        if (inputField.isFocused && inputField.text == string.Empty)
        {
            inputState = 1;

        }
        else if (inputField.isFocused && inputField.text != string.Empty)
        {
            inputState = 2;
        }
        else
        {
            inputState = 0;
        }


    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!hasAuthority) { return; }

        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string message, string ID)
    {
        string[] Chat = chatAllText.text.Split('\n');
        for (int i = 0; i < Chat.Length; i++)
        {
            Debug.Log(Chat[i]);

        }
        chatAllText.text = "";
        int min;
        if (Chat.Length < 5)
            min = 0;
        else
            min = Chat.Length - 5;
        for (int i = min; i < Chat.Length; i++)
        {
             chatAllText.text += "\n"  + Chat[i] ;
        }

        //chatText.text += message;
        if (ID == MainGame.instance.LocalPlayerId)
        {
            chatAllText.text += "\n" +  "<#80ff80>"  +message  + "</color>"  ;

        }
        else
        {
            chatAllText.text += "\n"  + message;
        }

    }

    [Client]
    public void Send(string message)
    {
        //Debug.Log("Send");
        inputField.text = string.Empty;
        //inputField.enabled = false;

        if (!Input.GetKeyDown(KeyCode.Return)) { return; }

        if (string.IsNullOrWhiteSpace(message)) { return; }



        CmdSendMessage(message, MainGame.instance.LocalPlayerName, MainGame.instance.LocalPlayerId);

        //inputField.OnDeselect(new BaseEventData(EventSystem.current));



    }

    [Command]
    public void CmdSendMessage(string message, string name, string ID)
    {
        //RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");
        RpcHandleMessage($"[{name}]: {message}", ID);
    }

    [ClientRpc]
    public void RpcHandleMessage(string message, string ID)
    {
        OnMessage?.Invoke($"{message}", ID);
    }

    public void Debugg(string DebugMessage)
    {
        Debug.Log(DebugMessage);
    }

}

