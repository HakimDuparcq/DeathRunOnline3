using Mirror;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatBehaviour : NetworkBehaviour
{
    public static ChatBehaviour instance;


    public GameObject chatUI = null;
    public TMP_Text chatText = null;
    public TMP_Text chatAllText = null;
    public TMP_InputField inputField = null;

    private static event Action<string> OnMessage;

    public override void OnStartAuthority()
    {
        instance = this;
        chatUI.SetActive(true);

        OnMessage += HandleNewMessage;
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
        if (Input.GetKeyDown(KeyCode.Return) && !inputField.isFocused)   // !inputField.isFocused  == n'est pas en ecriture inputfield
        {
            inputField.Select();
        }
    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!hasAuthority) { return; }

        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string message)
    {
        string[] Chat = chatText.text.Split('\n');
        chatText.text = "";
        int min;
        if (Chat.Length < 5)
            min = 0;
        else
            min = Chat.Length-5;
        for (int i = min ; i < Chat.Length; i++)
        {
            chatText.text += "\n" + Chat[i] ;
        }
        
        chatText.text += message;
        chatAllText.text += message;

    }

    [Client]
    public void Send(string message)
    {
        if (!Input.GetKeyDown(KeyCode.Return)) { return; }

        if (string.IsNullOrWhiteSpace(message)) { return; }

        CmdSendMessage(message, MainGame.instance.LocalPlayerName);

        inputField.text = string.Empty;

    }

    [Command]
    public void CmdSendMessage(string message, string name)
    {
        //RpcHandleMessage($"[{connectionToClient.connectionId}]: {message}");
        RpcHandleMessage($"[{name}]: {message}");
    }

    [ClientRpc]
    public void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
}

