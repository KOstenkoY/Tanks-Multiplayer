using Mirror;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : NetworkBehaviour
{
    private Rigidbody2D _rigidbody = null;

    [SerializeField] private int _damage = 1;

    [SyncVar, SerializeField] private float _bulletSpeed = 1.5f;

    private float _gravityScale = 0;

    // count of bullets that player get back after bullet got into something
    private int _countBulletsReturn = 1;

    public override void OnStartAuthority()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.gravityScale = _gravityScale;
    }

    private void Update()
    {
        CmdBulletMove();
    }

    [Command]
    private void CmdBulletMove()
    {
        transform.Translate(new Vector2(0, _bulletSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // compare tags
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealthController>().TakeDamage(_damage);
        }
        else if (collision.CompareTag("BrickWall"))
        {
            collision.GetComponent<BrickWall>().RemoveWall(collision.gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
        }
        else
        {
            throw new Exception("Bullet hit in uncertain object");
        }

        CmdRemoveBullet(this.gameObject);
    }

    [Command]
    private void CmdRemoveBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
