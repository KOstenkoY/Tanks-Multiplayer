using Mirror;
using System;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private string _playerName = null;

    public string PlayerName { get { return _playerName; } set { _playerName = value; } }

    private void Start()
    {
        if (isOwned)
            UIController.Instance.SetPlayer(this);

        if (isClient)
            UIController.Instance.SetPlayer(this);
    }
}
