using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// also must be added component Network Rigidbody2D
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour
{
    [SyncVar, SerializeField]
    private float _speedForce = 1.5f;

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
        RpcMovePlayer(direction);
    }

    [ClientRpc]
    private void RpcMovePlayer(Vector2 direction) => _rigidbody.velocity = direction * _speedForce;

    public void StopMovePlayer()
    {
        _rigidbody.velocity = Vector2.zero;

        CmdStopMovePlayer();
    }

    [Command]
    private void CmdStopMovePlayer()
    {
        RpcStopMovePlayer();
    }

    [ClientRpc]
    private void RpcStopMovePlayer() => _rigidbody.velocity = Vector2.zero;
}
