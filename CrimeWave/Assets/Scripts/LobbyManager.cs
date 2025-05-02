using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UIElements;
using ExitGames.Client.Photon;
using System.Collections;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // Players
    public GameObject player1;
    public GameObject player2;


    //public GameObject playersPanel;
    public TMP_Text player1Text;
    public TMP_Text player2Text;

    // lobby related game objects
    public TMP_InputField roomInputField;
    public TMP_InputField joinRoomInputField;
    public GameObject lobbyUI;
    public GameObject roomsList;

    // room related game objects
    public GameObject roomUI;
    public TMP_Text roomName;
    public GameObject playButton;

    public GameObject roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    // This is called when left the room, rejoin lobby once left the room to continue updating room lists
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Successfully joined lobby.");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Failed to join room: {message}");
        // Optional: Show a message to the user that the room doesn't exist
        // Could add a UI Text component to display errors
    }

    public override void OnJoinedRoom()
    {
        lobbyUI.SetActive(false);
        roomUI.SetActive(true);

        // Disable play button for non hosts
        if(PhotonNetwork.IsMasterClient)
            playButton.SetActive(true);
        else
            playButton.SetActive(false);

        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name +
                            " (" + PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                            PhotonNetwork.CurrentRoom.MaxPlayers + ")";

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject p1 = PhotonNetwork.Instantiate("Player1Square", Vector2.zero, Quaternion.identity);
            //p1.GetComponent<SpriteRenderer>().enabled = false;
            //p1.GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            GameObject p2 = PhotonNetwork.Instantiate("Player2Square", new Vector2(2, 2), Quaternion.identity);
        }

        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Updated room list..");

        foreach (RoomItem room in roomItemsList)
        {
            Destroy(room.gameObject);
        }

        roomItemsList.Clear();

        foreach (RoomInfo room in roomList)
        {
            if (!room.RemovedFromList)
            {
                RoomItem newRoom = Instantiate(roomItemPrefab, contentObject).GetComponent<RoomItem>();
                newRoom.SetRoomInfo(room.Name, room.PlayerCount, room.MaxPlayers);
                roomItemsList.Add(newRoom);
            }
        }
    }

    public override void OnLeftRoom()
    {
        roomUI.SetActive(false);
        lobbyUI.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name +
                         " (" + PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                         PhotonNetwork.CurrentRoom.MaxPlayers + ")";
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name +
                         " (" + PhotonNetwork.CurrentRoom.PlayerCount + "/" +
                         PhotonNetwork.CurrentRoom.MaxPlayers + ")";
        UpdatePlayerList();
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickJoinRoom()
    {
        if (!string.IsNullOrEmpty(joinRoomInputField.text))
        {
            JoinRoom(joinRoomInputField.text);
        }
    }
    public void OnClickCreate()
    {
        PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 2 });
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnClickPlay()
    {
        Debug.Log("Play button clicked"); // ADDED: Debug log

        // Always load the Game scene
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client loading level for all players"); // ADDED: Debug info
            PhotonNetwork.LoadLevel("Game");
        }
        else
        {
            Debug.Log("Non-master client loading scene locally"); // ADDED: Debug info
            SceneManager.LoadScene("Game");
        }

    }

    private void UpdatePlayerList()
    {
        // Reset player texts
        player1Text.text = "";
        player2Text.text = "";

        // Add text for each player in room
        if (PhotonNetwork.CurrentRoom != null)
        {
            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < players.Length; i++)
            {
                Player player = players[i];
                string playerText = player.NickName;

                // Add "(You)" after your own name
                if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    playerText += " (You)";
                }

                // Assign to the appropriate text field
                if (i == 0)
                {
                    player1Text.text = playerText;
                }
                else if (i == 1)
                {
                    player2Text.text = playerText;
                }
            }
        }
    }

}
