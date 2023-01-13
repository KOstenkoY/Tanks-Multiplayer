using Mirror;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnSystem : NetworkBehaviour
{
    // must be equals lobby available colors
    [SerializeField] private GameObject[] _playerPrefabs = null;

    private static List<Transform> _spawnPoints = new List<Transform>();

    private int _nextIndex = 0;

    private NetworkManagerLobby _room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (_room != null)
                return _room;

            return _room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public static void AddSpawnPoint(Transform transform)
    {
        _spawnPoints.Add(transform);

        _spawnPoints = _spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoints(Transform transform)
    {
        _spawnPoints.Remove(transform);
    }

    public override void OnStartServer()
    {
        NetworkManagerLobby.OnServerReadied += SpawnPlayer;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        NetworkManagerLobby.OnServerReadied -= SpawnPlayer;
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        SpawnPlayerInGame(conn);
    }

    [Server]
    private void SpawnPlayerInGame(NetworkConnection conn)
    {
        Transform spawnPoint = _spawnPoints.ElementAtOrDefault(_nextIndex);

        if (spawnPoint == null)
        {
            Debug.LogError($"Missing spawn point for player {_nextIndex}");
            return;
        }

        GameObject playerInstance = Instantiate(
            _playerPrefabs[Room.RoomPlayers[_nextIndex].ColorId],
            _spawnPoints[_nextIndex].position, 
            _spawnPoints[_nextIndex].rotation);

        NetworkServer.Spawn(playerInstance, conn);

        //////////
        playerInstance.GetComponent<Player>().PlayerName = Room.GamePlayers[_nextIndex].DisplayName;

        _nextIndex++;
    }
}