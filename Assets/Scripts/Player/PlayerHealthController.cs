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

    public override void OnStartClient()
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
            OnHealthHandler?.Invoke();

        if (isClient)
        {
            _health -= damage;

            CmdHandleDeath(gameObject);
        }
    }

    [Command]
    private void CmdHandleDeath(GameObject player)
    {
        if (_health <= 0)
            NetworkServer.Destroy(player);
        else
            RpcHandleDeath();
    }

    [ClientRpc]
    private void RpcHandleDeath()
    {
        gameObject.SetActive(false);

        if(isOwned)
            GameManager.Instance.SpawnPlayer(gameObject);

        gameObject.SetActive(true);
    }
}
