using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SyncVar, SerializeField] private float _speed = 1000;

    private void Start()
    {
        InputManager.Instance.SetPlayer(this);
    }

    private void Update()
    {
        
    }

    public void Move(float movementX, float movementY)
    {
        transform.Translate(new Vector3(movementX * _speed * Time.deltaTime, movementY * _speed * Time.deltaTime));
    }

}
