using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] private GameObject playerListPrefab;
    [SerializeField] private GameObject playerListContainer;

    private List<GameObject> playersListObjects = new List<GameObject>();

    private bool tabHeld = false;
    private float updateInterval = 1.5f;

    void Start()
    {
        for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            GameObject newPlayer = Instantiate(playerListPrefab, transform.position, Quaternion.identity, playerListContainer.transform);
            playersListObjects.Add(newPlayer);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerListContainer.SetActive(true);
            tabHeld = true;
            StartCoroutine(UpdatePlayerList());
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            playerListContainer.SetActive(false);
            tabHeld = false;
            StopCoroutine(UpdatePlayerList());
        }
    }

    IEnumerator UpdatePlayerList()
    {
        while(tabHeld)
        {
            for (int i = 0; i < playersListObjects.Count; i++)
            {
                GameObject player = GameManager.playerList[i]; // get actual player object in scene
                GameObject playerList = playersListObjects[i]; // player listing

                // set sprite and color
                playerList.transform.GetChild(0).GetComponent<Image>().sprite = player.GetComponent<SpriteRenderer>().sprite;
                playerList.transform.GetChild(0).GetComponent<Image>().color = player.GetComponent<SpriteRenderer>().color;

                // set name
                playerList.transform.GetChild(1).GetComponent<TMP_Text>().text = player.GetComponent<PhotonView>().Owner.NickName;

                // set money have
                playerList.transform.GetChild(2).GetComponent<TMP_Text>().text = "$" + player.GetComponent<CurrencyHandler>().money.ToString();
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
