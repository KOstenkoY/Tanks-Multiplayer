using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : NetworkManager
{
    [SerializeField] private Transform[] _startPositions;

    // helper index for Random Spawn our Players, when game starts
    private int _helperIndex = 0;

    public override void OnStartServer()
    {
        base.OnStartServer();

        // specify which struct should come to the server in order for the swap to be performed
        NetworkServer.RegisterHandler<PositionMessage>(OnCreateCharacter);

        ShuffleArray(_startPositions);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        PositionMessage pos = new PositionMessage { startPosition = _startPositions[_helperIndex].position };

        _helperIndex++;

        NetworkClient.Send(pos);
    }

    private void OnCreateCharacter(NetworkConnectionToClient conn, PositionMessage message)
    {
        // playerPrefab is the one assigned in the inspector in Network Manager
        GameObject player = Instantiate(playerPrefab, message.startPosition, Quaternion.identity);

        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, player);
    }

    private void ShuffleArray<T>(T[] array)
    {
        for(int i = 0; i < array.Length; i++)
        {
            T tmp = array[i];
            int r = Random.Range(i, array.Length);
            array[i] = array[r];
            array[r] = tmp;
        }
    } 

    public struct PositionMessage : NetworkMessage
    {
        public Vector3 startPosition;
    }
}
