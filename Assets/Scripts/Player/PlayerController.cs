using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// also must be added component Network Rigidbody2D
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour
{
    //[SyncVar, SerializeField] 
    public float _speedForce = 2f;
    //[SyncVar, SerializeField] 
    private int _health = 3;

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        if (isClient && isLocalPlayer)
        {
            InputManager.Instance.SetPlayer(this);
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }

    public void MovePlayer(Vector2 direction)
    {
        _rigidbody.velocity = direction * _speedForce;

        CmdMovePlayer(direction);
    }

    [Command]
    private void CmdMovePlayer(Vector2 direction)
    {
        OnMovePlayer(direction);
    }

    [ClientRpc]
    private void OnMovePlayer(Vector2 direction) => _rigidbody.velocity = direction * _speedForce;

    public void StopMovePlayer()
    {
        _rigidbody.velocity = Vector2.zero;

        CmdStopMovePlayer();
    }

    [Command]
    private void CmdStopMovePlayer()
    {
        OnStopMovePlayer();
    }

    [ClientRpc]
    private void OnStopMovePlayer() => _rigidbody.velocity = Vector2.zero;

    public  void TakeDamage(int damage)
    {
        CmdTakeDamage(damage);
    }

    [Command]
    private void CmdTakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(gameObject);

            NetworkServer.UnSpawn(gameObject);
        }
    }
}
