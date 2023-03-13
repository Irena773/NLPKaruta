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
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
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

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == MAXPLAYER)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                messege.text = "対戦相手が揃いました。";
                PhotonNetwork.LoadLevel("Karuta");
            }
        }
    }
    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == MAXPLAYER)
        {
            messege.text = "対戦相手が揃いました。";
        }
        else
        {
            messege.text = "対戦相手を待っています。";
        } 
    }
}
