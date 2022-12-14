using Mirror;
using UnityEngine;

public class BrickWall : NetworkBehaviour
{
    public void RemoveWall()
    {
        Destroy(gameObject);

        CmdRemoveWall();
    }

    [Command]
    public void CmdRemoveWall()
    {
        RpcRemoveWall();
    }

    [ClientRpc]
    private void RpcRemoveWall()
    {
        Destroy(gameObject);
    }
}
