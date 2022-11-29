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
        OnRemoveWall(go);
    }

    [ClientRpc]
    private void OnRemoveWall(GameObject go) => NetworkServer.UnSpawn(go);
    
}
