using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickWall : NetworkBehaviour
{
    [Command]
    public void CmdRemoveWall(GameObject go)
    {
        NetworkServer.Destroy(go);
    }

    public void RemoveWall(GameObject go)
    {
        Destroy(go);
    }
}
