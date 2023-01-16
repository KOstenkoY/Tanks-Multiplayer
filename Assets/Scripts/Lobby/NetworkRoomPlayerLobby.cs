using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    private ColorHandler _playerColorHandler = null;

    [Header("UI")]
    [SerializeField] private GameObject _lobbyUI = null;
    [SerializeField] private Text[] _playerNameTexts = new Text[5];
    [SerializeField] private Text[] _playerReadyTexts = new Text[5];
    [SerializeField] private Image[] _playerUniqueColorsImage = new Image[5];
    [SerializeField] private Toggle[] _uniqueColors = new Toggle[6];
    [SerializeField] private Button _startGameButton = null;

    private SyncDictionary<int, Color> _availableColors = new SyncDictionary<int, Color>();

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string displayName = "Loading...";

    [SyncVar(hook = nameof(HandlePlayerColorStatusChanged))]
    public Color playerUniqueColor = Color.white;

    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool isReady = false;

    [SyncVar(hook = nameof(HandleColorChanged))]
    private int _colorId = -1;
    public int ColorId => _colorId;

    [SyncVar(hook = nameof(HandleOldColorChanged))]
    private int _oldColorId = -1;
    public int OldColorId => _oldColorId;

    private bool _isLeader;

    public bool IsLeader
    {
        set
        {
            _isLeader = value;

            _startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby _room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (_room != null)
            {
                return _room;
            }

            return _room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.displayName);

        CmdSetAvailableColors();

        CmdSetColorHandler();

        CmdSetDisplayColor();

        _lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        UpdateDisplay();
    }

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
    public void HandlePlayerColorStatusChanged(Color oldValue, Color newValue) => UpdateDisplay();
    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleColorChanged(int oldValue, int newValue) => UpdateDisplay();
    public void HandleOldColorChanged(int oldValue, int newValue) => UpdateDisplay();

    public void UpdateDisplay()
    {
        if (!isOwned)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.isOwned)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < _playerNameTexts.Length; i++)
        {
            _playerNameTexts[i].text = "Waiting For Player...";
            _playerReadyTexts[i].text = string.Empty;
            _playerUniqueColorsImage[i].color = Color.white;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            _playerNameTexts[i].text = Room.RoomPlayers[i].displayName;
            _playerReadyTexts[i].text = Room.RoomPlayers[i].isReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
            _playerUniqueColorsImage[i].color = Room.RoomPlayers[i].playerUniqueColor;

            if (Room.RoomPlayers[i].OldColorId >= 0 && Room.RoomPlayers[i].OldColorId < _availableColors.Count)
            {
                _uniqueColors[Room.RoomPlayers[i].OldColorId].isOn = false;
                _uniqueColors[Room.RoomPlayers[i].ColorId].isOn = true;
            }
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!_isLeader)
            return;

        _startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        this.displayName = displayName;
    }

    [Command]
    private void CmdSetDisplayColor()
    {
        _colorId = SetDisplayColor();

        _oldColorId = _colorId;

        playerUniqueColor = _availableColors[_colorId];
    }
    
    private int SetDisplayColor()
    {
        if (_playerColorHandler != null)
        {
            return _playerColorHandler.GetFreePlayerColorUI();
        }
        else
        {
            throw new Exception("The event OnGetFreeColor equals null!");
        }
    }

    [Command]
    public void CmdReadyUp()
    {
        isReady = !isReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient)
            return;

        Room.StartGame();
    }

    // must be equals lobby available colors
    [Command]
    private void CmdSetAvailableColors()
    {
        _availableColors.Add(0, new Color(0.7924528f, 0.6588222f, 0.04037017f, 1));
        _availableColors.Add(1, new Color(0.7735849f, 0, 0, 1));
        _availableColors.Add(2, new Color(0.2598967f, 0.2718508f, 0.8773585f, 1));
        _availableColors.Add(3, new Color(0.04404486f, 0.5283019f, 0.04186539f, 1));
        _availableColors.Add(4, new Color(0.8113208f, 0.3015664f, 0.5777475f, 1));
        _availableColors.Add(5, new Color(0.5188679f, 0.5061409f, 0.5129939f, 1));
    }

    [Command]
    private void CmdSetColorHandler()
    {
        if (_playerColorHandler == null)
        {
            _playerColorHandler = Room.SetColorHandler();

            if (_playerColorHandler.Length == 0)
            {
                _playerColorHandler.InitializeColorHandler();
            }
        }
    }

    [Command]
    public void CmdChangeColor(int number)
    {
        if (!isReady)
        {
            if (number != _colorId)
            {
                if (_playerColorHandler.CheckFreePlayerColorUI(_colorId, number))
                {
                    _oldColorId = _colorId;

                    _colorId = number;

                    playerUniqueColor = _availableColors[number];
                }
                else
                {
                    _uniqueColors[number].isOn = true;
                }
            }
            else
            {
                _uniqueColors[number].isOn = true;
            }
        }
    }
}
