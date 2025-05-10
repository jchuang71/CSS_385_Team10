using Photon.Pun;
using UnityEngine;

public class PlayerCamera : MonoBehaviourPun
{

    private Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        FollowLocalPlayer();
    }

    void FollowLocalPlayer()
    {

        if(photonView.IsMine)
            cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3 (transform.position.x, transform.position.y, -10), 5.0f * Time.deltaTime);
    }
}
