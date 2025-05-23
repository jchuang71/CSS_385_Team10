using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static List<GameObject> playerList = new List<GameObject>();
    public GameObject playerPrefab; // Reference in inspector
    private GameObject myPlayer;
    private static int currentPlayers;

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

        Vector3 position = new Vector3(-2 + PhotonNetwork.LocalPlayer.ActorNumber * 4, 0, 0);

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

            Debug.Log("Player instantiated successfully: " + prefabName); // ADDED: Success confirmation
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