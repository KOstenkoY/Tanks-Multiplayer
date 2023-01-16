using Mirror;
using System;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class PlayerHealthController : NetworkBehaviour
{
    [SyncVar, SerializeField] private float _delayBeforeSpawning = 0.5f;

    // must be equals count of Healt images (UI)
    [SyncVar, SerializeField] private int _maxHealth = 3;

    [SyncVar(hook = nameof(HealthChanged))]
    private int _health = 0;

    public bool isDead => _health == 0;

    public static Action OnHealthHandler;

    public override void OnStartClient()
    {
        _health = _maxHealth;
    }

    private void HealthChanged(int oldValue, int newValue) {}

    public void TakeDamage(int damage)
    {
        if (isOwned)
            OnHealthHandler?.Invoke();

        if (isClient)
        {
            CmdHandleDeath(gameObject, damage);
        }
    }

    [Command]
    private void CmdHandleDeath(GameObject player, int damage)
    {
        _health -= damage;

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

        DelayBeforeSpawning(_delayBeforeSpawning);

        //gameObject.SetActive(true);
    }

    private async void DelayBeforeSpawning(float milliseconds)
    {
        await Task.Delay((int)(milliseconds * 1000));

        gameObject.SetActive(true);
    }
}
