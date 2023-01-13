using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string _playerName = null;

    public string PlayerName { get { return _playerName; } set { _playerName = value; } }
    
    private void Start()
    {
        ChatBehavior.OnGetPlayerName += GetPlayerName;
    }

    private void OnDisable()
    {
        ChatBehavior.OnGetPlayerName -= GetPlayerName;
    }

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

    private string GetPlayerName()
    {
        return _playerName;
    }
}
