using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    private Player _player;

    public Player Player => _player;

    public void SetPlayer(Player player)
    {
        _player = player;
    }
}
