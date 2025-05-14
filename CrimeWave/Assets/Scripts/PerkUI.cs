using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour
{
    public List<PerkEffectList> perkCategories = new List<PerkEffectList>(); // category of effects

    [SerializeField] private GameObject perkObject;
    private List<GameObject> currentPerkRolls = new List<GameObject>();
    private GameObject selectPerkPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectPerkPanel = transform.Find("SelectPerkPanel").gameObject;
    }

    public void RollRandomPerks()
    {
        ClearPerkRolls();
        selectPerkPanel.SetActive(true);

        for(int i = 0; i < 3; i++) // 3 perks per roll, in theory we can have more perks per roll, the only problem is the ui scaling
        {
            Debug.Log("count aaa");
            GameObject newPerk = Instantiate(perkObject, transform.position, Quaternion.identity, selectPerkPanel.transform); // parent is selectperkpanel
            newPerk.GetComponentInChildren<Button>().onClick.AddListener(newPerk.GetComponent<Perk>().ActivatePerk); // add onclick listener 

            PerkEffectList randomCategory = perkCategories[Random.Range(0, perkCategories.Count)];
            PerkEffect randomEffectInCategory = randomCategory.list[Random.Range(0, randomCategory.list.Count)];
            newPerk.GetComponent<Perk>().effect = randomEffectInCategory;

            currentPerkRolls.Add(newPerk);
        }
    }

    private void ClearPerkRolls()
    {
        foreach (GameObject perk in currentPerkRolls)
        {
            Destroy(perk);
        }

        currentPerkRolls.Clear();
    }
}
