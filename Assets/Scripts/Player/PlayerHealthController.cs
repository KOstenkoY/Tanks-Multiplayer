using Mirror;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHealthController : NetworkBehaviour
{
    [SyncVar, SerializeField] private float _delayBeforeSpawning = 0.5f;

    // must be equals count of Healt images (UI)
    [SerializeField] private int _maxHealth = 3;

    [SyncVar(hook = nameof(HealthChanged))]
    private int _health = 0;

    public bool isDead => _health == 0;

    public static Action OnHealthHandler;

    private NetworkManagerLobby _room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (_room != null)
            {
                return _room;
            }

            return _room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }


    public override void OnStartClient()
    {
        _health = _maxHealth;
    }

    private void HealthChanged(int oldValue, int newValue) {}

    public void TakeDamage(int damage)
    {
        _maxHealth -= damage;

        if (isOwned)
        {
            OnHealthHandler?.Invoke();
            GameManager.Instance.ResetPosition();
        }

        if (_maxHealth <= 0)
        {
            if(isOwned)
            {
                gameObject.SetActive(false);

                PlayerDead();
            }    
        }

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
        {
            RpcDestroyPlayer();
        }
        else
        {
            RpcHandleDeath();
        }
    }

    [ClientRpc]
    private void RpcHandleDeath()
    {
        gameObject.SetActive(false);

        if(isClient)
            GameManager.Instance.SpawnPlayer(gameObject);

        DelayBeforeSpawning(_delayBeforeSpawning);
    }

    [ClientRpc]
    private void RpcDestroyPlayer()
    {
        gameObject.SetActive(false);
    }

    private async void DelayBeforeSpawning(float milliseconds)
    {
        await Task.Delay((int)(milliseconds * 1000));

        gameObject.SetActive(true);
    }

    private void PlayerDead()
    {
        CustomGameManager.Instance.RemovePlayer(gameObject.GetComponent<Player>());
    }
}
