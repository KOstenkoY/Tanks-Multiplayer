using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    private void Awake()
    {
        PlayerSpawnSystem.AddSpawnPoint(transform);
    }

    private void OnDestroy()
    {
        PlayerSpawnSystem.RemoveSpawnPoints(transform);
    }

    // graphical display of spawn points
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
    }*/
}
