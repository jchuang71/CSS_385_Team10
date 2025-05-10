using Photon.Pun;
using System.Collections;
using UnityEngine;

public class DestructibleObjectManager : MonoBehaviourPunCallbacks
{
    public static DestructibleObjectManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Call this to respawn an object
    public void RespawnDestructible(string prefabName, Vector3 position, float delay)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(RespawnRoutine(prefabName, position, delay));
        }
    }

    private IEnumerator RespawnRoutine(string prefabName, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        PhotonNetwork.InstantiateRoomObject("Prefabs/DestructibleObjects/" + prefabName, position, Quaternion.identity);
        Debug.Log("Respawned destructible: " + prefabName);
    }
}
