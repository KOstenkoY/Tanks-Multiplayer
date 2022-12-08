using UnityEngine;
using Mirror;
using System;

public class PlayerHealthController : NetworkBehaviour
{
    [SerializeField] private int _maxHealth = 3;

    [SyncVar(hook = nameof(OnHealthChanged))]
    private int _health = 0;

    public bool isDead => _health == 0;

    public override void OnStartServer()
    {
        _health = _maxHealth;
    }

    private void OnHealthChanged(int oldValue, int newValue)
    {
        if(oldValue > newValue)
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
            CmdTakeDamage();
        }
    }

    [Command]
    private void CmdTakeDamage()
    {
        RpcHandleDeath();
    }

    [ClientRpc]
    private void RpcHandleDeath()
    {
        gameObject.SetActive(false);
    }
}
