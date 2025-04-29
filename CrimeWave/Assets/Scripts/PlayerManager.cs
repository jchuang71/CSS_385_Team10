using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviourPun
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
