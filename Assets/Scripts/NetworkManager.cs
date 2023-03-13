using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI messege;

    private const int MAXPLAYER = 2;

    public void DualMode()
    {
        PhotonNetwork.NickName = "Player";
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();

    }
    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MAXPLAYER)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                messege.text = "�ΐ푊�肪�����܂����B";
                PhotonNetwork.LoadLevel("Karuta");
            }
        }
    }
    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MAXPLAYER)
        {
            messege.text = "�ΐ푊�肪�����܂����B";
        }
        else
        {
            messege.text = "�ΐ푊���҂��Ă��܂��B";
        } 
    }
}
