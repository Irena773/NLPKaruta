using Photon.Pun;
using TMPro;

public class AvatarName : MonoBehaviourPunCallbacks
{
    void Start()
    {
        TextMeshPro nameLabel = GetComponent<TextMeshPro>();
        // �v���C���[���ƃv���C���[ID��\������
        nameLabel.text = photonView.Owner.NickName+"(" + photonView.OwnerActorNr +")";
    }

}
