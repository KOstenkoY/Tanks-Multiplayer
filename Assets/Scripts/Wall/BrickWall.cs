using Mirror;

public class BrickWall : NetworkBehaviour
{
    public void RemoveWall()
    {
        Destroy(gameObject);
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
