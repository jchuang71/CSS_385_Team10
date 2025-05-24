using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
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
        //photonView.ObservedComponents.Add(GetComponent<CurrencyHandler>());

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

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject; // set a reference to a player's game object through tagobject
    }
}