using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameTimer : Timer
{
    public TMP_Text gameTimerText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clientOnly = false;

        if(PhotonNetwork.IsMasterClient)
        {
            SetMaxTime(30f);
            StartCountdown();
        }
    }

    [PunRPC]
    protected override void OnCountdownDecrement()
    {
        currentTime -= Time.deltaTime;

        string minutes = ((int)currentTime / 60).ToString("00");
        string seconds = ((int)currentTime % 60).ToString("00");
        gameTimerText.text = "Time until island explodes: " + minutes + ":" + seconds;
    }

    [PunRPC]
    protected override void OnCountdownDone()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            isCounting = false;
            UIManager.UIManagerInstance.endText.gameObject.SetActive(true);
            currentTime = maxTime; // reset timer

        }
    }
}
