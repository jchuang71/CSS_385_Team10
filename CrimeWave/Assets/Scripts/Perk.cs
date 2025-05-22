using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    public PerkEffect effect;
    public PerkTimer perkTimer;
    
    private TextMeshProUGUI perkNameText;
    private TextMeshProUGUI perkDescriptionText;
    private TextMeshProUGUI perkDurationText;
    private Button selectButton;

    void Start()
    {
        perkNameText = transform.Find("PerkName").GetComponent<TextMeshProUGUI>();
        perkDescriptionText = transform.Find("PerkDescription").GetComponent<TextMeshProUGUI>();
        perkDurationText = transform.Find("PerkDuration").GetComponent<TextMeshProUGUI>();
        selectButton = transform.Find("PerkSelect").GetComponent<Button>();

        perkNameText.text = effect.perkName;
        perkDescriptionText.text = effect.perkDescription;

        if (!effect.isPermanent)
            perkDurationText.text = effect.perkDuration + " secs";
        else
            perkDurationText.text = "Permanent";
    }

    public void ActivatePerk()
    {
        PlayerManager.localPlayerInstance.GetComponent<PlayerPerk>().ActivatePerk(effect); /// activate the perk on the player so duration script stays with player
        transform.parent.gameObject.SetActive(false);

        perkTimer.StartCountdown();
    }
}