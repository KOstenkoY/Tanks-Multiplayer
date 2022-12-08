using System;
using UnityEngine;
using Mirror;

public class PlayerWeapons : NetworkBehaviour
{
    [SerializeField] private Transform _bulletSpawnPosition;

    public override void OnStartAuthority()
    {
        InputManager.Instance.SetWeapons(this);

        ObjectPool.SharedInstance.InitializePool();
    }

    public void Fire()
    {
        GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();

        if (bullet != null)
        {
            CmdFire(bullet);
        }
    }

    [Command]
    public void CmdFire(GameObject bullet)
    {
        // take bullet from object pool
        bullet.transform.position = _bulletSpawnPosition.position;
        bullet.transform.rotation = _bulletSpawnPosition.rotation;

        bullet.SetActive(true);
    }
}
