using Photon.Pun;
using System;
using System.Diagnostics;
using TMPro;

public class PlayerScore : MonoBehaviourPunCallbacks, IPunObservable
{
    private int playerScore;
    TextMeshPro scoreLabel;
    public int getplayer1Ans() { return playerScore; }
    public void setplayer1Ans(int value)
    {
            playerScore += value;
            scoreLabel.text = playerScore.ToString();
    }

    void Start()
    {
        playerScore = 0;
        scoreLabel = GetComponent<TextMeshPro>();
    }

    void Update()
    {
        scoreLabel.text = playerScore.ToString();
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���g�̃A�o�^�[�̃X�^�~�i�𑗐M����
            stream.SendNext(playerScore);
            
        }
        else
        {
            // ���v���C���[�̃A�o�^�[�̃X�^�~�i����M����
            playerScore = (int)stream.ReceiveNext();
            
        }
    }
}

