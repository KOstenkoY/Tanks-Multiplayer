using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private PlayerController _playerController;

    private bool _buttonPressed;

    private Vector2 movement = new Vector2();

    private void Update()
    {
        if (_buttonPressed)
        {
            _playerController.Move(movement.x, movement.y);
        }
    }
    
    public void SetPlayer(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void OnUpButtonDown()
    {
        _buttonPressed = true;

        movement = new Vector2(0, 1);
    }

    public void OnDownButtonDown()
    {
        _buttonPressed = true;

        movement = new Vector2(0, -1);
    }

    public void OnRightButtonDown()
    {
        _buttonPressed = true;

        movement = new Vector2(1, 0);
    }

    public void OnLeftButtonDown()
    {
        _buttonPressed = true;

        movement = new Vector2(-1, 0);
    }

    public void OnButtonUp()
    {
        _buttonPressed = false;
    }
}
