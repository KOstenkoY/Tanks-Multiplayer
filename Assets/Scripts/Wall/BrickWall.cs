using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : NetworkBehaviour
{
    public void RemoveWall(GameObject go)
    {
        CmdRemoveWall(go);

        Destroy(go);
    }

    [Command]
    private void CmdRemoveWall(GameObject go)
    {
        RpcRemoveWall(go);
    }

    [ClientRpc]
    private void RpcRemoveWall(GameObject go) => NetworkServer.UnSpawn(go);
    
}
