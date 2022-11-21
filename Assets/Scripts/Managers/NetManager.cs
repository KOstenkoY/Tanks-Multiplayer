using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetManager : NetworkManager
{
    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkServer.RegisterHandler<CreateMMOCharacterMessage>(OnCreateCharacter);
    }


    public override void OnClientConnect()
    {
        base.OnClientConnect();

        // you can send the message here, or whereve
        CreateMMOCharacterMessage characterMessage = new CreateMMOCharacterMessage { color = Color.black, name = "Jeck" };

        NetworkClient.Send(characterMessage);
    }

    private void OnCreateCharacter(NetworkConnectionToClient conn, CreateMMOCharacterMessage characterMessage)
    {
        // playerPrefab is the one assigned in the inspector in Network Manager
        GameObject gameObject = Instantiate(playerPrefab);

        // apply data from the message
        PlayerController player = gameObject.GetComponent<PlayerController>();

        // call this to use this gameobject as the primary controller
        NetworkServer.AddPlayerForConnection(conn, gameObject);
    }

    public struct CreateMMOCharacterMessage : NetworkMessage
    {
        public Color color;
        public string name;
    }
}
