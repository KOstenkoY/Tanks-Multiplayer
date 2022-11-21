using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private PlayerController _playerController;
    private PlayerWeapons _playerWeapons;

    private bool _buttonPressed;

    private float _rotationZ;

    private Vector2 movement = new Vector2();

    private void Start()
    {
        // always move straight
        movement = new Vector2(0, 1);
    }

    private void Update()
    {
        if (_buttonPressed && _playerController)
        {
            _playerController.CmdMovePlayer(movement.x, movement.y);

            _playerController.MovePlayer(movement.x, movement.y);
        }
    }
    
    public void SetPlayer(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void SetWeapons(PlayerWeapons playerWeapons)
    {
        _playerWeapons = playerWeapons;
    }

    public void OnUpButtonDown()
    {
        _buttonPressed = true;

        if (_rotationZ != 0)
        {
            _rotationZ = 0;

            _playerController.transform.localEulerAngles = new Vector3(0, 0, _rotationZ); ;
        }
    }

    public void OnDownButtonDown()
    {
        _buttonPressed = true;

        if (_rotationZ != 180)
        {
            _rotationZ = 180;

            _playerController.transform.localEulerAngles = new Vector3(0, 0, _rotationZ);
        }
    }

    public void OnRightButtonDown()
    {
        _buttonPressed = true;

        if (_rotationZ != -90)
        {
            _rotationZ = -90;

            _playerController.transform.localEulerAngles = new Vector3(0, 0, _rotationZ);
        }
    }

    public void OnLeftButtonDown()
    {
        _buttonPressed = true;

        if (_rotationZ != 90)
        {
            _rotationZ = 90;

            _playerController.transform.localEulerAngles = new Vector3(0, 0, _rotationZ);
        }
    }

    public void OnButtonUp()
    {
        _buttonPressed = false;
    }

    public void OnFireButtonDown()
    {
        _playerWeapons.CmdFire();
    }
}
