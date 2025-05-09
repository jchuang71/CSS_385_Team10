using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{

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
        Debug.Log("Attempting to create player"); // ADDED: Debug log

        // Get player index (0 or 1) based on actor number
        int playerIndex = (PhotonNetwork.LocalPlayer.ActorNumber - 1) % 2; // FIXED: Ensure valid index with modulo
        Debug.Log("Player index: " + playerIndex + " from ActorNumber: " + PhotonNetwork.LocalPlayer.ActorNumber); // ADDED: Debug info

        // Create player at different positions based on player number
        Vector3 position = new Vector3(-2 + playerIndex * 4, 0, 0);

        // CHANGED: Only one prefab is in use now
        string prefabName = "Prefabs/" + playerPrefab.name;

        // Non-hosts players become blue
        

        // FIXED: Test loading first to make sure resources are correctly set up
        GameObject prefabTest = Resources.Load<GameObject>(prefabName);
        if (prefabTest == null)
        {
            Debug.LogError("Cannot find prefab: " + prefabName + " in Resources folders"); // ADDED: Better error detection
            return;
        }

        // Instantiate the player using the prefab name
        try
        {
            myPlayer = PhotonNetwork.Instantiate(prefabName, position, Quaternion.identity);
            photonView.RPC("AddPlayerRPC", RpcTarget.All);

            Debug.Log("Player instantiated successfully: " + prefabName); // ADDED: Success confirmation
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error instantiating player: " + e.Message); // ADDED: Error handling
            Debug.LogException(e); // ADDED: Full exception details
        }
    }

    [PunRPC]
    public void AddPlayerRPC()
    {
        currentPlayers++;

        if(currentPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            GetComponent<DestructibleObjectManager>().StartSpawning();
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