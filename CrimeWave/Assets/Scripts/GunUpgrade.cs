using UnityEngine;
using System.Collections;
using Photon.Pun;

[CreateAssetMenu(fileName = "GunUpgrade", menuName = "Scriptable Objects/Perks/GunUpgrade")]
public class GunUpgrade : PerkEffect
{
    public Gun gun;
    public float rarity;
    public int rarityLevel;
    public Color rarityColor;

    public override void Apply(GameObject player)
    {
        int selectorIndex = gun.gunSelectorIndex;
        PlayerGun playerGun = player.GetComponent<PlayerGun>();
        if (playerGun != null)
        {
            playerGun.guns[selectorIndex] = gun; // Update the gun in the player's gun list
        }
    }

    public override IEnumerator Duration(GameObject player, GameObject perkTextInstance)
    {
        return null; // No duration for gun upgrades
    }
}
