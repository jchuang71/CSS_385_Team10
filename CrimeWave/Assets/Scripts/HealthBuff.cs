using Photon.Pun;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthBuff", menuName = "Scriptable Objects/Perks/HealthBuff")]
public class HealthBuff : PerkEffect
{
    public float amount;

    public override void Apply(GameObject player)
    {
        PhotonView playerPV = player.GetComponent<PhotonView>();
        playerPV.RPC("ChangeMaxHealthBy", RpcTarget.All, amount);
        playerPV.RPC("ChangeHealthBy", RpcTarget.All, amount); // also heal the target, can be removed
    }

    public override IEnumerator Duration(GameObject player, GameObject perkTextInstance)
    {
        Debug.Log(perkName + " started countdown");
        yield return new WaitForSeconds(perkDuration);
        Debug.Log(perkName + " ran out of duration");

        PhotonView playerPV = player.GetComponent<PhotonView>();
        playerPV.RPC("ChangeMaxHealthBy", RpcTarget.All, -amount);

        Destroy(perkTextInstance);
    }
}
