using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [Header("UI")]
    [SerializeField] private GameObject _gameplayMenu = null;
    [SerializeField] private GameObject _deathMenu = null;
    // UI for player that have second place
    [SerializeField] private GameObject _deathMenuForSecondPlace = null;
    [SerializeField] private GameObject _winMenu = null;

    [Header("Chat UI")]
    [SerializeField] private Canvas _chatCanvas = null;
    [SerializeField] private Text _chatText = null;
    [SerializeField] private InputField _chatInputField = null;

    public Text ChatText => _chatText;
    public InputField ChatInputField => _chatInputField;

    public static event Action OnReturnToMainMenu;
    public static event Action OnSendMessage;

    private void Start()
    {
        _chatCanvas.gameObject.SetActive(false);
    }

    public void SendMessage()
    {
        OnSendMessage?.Invoke();
    }

    public void WinGame()
    {
        _gameplayMenu.SetActive(false);

        _winMenu.SetActive(true);
    }

    public void LoseGame()
    {
        _gameplayMenu.SetActive(false);
        _deathMenu.SetActive(true);
    }

    public void LoseGameSecondPlace()
    {
        _gameplayMenu.SetActive(false);

        _deathMenuForSecondPlace.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        OnReturnToMainMenu?.Invoke();
    }
}