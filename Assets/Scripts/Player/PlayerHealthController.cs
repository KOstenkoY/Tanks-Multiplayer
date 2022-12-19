using Mirror;
using System;
using UnityEngine;

public class PlayerHealthController : NetworkBehaviour
{
    // must be equals count of Healt images (UI)
    [SyncVar, SerializeField] private int _maxHealth = 3;

    [SyncVar(hook = nameof(HealthChanged))]
    public int _health = 0;

    public bool isDead => _health == 0;

    public static Action OnHealthHandler;

    public override void OnStartServer()
    {
        _health = _maxHealth;
    }

    private void HealthChanged(int oldValue, int newValue)
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
        if (isOwned)
        {
            OnHealthHandler?.Invoke();

            _health -= damage;

            CmdHandleDeath();

            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    [Command]
    private void CmdHandleDeath()
    {
        RpcHandleDeath();
    }

    [TargetRpc]
    private void RpcHandleDeath()
    {
        if(_health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);

            GameManager.Instance.SpawnPlayer(gameObject);
        }
    }
}
