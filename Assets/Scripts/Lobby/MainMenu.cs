using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby _networkManagerLobby;

    [Header("UI")]
    [SerializeField] private GameObject _setMapPanel = null;

    public void HostLobby()
    {
        _networkManagerLobby.StartHost();

        _setMapPanel.SetActive(false);
    }
}
