using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // parent of spawn points
    [SerializeField] private Transform _spawnPositions;

    [Header("UI")]
    // the must be a strict sequence images
    [SerializeField] private List<Image> _healthImages;

    private HelperRandomNumbers _helperRandomIndex;
    private int _index = 0;

    private void OnEnable()
    {
        PlayerHealthController.OnHealthHandler += HealthHandler;
    }

    private void OnDisable()
    {
        PlayerHealthController.OnHealthHandler -= HealthHandler;
    }

    private void Start()
    {
        if(_spawnPositions != null)
            _helperRandomIndex = new HelperRandomNumbers(_spawnPositions.childCount);
    }

    private void HealthHandler()
    {
        if (_healthImages.Count != 0)
        {
            for (int i = _healthImages.Count - 1; i >= 0 ; i--)
            {
                if (_healthImages[i].isActiveAndEnabled)
                {
                    _healthImages[i].enabled = false;

                    return;
                }
            }
        }
    }

    public void SpawnPlayer(GameObject player)
    {
        if (player == null)
            return;

        player.transform.position = _spawnPositions.GetChild(_helperRandomIndex[_index]).position;
        player.transform.rotation = _spawnPositions.GetChild(_helperRandomIndex[_index]).rotation;

        InputManager.Instance.ResetRotation();

        //StartCoroutine(DelayBeforeSpawning());

        _index++;
    }
}
