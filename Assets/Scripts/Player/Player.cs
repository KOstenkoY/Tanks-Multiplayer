using System;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    private string _playerName = null;

    private int _serverId;

    public string PlayerName { get { return _playerName; } set { _playerName = value; } }
    public int ServerId { get { return _serverId; } set { _serverId = value; } }

    public void OnNameChanged(string oldName, string newName)
    {
        if(newName != null)
        {
            _playerName = newName;
        }
        else
        {
            throw new ArgumentNullException();
        }
    }

    [Command]
    public void CmdSetupPlayer(string name)
    {
        _playerName = name;
    }
}
