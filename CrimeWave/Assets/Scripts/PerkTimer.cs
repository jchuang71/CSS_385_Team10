using TMPro;
using Photon.Pun;
using UnityEngine;

public class PerkTimer : MonoBehaviour
{
    private UIManager uiManager;
    private float maxTime;
    private float currentTime;
    private bool isCounting = false;

    void Start()
    {
        uiManager = UIManager.UIManagerInstance;

        SetMaxTime(10f);
        StartCountdown();
    }

    void Update()
    {
        if (isCounting)
        {
            currentTime = currentTime -= Time.deltaTime;
            OnCountdownDecrement();

            if (currentTime <= 0)
            {
                 OnCountdownDone();
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

    private void OnCountdownDecrement()
    {
        string minutes = ((int)currentTime / 60).ToString("00");
        string seconds = ((int)currentTime % 60).ToString("00");
        uiManager.perkTimerText.text = "Time until next perk: " + minutes + ":" + seconds;
    }

    private void OnCountdownDone()
    {
        isCounting = false;
        uiManager.perkUI.RollRandomPerks();
        currentTime = maxTime;
    }
}
