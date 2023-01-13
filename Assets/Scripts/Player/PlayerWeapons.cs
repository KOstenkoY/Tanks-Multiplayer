using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerWeapons : NetworkBehaviour
{
    [SerializeField] private Transform _bulletSpawnPosition;

    [SerializeField] private Bullet _bulletPrefab = null;

    [SyncVar, SerializeField] private float _bulletSpeed = 4f;

    // list with our Bullets
    private List<GameObject> _bulletsList = new List<GameObject>();

    // delay before next shooting after bullet hit something
    [SyncVar, SerializeField] private float _delayBeforeNextShoot = 0.7f;

    // when this flag equals true, we can shoot 
    private bool _canUse = true;

    public override void OnStartAuthority()
    {
        InputManager.Instance.SetWeapons(this);

        _canUse = true;
    }

    [Command]
    public void CmdFire()
    {
        RpcFire();
    }

    [ClientRpc]
    private void RpcFire()
    {
        if (!_canUse)
            return;
        else
            _canUse = false;

        WaitBeforeNextShoot(_delayBeforeNextShoot);

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
            bullet = Instantiate(_bulletPrefab.transform.gameObject, _bulletSpawnPosition.position, _bulletSpawnPosition.rotation);

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

    private async void WaitBeforeNextShoot(float milliseconds)
    {
        await Task.Delay((int)(milliseconds * 1000));

        _canUse = true;
    }
}
