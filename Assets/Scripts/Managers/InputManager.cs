using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private PlayerController _playerController;
    private PlayerWeapons _playerWeapons;

    private bool _buttonPressed;

    private float _rotationZ;

    private Vector2 _direction = new Vector2();

    private void Start()
    {
        _direction = new Vector2(0, 0);
    }

    private void FixedUpdate()
    {
        if (_buttonPressed && _playerController)
        {
            _playerController.MovePlayer(_direction);
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
        _direction = new Vector2(0, 1);

        _buttonPressed = true;

        if (_rotationZ != 0)
        {
            _rotationZ = 0;

            _playerController.transform.localEulerAngles = new Vector3(0, 0, _rotationZ); ;
        }
    }

    public void OnDownButtonDown()
    {
        _direction = new Vector2(0, -1);

        _buttonPressed = true;

        if (_rotationZ != 180)
        {
            _rotationZ = 180;

            _playerController.transform.localEulerAngles = new Vector3(0, 0, _rotationZ);
        }
    }

    public void OnRightButtonDown()
    {
        _direction = new Vector2(1, 0);

        _buttonPressed = true;

        if (_rotationZ != -90)
        {
            _rotationZ = -90;

            _playerController.transform.localEulerAngles = new Vector3(0, 0, _rotationZ);
        }
    }

    public void OnLeftButtonDown()
    {
        _direction = new Vector2(-1, 0);

        _buttonPressed = true;

        if (_rotationZ != 90)
        {
            _rotationZ = 90;

            _playerController.transform.localEulerAngles = new Vector3(0, 0, _rotationZ);
        }
    }

    public void OnButtonUp()
    {
        _playerController.StopMovePlayer();

        _buttonPressed = false;
    }

    public void OnFireButtonDown()
    {
        _playerWeapons.TryToFire();
    }
}
