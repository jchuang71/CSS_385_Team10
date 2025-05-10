using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{

    public TMP_Text roomName;
    LobbyManager manager;
    private string roomNameOnly;

    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string _roomName)
    {
        roomNameOnly = _roomName;
        roomName.text = _roomName;
    }
    public void SetRoomInfo(string _roomName, int currentPlayers, int maxPlayers)
    {
        roomNameOnly = _roomName;
        roomName.text = $"{_roomName} ({currentPlayers}/{maxPlayers})";
    }

    public string GetRoomNameOnly()
    {
        return roomNameOnly;
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomNameOnly);
    }

}
