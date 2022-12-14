using Mirror;
using UnityEngine;

public class BrickWall : NetworkBehaviour
{
    [ClientRpc]
    private void RemoveWall()
    {
        NetworkServer.UnSpawn(this.gameObject);

        Destroy(this.gameObject);
    }

    [Command]
    public void CmdRemoveWall()
    {
        RemoveWall();
    }
}
