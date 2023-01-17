using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController>
{
    private Player _player;

    [Header("UI")]
    [SerializeField] private GameObject _gameplayMenu = null;
    [SerializeField] private GameObject _deathMenu = null;

    public Player Player => _player;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void LoseGame()
    {
        _gameplayMenu.SetActive(false);
        _deathMenu.SetActive(true);
    }
}
