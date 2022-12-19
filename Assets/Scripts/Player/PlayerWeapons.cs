using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerWeapons : NetworkBehaviour
{
    [SerializeField] private Transform _bulletSpawnPosition;

    // bullet Prefab
    [SerializeField] private GameObject _bulletPrefab = null;

    [SyncVar, SerializeField] private float _bulletSpeed = 4f;

    // list with our Bullets
    private List<GameObject> _bulletsList = new List<GameObject>();

    public override void OnStartAuthority()
    {
        InputManager.Instance.SetWeapons(this);
    }

    [Command]
    public void CmdFire()
    {
        Fire();
    }

    [ClientRpc]
    private void Fire()
    {
        GameObject bullet = GetBullet();

        if (bullet != null)
        {
            bullet.transform.position = _bulletSpawnPosition.position;
            bullet.transform.rotation = _bulletSpawnPosition.rotation;

            bullet.SetActive(true);

            bullet.GetComponent<Rigidbody2D>().velocity = _bulletSpeed * _bulletSpawnPosition.up;
        }
        else if (bullet == null && _bulletsList.Count == 0)
        {
            bullet = Instantiate(_bulletPrefab, _bulletSpawnPosition.position, _bulletSpawnPosition.rotation);

            _bulletsList.Add(bullet);

            bullet.GetComponent<Rigidbody2D>().velocity = _bulletSpeed * _bulletSpawnPosition.up;
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
