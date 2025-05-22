using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject localPlayerInstance;

    private void Awake()
    {
        // Assign the local player instance only for the local player
        if (photonView.IsMine)
        {
            if (localPlayerInstance != null)
            {
                Debug.LogWarning("LocalPlayerInstance is being reassigned. This could be an error.");
            }

            localPlayerInstance = gameObject;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            GameManager.playerList.Add(localPlayerInstance); // add to player list at instantiation

            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);
            float a = 1f;
            photonView.RPC("SetPlayerColor", RpcTarget.All, new object[] { r, g, b, a });
        }
    }

    [PunRPC]
    public void SetPlayerColor(float r, float g, float b, float a)
    {
        GetComponent<SpriteRenderer>().color = new Color(r, g, b, a);
    }
}