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

    public override void OnStartAuthority()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        InputManager.Instance.SetPlayer(this);
    }

    public void MovePlayer(Vector2 direction)
    {
        _rigidbody.velocity = direction * _speedForce;

        CmdMovePlayer(direction);
    }

    [Command]
    private void CmdMovePlayer(Vector2 direction)
    {
        _rigidbody.velocity = direction * _speedForce;
    }

    public void StopMovePlayer()
    {
        _rigidbody.velocity = Vector2.zero;
        CmdStopMovePlayer();
    }

    [Command]
    private void CmdStopMovePlayer()
    {
        _rigidbody.velocity = Vector2.zero;
    }
}
