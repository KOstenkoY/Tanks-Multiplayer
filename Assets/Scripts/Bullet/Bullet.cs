using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _bulletSpeed = 1.5f;

    private Rigidbody2D _rigidbody;

    private float _gravityScale = 0;

    // count of bullets that player get back after bullet got into something
    private int _countBulletsReturn = 1;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = _gravityScale;
    }

    private void Update()
    {
        transform.Translate(new Vector2(0, _bulletSpeed * Time.deltaTime));
    }

    [Mirror.ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // compare tags
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().CmdTakeDamage(_damage);

        }
        else if (collision.CompareTag("BrickWall"))
        {
            collision.GetComponent<BrickWall>().CmdRemoveWall(collision.gameObject);
            collision.GetComponent<BrickWall>().RemoveWall(collision.gameObject);
        }
        else if(collision.CompareTag("Wall"))
        {
            // destroy object with animation
        }
        else
        {
            // throw new Exception about bag in game
        }

        gameObject.SetActive(false);
    }
}
