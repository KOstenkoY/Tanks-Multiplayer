using Mirror;
using System;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private string _playerName = null;

    public string PlayerName { get { return _playerName; } set { _playerName = value; } }

    private NetworkManagerLobby _room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (_room != null)
                return _room;

            return _room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        UIController.OnReturnToMainMenu += DestroyPlayer;
    }

    public override void OnStartClient()
    {
        CustomGameManager.Instance.AddPlayer(this);
    }

    public override void OnStopAuthority()
    {
        UIController.OnReturnToMainMenu -= DestroyPlayer;
    }

    private void DestroyPlayer()
    {
        if (isOwned)
        {
            //CustomGameManager.Instance.RemovePlayer(this);

            NetworkManagerLobby.singleton.StopClient();
        }
    }

    public void Win()
    {
        if (isOwned)
            UIController.Instance.WinGame();
    }

    public void Dead()
    {
        if(isOwned)
            UIController.Instance.LoseGame();
    }

    public void DeadSecond()
    {
        if (isOwned)
            // called for player that have second place
            UIController.Instance.LoseGameSecondPlace();
    }

}
