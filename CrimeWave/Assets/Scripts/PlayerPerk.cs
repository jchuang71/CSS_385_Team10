using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPerk : MonoBehaviour
{
    [SerializeField] private GameObject perkTextPrefab;

    private UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = UIManager.UIManagerInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivatePerk(PerkEffect effect)
    {
        effect.Apply(PlayerManager.localPlayerInstance);

        if (!effect.isPermanent)
        {
            GameObject perkText = Instantiate(perkTextPrefab, transform.position, Quaternion.identity, uiManager.activeTempPerks.transform);
            perkText.GetComponent<TextMeshProUGUI>().text = effect.perkName;

            StartCoroutine(effect.Duration(PlayerManager.localPlayerInstance, perkText));
        }
    }
}
