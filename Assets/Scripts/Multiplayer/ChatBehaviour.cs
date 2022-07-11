using Mirror;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ChatBehaviour : NetworkBehaviour
{
    public static ChatBehaviour instance;


    [SerializeField] private GameObject chatUI = null;
    [SerializeField] private TMP_Text chatText = null;
    [SerializeField] private TMP_InputField inputField = null;

    private static event Action<string> OnMessage;

    public override void OnStartAuthority()
    {
        instance = this;
        chatUI.SetActive(true);

        OnMessage += HandleNewMessage;
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

