using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class Timer : MonoBehaviour
{
    protected float maxTime;
    protected float currentTime;
    protected bool isCounting = false;
    protected bool clientOnly = false;

    void Update()
    {
        if (isCounting)
        {
            // trigger something at timer decrement
            if (clientOnly)
                OnCountdownDecrement();
            else
                GetComponent<PhotonView>().RPC("OnCountdownDecrement", RpcTarget.All);

            if(currentTime <= 0)
            {
                // trigger something at countdown done
                if (clientOnly)
                    OnCountdownDone();
                else
                    GetComponent<PhotonView>().RPC("OnCountdownDone", RpcTarget.All);
            }
        }
    }

    public void SetMaxTime(float time)
    {
        maxTime = time;
        currentTime = time;
    }

    public void StartCountdown()
    {
        isCounting = true;
    }

    protected abstract void OnCountdownDecrement(); // to be overrided for custom functionality

    protected abstract void OnCountdownDone(); // to be overrided for custom functionality
}
