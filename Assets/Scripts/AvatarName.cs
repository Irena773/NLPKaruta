using Photon.Pun;
using TMPro;

public class AvatarName : MonoBehaviourPunCallbacks
{
    void Start()
    {
        TextMeshPro nameLabel = GetComponent<TextMeshPro>();
        // プレイヤー名とプレイヤーIDを表示する
        nameLabel.text = photonView.Owner.NickName+"(" + photonView.OwnerActorNr +")";
    }

}
