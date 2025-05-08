using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab; // Reference in inspector
    private GameObject myPlayer;
    private int maxPlayers = 2; // Maximum number of players allowed in the game

    void Start()
    {
        // COMPLETELY REVISED: Simplified player instantiation logic
        Debug.Log("GameManager Start - Is connected: " + PhotonNetwork.IsConnected); // ADDED: Basic connection check
        if (PhotonNetwork.IsConnected)
        {
            if(PlayerManager.localPlayerInstance == null)
                CreatePlayer(); // MODIFIED: Always try to create a player if connected
        }
    }

/*
    public override void OnJoinedRoom()
    {
        CreatePlayer(); // MODIFIED: Always try to create a player if connected
    }
    */

    void CreatePlayer()
    {
        Debug.Log("Attempting to create player");

        int playerIndex = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % maxPlayers;
        Debug.Log("Player index: " + playerIndex + " from ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber);

        Vector3 position = new Vector3(-2 + playerIndex * 4, 0, 0);

        string prefabName = "Prefabs/" + playerPrefab.name; //Correct path for Photon

        // Test load to ensure prefab exists in Resources
        GameObject prefabTest = Resources.Load<GameObject>(prefabName);
        if (prefabTest == null)
        {
            Debug.LogError("Cannot find prefab: " + prefabName + " in Resources folders");
            return;
        }

        try
        {
            myPlayer = PhotonNetwork.Instantiate(prefabName, position, Quaternion.identity);
            Debug.Log("Player instantiated successfully: " + prefabName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error instantiating player: " + e.Message);
            Debug.LogException(e);
        }
    }

    public void LeaveGame()
    {
        // SIMPLIFIED: Just destroy player and return to lobby
        if (myPlayer != null)
        {
            PhotonNetwork.Destroy(myPlayer);
        }
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left room, reconnecting to master server"); // ADDED: Debug info
        SceneManager.LoadScene("Lobby");
        //PhotonNetwork.ConnectToRegion("");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // everybody leaves when host leaves
        LeaveGame();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // host leaves when anybody leaves
        LeaveGame();
    }
}