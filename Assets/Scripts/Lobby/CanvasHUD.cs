using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CanvasHUD : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private Image _startMenu;
    [SerializeField] private Image _stopMenu;

    [Header("Buttons: ")]
    [SerializeField] private Button _buttonHost;
    [SerializeField] private Button _buttonServer;
    [SerializeField] private Button _buttonClient;
    [SerializeField] private Button _buttonStop;

    [Header("Server address")]
    [SerializeField] private InputField _inputFieldAddress;

    [Header("Texts: ")]
    [SerializeField] private Text _serverText;
    [SerializeField] private Text _clientText;

    void Start()
    {
        // update the canvas text if we have manually changed network 
        // managers address from the game object before starting the game scene
        if (NetworkManager.singleton.networkAddress != "localhost")
        {
            _inputFieldAddress.text = NetworkManager.singleton.networkAddress;
        }

        // adds a listener to the main input field and invokes a method when the value changes
        _inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        // make sure to attach these Buttons in the Inspector
        _buttonHost.onClick.AddListener(ButtonHost);
        _buttonServer.onClick.AddListener(ButtonServer);
        _buttonClient.onClick.AddListener(ButtonClient);
        _buttonStop.onClick.AddListener(ButtonStop);

        //This updates the Unity canvas, we have to manually call it ever change, unlike legacy OnGUI 
        SetupCanvas();
    }

    public void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = _inputFieldAddress.text;
    }

    public void ButtonHost()
    {
        NetworkManager.singleton.StartHost();
        SetupCanvas();
    }

    public void ButtonServer()
    {
        NetworkManager.singleton.StartServer();
        SetupCanvas();
    }
    public void ButtonClient()
    {
        NetworkManager.singleton.StartClient();
        SetupCanvas();
    }
    public void ButtonStop()
    {
        if (NetworkServer.active && NetworkClient.isConnected)       // stop host if host mode
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)                         // stop client if client-only
        {
            NetworkManager.singleton.StopClient();
        }
        else if (NetworkServer.active)                               // stop server if server-only
        {
            NetworkManager.singleton.StopServer();
        }

        SetupCanvas();
    }

    public void SetupCanvas()
    {
        // here we will dump majority of the canvas UI that may be changed
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (NetworkClient.active)
            {
                _startMenu.gameObject.SetActive(false);
                _stopMenu.gameObject.SetActive(true);

                _clientText.text = "Connecting to " + NetworkManager.singleton.networkAddress + "...";
            }
            else
            {
                _startMenu.gameObject.SetActive(true);
                _stopMenu.gameObject.SetActive(false);
            }
        }
        else
        {
            _startMenu.gameObject.SetActive(false);
            _stopMenu.gameObject.SetActive(true);

            if (NetworkServer.active)
            {
                _serverText.text = "Server: active." + Transport.active;
            }
            if (NetworkClient.isConnected)
            {
                _clientText.text = "Client: address" + NetworkManager.singleton.networkAddress;
            }
        }
    }
}
