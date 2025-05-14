using Photon.Pun;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveSpeedBuff", menuName = "Scriptable Objects/Perks/MoveSpeedBuff")]
public class MoveSpeedBuff : PerkEffect
{
    public float amount;

    public override void Apply(GameObject player)
    {
        PhotonView playerPV = player.GetComponent<PhotonView>();
        playerPV.RPC("ChangeSpeedBy", RpcTarget.All, amount);
    }

    public override IEnumerator Duration(GameObject player, GameObject perkTextInstance)
    {
        Debug.Log(perkName + " started countdown");
        yield return new WaitForSeconds(perkDuration);
        Debug.Log(perkName + " ran out of duration");

        PhotonView playerPV = player.GetComponent<PhotonView>();
        playerPV.RPC("ChangeSpeedBy", RpcTarget.All, -amount);

        Destroy(perkTextInstance);
    }
}
