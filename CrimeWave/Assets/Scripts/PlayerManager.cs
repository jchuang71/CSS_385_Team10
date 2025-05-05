using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject localPlayerInstance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.localPlayerInstance = gameObject;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
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
