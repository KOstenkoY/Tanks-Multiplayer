using Mirror;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [SerializeField] private int _minPlayers = 2;

    [Scene] [SerializeField] private string _menuScene = string.Empty;
    [SerializeField] private string menuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby _roomPlayerPrefab = null;
    [SerializeField] private ColorHandler _playerColorHandlerPref = null;

    //[Header("Maps")]
    //[SerializeField] private int _numberOfRounds = 1;
    //[SerializeField] private MapSet _mapSet = null;

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby _gamePlayerPrefab = null;
    [SerializeField] private GameObject _playerSpawnSystem = null;
    //[SerializeField] private GameObject _roundSystem = null;

    //private MapHandler _mapHandler;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;
    public static event Action OnServerStopped;

    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();

    // load resources from code 
    //public override void OnStartServer()
    //{
    //    spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    //}

    //public override void OnStartClient()
    //{
    //    var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

    //    foreach (var prefab in spawnablePrefabs)
    //    {
    //        NetworkClient.RegisterPrefab(prefab);
    //    }
    //}

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if(numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        if(SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if(SceneManager.GetActiveScene().name == menuScene)
        {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(_roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

            RoomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();

        RoomPlayers.Clear();
        GamePlayers.Clear();
    }

    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < _minPlayers)
            return false;

        foreach (var player in RoomPlayers)
        {
            if (!player.isReady)
            {
                return false;
            }
        }

        return true;
    }

    public void StartGame()
    {
        //if(SceneManager.GetActiveScene().name == _menuScene)
        //{
        //    if (!IsReadyToStart())
        //    {
        //        return;
        //    }

        //    _mapHandler = new MapHandler(_mapSet, _numberOfRounds);

        //    ServerChangeScene(_mapHandler.NextMap);
        //}

        if (SceneManager.GetActiveScene().name == menuScene)
        {
            if (!IsReadyToStart())
            {
                return;
            }

            ServerChangeScene("SampleScene");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        // From menu to game
        if (SceneManager.GetActiveScene().name == menuScene && newSceneName.StartsWith("SampleScene"))
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; i--)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayerInstance = Instantiate(_gamePlayerPrefab);
                gameplayerInstance.SetDisplayName(RoomPlayers[i].displayName);

                NetworkServer.Destroy(conn.identity.gameObject);

                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName.StartsWith("SampleScene"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(_playerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }

    public ColorHandler SetColorHandler()
    {
        if (_playerColorHandlerPref != null)
            return _playerColorHandlerPref;
        else
            throw new NullReferenceException("Foget set instance to _playerColorHandlePrefab");
    }
}