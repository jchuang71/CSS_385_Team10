using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjectManager : MonoBehaviourPunCallbacks
{
    public List<GameObject> destructibleObjects = new List<GameObject>();

    private float spawnInterval = 5.0f;
    private float xBounds;
    private float yBounds;

    private bool isSpawning;

    void Start()
    {
        // should be changed eventually according to map size
        xBounds = 10.0f;
        yBounds = 10.0f;
    }

    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        // endlessly spawn new destructible objects
        // may spawn objects on top of each other, we could also just manually set up the map instead of spawning
        while (isSpawning)
        {
            GameObject obj = PhotonNetwork.InstantiateRoomObject("Prefabs/DestructibleObjects/" + GetRandomObject().name, GenerateSpawnPos(), Quaternion.identity);

            Debug.Log("Spawned new object of name: " + obj.GetComponent<DestructibleObject>().name);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private GameObject GetRandomObject()
    {
        return destructibleObjects[Random.Range(0, destructibleObjects.Count)];
    }

    private Vector2 GenerateSpawnPos()
    {
        float xPos = Random.Range(-xBounds, xBounds);
        float yPos = Random.Range(-yBounds, yBounds);

        return new Vector2(xPos, yPos);
    }
}
