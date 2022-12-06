using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby _networkManagerLobby = null;

    [Header("UI")]
    [SerializeField] private GameObject _landingPagePanel = null;
    [SerializeField] private InputField _ipAddressInputField = null;
    [SerializeField] private Button _joinButton = null;

    private void OnEnable()
    {
        NetworkManagerLobby.OnClientConnected += HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
        NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

    private void Start()
    {
        _ipAddressInputField.text = "localhost";
    }

    public void JoinLobby()
    {
        string ipAddress = _ipAddressInputField.text;

        _networkManagerLobby.networkAddress = ipAddress;
        _networkManagerLobby.StartClient();

        _joinButton.interactable = false;
    }

    private void HandleClientConnected()
    {
        _joinButton.interactable = true;

        gameObject.SetActive(false);
        _landingPagePanel.gameObject.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        _joinButton.interactable = true;
    }
}
