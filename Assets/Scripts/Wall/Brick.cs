using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Brick : NetworkBehaviour
{
    [Command]
    public void CmdDestroy()
    {
        NetworkServer.UnSpawn(gameObject);
        Destroy(gameObject);
    }
}
