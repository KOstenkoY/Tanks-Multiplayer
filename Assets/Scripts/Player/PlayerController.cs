using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SyncVar, SerializeField] private float _speed = 2;
    [SyncVar, SerializeField] private int _health = 3;
    private void Start()
    {
        if (isClient && isLocalPlayer)
        {
            InputManager.Instance.SetPlayer(this);
        }
    }
    public void MovePlayer(float movementX, float movementY)
    {
        transform.Translate(new Vector3(movementX * _speed * Time.deltaTime, movementY * _speed * Time.deltaTime));
    }

    [Command]
    public void CmdMovePlayer(float movementX, float movementY)
    {
        transform.Translate(new Vector3(movementX * _speed * Time.deltaTime, movementY * _speed * Time.deltaTime));
    }

    [Command]
    public void CmdTakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(gameObject);

            NetworkServer.UnSpawn(gameObject);
        }
        OnTakeDamage();
    }

    [ClientRpc]
    private void OnTakeDamage()
    {

    }
}
