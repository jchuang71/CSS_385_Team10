using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjectManager : MonoBehaviourPunCallbacks
{
    public List<DestructibleObject> destructibleObjects = new List<DestructibleObject>();
    
    [SerializeField] private GameObject destructibleObject;
    private float spawnInterval = 5.0f;
    private float xBounds;
    private float yBounds;

    private bool isSpawning;

    void Start()
    {
        // should be changed eventually according to map size
        xBounds = 10.0f;
        yBounds = 10.0f;

        isSpawning = true;
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        // endlessly spawn new destructible objects
        // may spawn objects on top of each other, we could also just manually set up the map instead of spawning
        while (isSpawning)
        {
            GameObject obj = PhotonNetwork.InstantiateRoomObject("Prefabs/" + destructibleObject.name, GenerateSpawnPos(), Quaternion.identity);
            obj.GetComponent<DestructibleObjectBehavior>().SetDestructibleType(GetRandomObject());

            Debug.Log("Spawned new object of name: " + obj.GetComponent<DestructibleObjectBehavior>().destructibleObj.name);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private DestructibleObject GetRandomObject()
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
