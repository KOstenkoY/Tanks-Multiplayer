using Mirror;

public class BrickWall : NetworkBehaviour
{
    public void RemoveWall()
    {
        Destroy(gameObject);
    }
}
