using System;
using UnityEngine;
using Mirror;

public class PlayerWeapons : NetworkBehaviour
{
    [SyncVar, SerializeField] private float _bulletSpeed = 6;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _bulletSpawnPosition;

    private int _countBullets = 1;


    private void OnEnable()
    {
        Bullet.OnBulletReturn += ReturnBullet;
    }

    private void OnDisable()
    {
        Bullet.OnBulletReturn -= ReturnBullet;
    }

    private void Start()
    {
        if (isClient && isLocalPlayer)
        {
            InputManager.Instance.SetWeapons(this);
        }
    }

    public void TryToFire()
    {
        if (_countBullets > 0)
        {
            CmdFire();

            _countBullets--;
        }
    }

    [Command]
    public void CmdFire()
    {
        // create the bullet from te prefab
        GameObject bullet = (GameObject)Instantiate(_bullet, _bulletSpawnPosition.position, _bulletSpawnPosition.rotation);

        // add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.forward * _bulletSpeed;

        //  spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);
    }

    private void ReturnBullet()
    {
        _countBullets++;
    }
}
