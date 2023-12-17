using Mirror;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : NetworkBehaviour
{
    private Rigidbody2D _rigidbody = null;

    [SerializeField] private int _damage = 1;

    private float _gravityScale = 0;

    public override void OnStartAuthority()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _rigidbody.gravityScale = _gravityScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
            return;

        gameObject.SetActive(false);

        CmdDestroyBullet(this.gameObject);

        // compare tags
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealthController>()?.TakeDamage(_damage);
        }
        else if (collision.CompareTag("BrickWall"))
        {
            collision.GetComponent<BrickWall>()?.RemoveWall();
        }
        else
        {
            Debug.LogError("Bullet hit in uncertain object");
        }
    }

    [Command]
    private void CmdDestroyBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
