using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviourPun, IOnEventCallback
{
    public UIManager uiManager;

    private float gameLength = 60f;
    private float currentTime;
    private bool isCounting;

    private byte gameTimerEventCode = 10;
    private byte gameTimeDoneCode = 11;

    private void Start()
    {
        uiManager = UIManager.UIManagerInstance;
        currentTime = gameLength;

        if (PhotonNetwork.IsMasterClient)
        {
            StartCountdown();
        }
    }

    public void StartCountdown()
    {
        isCounting = true;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while(isCounting)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
            OnCountdownDecrement_Send();
        }
    }

    private void OnCountdownDecrement_Send()
    {
        if (currentTime >= 0)
        {
            object[] package = new object[] { currentTime };
            PhotonNetwork.RaiseEvent(
                gameTimerEventCode,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
                );
        }
        else
        {
            object[] package = new object[] { currentTime };
            PhotonNetwork.RaiseEvent(
                gameTimeDoneCode,
                package,
                new RaiseEventOptions { Receivers = ReceiverGroup.All },
                new SendOptions { Reliability = true }
                );
        }
    }
    private void OnCountdownDecrement_Receive(object[] data)
    {
        currentTime = (float)data[0];

        string minutes = ((int)currentTime / 60).ToString("00");
        string seconds = ((int)currentTime % 60).ToString("00");
        uiManager.gameTimerText.text = "Time until ioland explodes: " + minutes + ":" + seconds;
    }

    private void OnGameDone_Receive(object[] data)
    {
        uiManager.endText.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code == gameTimerEventCode)
        {
            OnCountdownDecrement_Receive((object[])photonEvent.CustomData);
        }
        else if(photonEvent.Code == gameTimeDoneCode)
        {
            OnGameDone_Receive((object[])photonEvent.CustomData);
        }
    }
}
