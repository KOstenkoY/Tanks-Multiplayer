using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerWeapons : NetworkBehaviour
{
    [SyncVar, SerializeField] private float _bulletSpeed = 6;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _bulletSpawnPosition;

    private void Start()
    {
        if (isClient && isLocalPlayer)
        {
            InputManager.Instance.SetWeapons(this);
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
}
