using System;
using UnityEngine;
using Mirror;

public class PlayerWeapons : NetworkBehaviour
{
    [SyncVar, SerializeField] private float _bulletSpeed = 6;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _bulletSpawnPosition;

    private int _countBullets = 1;

    private void Start()
    {
        if (isClient && isLocalPlayer)
        {
            InputManager.Instance.SetWeapons(this);
        }
    }

    public void TryToFire()
    {
        // take bullet from object pool
        GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();

        if (bullet != null)
        {
            bullet.transform.position = _bulletSpawnPosition.position;
            bullet.transform.rotation = _bulletSpawnPosition.rotation;

            bullet.SetActive(true);
        }
    }

    [Command]
    public void CmdFire(GameObject bullet)
    {
        RpcFire(bullet);
    }

    [ClientRpc]
    private void RpcFire(GameObject bullet)
    {
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
