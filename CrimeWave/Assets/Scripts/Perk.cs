using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Perk : MonoBehaviour
{
    public PerkEffect effect;

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
        PlayerManager.localPlayerInstance.GetComponent<PlayerController>().AddPerk(effect);
        transform.parent.gameObject.SetActive(false);
    }
}