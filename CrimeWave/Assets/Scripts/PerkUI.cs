using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour
{
    [SerializeField] private GameObject perkObject;
    public List<PerkEffectList> perkCategories = new List<PerkEffectList>(); // category of effects

    private List<GameObject> currentPerkRolls;
    private GameObject selectPerkPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectPerkPanel = transform.Find("SelectPerkPanel").gameObject;
    }

    public void RollRandomPerks()
    {
        //selectPerkPanel.SetActive(true);

        for(int i = 0; i < 3; i++) // 3 perks per roll, in theory we can have more perks per roll, the only problem is the ui scaling
        {
            GameObject newPerk = Instantiate(perkObject, transform.position, Quaternion.identity, selectPerkPanel.transform); // parent is selectperkpanel
            newPerk.GetComponentInChildren<Button>().onClick.AddListener(newPerk.GetComponent<Perk>().ActivatePerk); // add onclick listener 

            PerkEffectList randomCategory = perkCategories[Random.Range(0, perkCategories.Count)];
            PerkEffect randomEffectInCategory = randomCategory.list[Random.Range(0, randomCategory.list.Count)];
            newPerk.GetComponent<Perk>().effect = randomEffectInCategory;

            currentPerkRolls.Add(newPerk);
        }
    }

    public void PerkSelected()
    {
        foreach(GameObject perk in currentPerkRolls)
        {
            Destroy(perk);
        }
    }
}
