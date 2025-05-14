using Photon.Pun;
using TMPro;
using UnityEngine;

public class Perk : MonoBehaviour
{
    public PerkEffect effect;

    private TextMeshProUGUI perkNameText;
    private TextMeshProUGUI perkDescriptionText;
    private TextMeshProUGUI perkDurationText;

    void Start()
    {
        perkNameText = transform.Find("PerkName").GetComponent<TextMeshProUGUI>();
        perkDescriptionText = transform.Find("PerkDescription").GetComponent<TextMeshProUGUI>();
        perkDurationText = transform.Find("PerkDuration").GetComponent<TextMeshProUGUI>();

        perkNameText.text = effect.perkName;
        perkDescriptionText.text = effect.perkDescription;

        if (!effect.isPermanent)
            perkDurationText.text = "Duration: " + effect.perkDuration + " secs";
        else
            perkDurationText.text = "Permanent";
    }

    public void ActivatePerk()
    {
        effect.Apply(PlayerManager.localPlayerInstance);

        if(!effect.isPermanent)
            StartCoroutine(effect.StartDuration(PlayerManager.localPlayerInstance));

        transform.parent.parent.GetComponent<PerkUI>().PerkSelected();
        // disable SelectPerk game object
        //transform.parent.parent.gameObject.SetActive(false);
    }
}