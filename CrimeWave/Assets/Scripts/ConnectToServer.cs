using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField userNameInputField; // Input field for the player to enter their username
    [SerializeField] private Button connectButton; // Button to initiate connection to the server

    string userName = "Player"; // Default username for the player
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        // Set the initial text in the input field to the default userName
        if (userNameInputField != null)
        {
            userNameInputField.text = userName;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnClickConnectToServer();
        }
    }

    public void OnClickConnectToServer()
    {

        if (!string.IsNullOrEmpty(userNameInputField.text))
        {
            userName = userNameInputField.text;
        }

        PhotonNetwork.NickName = userName; // Set the player's nickname to the default username
        connectButton.GetComponentInChildren<TextMeshProUGUI>().text = "Connecting..."; // Change the button text to indicate connection in progress
        connectButton.interactable = false;

        Debug.Log("directly before connecting awaiting connect...");
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon server using the settings defined in the PhotonServerSettings file
        Debug.Log("Connecting to server...");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LoadLevel("Lobby"); // Load the lobby scene after connecting to the server
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        connectButton.GetComponent<TextMeshProUGUI>().text = "Connect";
        connectButton.interactable = true;
    }
}
