using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerWeapons : NetworkBehaviour
{
    [SerializeField] private Transform _bulletSpawnPosition;

    // bullet Prefab
    [SerializeField] private GameObject _bulletPrefab = null;

    [SyncVar, SerializeField] private float _bulletSpeed = 5f;

    // list with our Bullets
    private List<GameObject> _bulletsList = new List<GameObject>();

    // basic count of our bullets
    private int _countBullets = 1;

    public override void OnStartAuthority()
    {
        InputManager.Instance.SetWeapons(this);

        InitializeBullets();
    }

    [Command]
    public void CmdFire()
    {
        RpcFire();
    }

    [ClientRpc]
    public void RpcFire()
    {
        GameObject bullet = GetBullet();

        if (bullet != null)
        {
            bullet.transform.position = _bulletSpawnPosition.position;
            bullet.transform.rotation = _bulletSpawnPosition.rotation;

            bullet.SetActive(true);

            bullet.GetComponent<Rigidbody2D>().velocity = _bulletSpeed * _bulletSpawnPosition.up;
        }

    }

    public void InitializeBullets()
    {
        GameObject bullet;

        for (int i = 0; i < _countBullets; i++)
        {
            bullet = Instantiate(_bulletPrefab);
            bullet.SetActive(false);

            _bulletsList.Add(bullet);
        }
    }

    private GameObject GetBullet()
    {
        if (_bulletsList.Count != 0)
        {
            for (int i = 0; i < _bulletsList.Count; i++)
            {
                if (!_bulletsList[i].activeSelf)
                {
                    _bulletsList[i].SetActive(true);

                    return _bulletsList[i];
                }
            }
        }

        return null;
    }
}
