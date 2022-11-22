using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _bulletSpeed = 7;

    private Rigidbody2D _rigidbody;

    private float _gravityScale = 0;

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
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().CmdTakeDamage(_damage);

        }
        else if (collision.CompareTag("BrickWall"))
        {
            // destroy brick wall and destroy this object with animation
            // collision.GetComponent<>
        }
        else if(collision.CompareTag("Wall"))
        {
            // destroy object with animation
        }
        else
        {
            // throw new Exception about bag in game
        }
        Destroy(gameObject);
    }
}
