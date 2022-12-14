using Mirror;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : NetworkBehaviour
{
    private Rigidbody2D _rigidbody = null;

    [SerializeField] private int _damage = 1;

    [SyncVar, SerializeField] private float _bulletSpeed = 2;

    private float _gravityScale = 0;

    public override void OnStartAuthority()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.gravityScale = _gravityScale;
    }

    //private void FixedUpdate()
    //{
    //    CmdBulletMove();
    //}

    //[Command(requiresAuthority = false)]
    //private void CmdBulletMove()
    //{
    //    _rigidbody.velocity = _bulletSpeed * Vector2.up;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);

        // compare tags
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealthController>().TakeDamage(_damage);
        }
        else if (collision.gameObject.CompareTag("BrickWall"))
        {
            collision.gameObject.GetComponent<BrickWall>().CmdRemoveWall();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
        }
        else
        {
            throw new Exception("Bullet hit in uncertain object");
        }
    }
}
