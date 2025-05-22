using TMPro;
using Photon.Pun;
using UnityEngine;

public class PerkTimer : Timer
{
    public TMP_Text perkTimerText;
    public PerkUI perkUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clientOnly = true;

        if(GetComponent<PhotonView>().IsMine)
        {
            SetMaxTime(10f);
            StartCountdown();
        }
    }

    protected override void OnCountdownDecrement()
    {
        currentTime = currentTime -= Time.deltaTime;

        string minutes = ((int)currentTime / 60).ToString("00");
        string seconds = ((int)currentTime % 60).ToString("00");
        perkTimerText.text = "Time until next perk: " + minutes + ":" + seconds;
    }

    protected override void OnCountdownDone()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            isCounting = false;
            perkUI.RollRandomPerks();
            currentTime = maxTime;
        }
    }
}
