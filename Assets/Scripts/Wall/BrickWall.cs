using Mirror;
using UnityEngine;

public class BrickWall : NetworkBehaviour
{
    public void RemoveWall(GameObject wall)
    {
        CmdRemoveWall(wall);
    }

    [Command]
    private void CmdRemoveWall(GameObject wall)
    {
        RpcRemoveWall(wall);
    }

    [ClientRpc]
    public void RpcRemoveWall(GameObject wall)
    {
        NetworkServer.UnSpawn(wall);

        Destroy(wall);
    }
}
