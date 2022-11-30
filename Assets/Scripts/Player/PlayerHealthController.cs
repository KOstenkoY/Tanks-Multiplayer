using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerHealthController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnHealthChanged)), SerializeField]
    private int _health = 3;

    private void OnHealthChanged(int oldValue, int newValue)
    {
        if(newValue > 0)
        {
            _health = newValue;
        }
        else
        {
            throw new ArgumentException("Incorrect argument value");
        }
    }

    public void TakeDamage(int damage)
    {
        OnHealthChanged(_health, _health - damage);

        if (_health <= 0)
        {
            Destroy(gameObject);

            CmdTakeDamage();
        }
    }

    [Command]
    private void CmdTakeDamage()
    {
        RpcTakeDamage();

        //_health -= damage;

        //if (_health <= 0)
        //{
        //    Destroy(gameObject);

        //    NetworkServer.UnSpawn(gameObject);
        //}
    }

    [ClientRpc]
    private void RpcTakeDamage()
    {
        gameObject.SetActive(false);
    }
}
