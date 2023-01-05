using Mirror;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ChatBehavior : NetworkBehaviour
{
    [SerializeField] private Canvas _chatCanvas = null;
    [SerializeField] private Text _chatText = null;
    [SerializeField] private InputField _inputField = null;

    public static event Action<string> OnMessage;

    public override void OnStartClient()
    {
        OnMessage += HandleNewMessage;
    }

    private void Start()
    {
        _chatCanvas.gameObject.SetActive(false);
    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!isOwned)
            return;

        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string message)
    {
        _chatText.text += message;
    }

    [Client]
    public void SendMessage()
    {
        if (string.IsNullOrWhiteSpace(_inputField.text))
            return;

        CmdSendMessage(_inputField.text);

        _inputField.text = string.Empty;
    }

    [Command(requiresAuthority = false)]
    private void CmdSendMessage(string message)
    {
        // Set Plyaer name from firebase
        RpcHandleMessage($"[{0}]: {message}");
    }

    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
}
