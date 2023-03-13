using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dealer : MonoBehaviourPunCallbacks
{
    //�J�[�h�̃v���n�u
    [SerializeField] GameObject CardPrefab;
    [SerializeField] GameObject Player1Side;
    [SerializeField] GameObject Player2Side;
   

    private string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private List<int> numbersList = new List<int>();
    const int START = 0;
    const int END = 26;
    //�v���C���[���ɕ\��������J�[�h�̖��O��ێ����郊�X�g
    private char[] player1Char = new char[13];
    //CPU���ɕ\��������J�[�h�̖��O��ێ����郊�X�g
    private char[] player2Char = new char[13];

    private PhotonView _PhotonViewControl;
    
    public void SendDatas()
    {
        //���M���\�b�h
        string player1Str = new string(player1Char);
        player1Str = player1Str.Replace(" ", "");
        string player2Str = new string(player2Char);
        player2Str = player2Str.Replace(" ", "");
        _PhotonViewControl.RPC("SendPlayer1Card", RpcTarget.All, player1Str);
        _PhotonViewControl.RPC("SendPlayer2Card", RpcTarget.All, player2Str);
    }

    [PunRPC]
    private void SendPlayer1Card(string SendedData)
    {
        player1Char = SendedData.ToCharArray();
        //���肩�痈���f�[�^�������̎󂯎M�ɏ㏑���ۑ�
        //TestText.text = string.Join(" ", player1Char);
        if (!PhotonNetwork.IsMasterClient)
        {
            Image cardImage;
            Sprite sprite;
            GameObject playerCard;
            PhotonView photonview;
            for (int i = 0; i < 13; i++)
            {   //player1���̃J�[�h�𐶐�����
                playerCard = Instantiate(CardPrefab);
                //playerCard = PhotonNetwork.Instantiate("CardPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
                playerCard.transform.SetParent(GameObject.Find("Player2Side").GetComponent<Transform>(), false);
                playerCard.name = "Player2Card" + i.ToString();
                photonview = playerCard.GetComponent<PhotonView>();
                photonview.ViewID = 3000 + i;
                //tag������
                string player1Str = new string(player1Char);
                
                playerCard.tag = player1Char[i].ToString();
                cardImage = playerCard.GetComponent<Image>();
                //�摜��ǂݍ���
                sprite = Resources.Load<Sprite>("Images/" + player1Char[i]);
                //Sprite��\��t����
                cardImage.sprite = sprite;
            }
        }
    }
    [PunRPC]//RPC�ŌĂяo���������\�b�h
    private void SendPlayer2Card(string SendedData)
    {
        //���肪�����Ă����Ƃ��Ɏ����I�ɔ���
        player2Char = SendedData.ToCharArray();
        //���肩�痈���f�[�^�������̎󂯎M�ɏ㏑���ۑ�
        //TestText.text = string.Join(" ", player2Char);
        if (!PhotonNetwork.IsMasterClient)
        {
            Image cardImage;
            Sprite sprite;
            GameObject Player2Card;
            PhotonView photonview;
            for (int i = 0; i < 13; i++)
            {
                //Player2���̃J�[�h�𐶐�����
                Player2Card = Instantiate(CardPrefab);
                //Player2Card = PhotonNetwork.Instantiate("CardPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
                Player2Card.transform.SetParent(GameObject.Find("Player1Side").GetComponent<Transform>(), false);
                Player2Card.name = "PlayerCard" + i;
                photonview = Player2Card.GetComponent<PhotonView>();
                photonview.ViewID = 4000 + i;
                //tag������
                Player2Card.tag = player2Char[i].ToString();
                cardImage = Player2Card.GetComponent<Image>();
                //�摜��ǂݍ���
                sprite = Resources.Load<Sprite>("Images/" + player2Char[i]);
                //Sprite��\��t����
                cardImage.sprite = sprite;
            }
        }
    }

    void Start()
    {
        _PhotonViewControl = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            
            for (int i = START; i < END; i++) numbersList.Add(i);
            int index = 0;
            //0~25�̐����̒����烉���_����13�̐����𐶐�����
            for (int count = 13; 0 < count; count--)
            {
                //�����𐶐�����
                int rndindex = Random.Range(0, numbersList.Count);
                int randomNum = numbersList[rndindex];
                //player1���̃A���t�@�x�b�g���i�[����
                player1Char[index] = alphabets[randomNum];
                //�d����h�����߂ɐ������ꂽ�����̓��X�g����폜����
                numbersList.RemoveAt(rndindex);
                index++;
            }

            //player2���̃A���t�@�x�b�g���i�[����
            for (int i = 0; i < 13; i++) player2Char[i] = alphabets[numbersList[i]];
            Image cardImage;
            Sprite sprite;
            GameObject playerCard;
            GameObject Player2Card;
            PhotonView photonview;
            for (int i = 0; i < 13; i++)
            {   //player1�[���̃J�[�h�𐶐�����

                //playerCard = PhotonNetwork.Instantiate("CardPrefab", new Vector3(0,0,0), Quaternion.identity, 0);
                playerCard = Instantiate(CardPrefab); 
                playerCard.transform.SetParent(GameObject.Find("Player1Side").GetComponent<Transform>(), false);
                playerCard.name = "PlayerCard" + i.ToString();
                //tag������
                string player1Str = new string(player1Char);
                photonview = playerCard.GetComponent<PhotonView>();
                photonview.ViewID = 3000 + i;
                playerCard.tag = player1Char[i].ToString();
                cardImage = playerCard.GetComponent<Image>();
                //�摜��ǂݍ���
                sprite = Resources.Load<Sprite>("Images/" + player1Char[i]);
                //Sprite��\��t����
                cardImage.sprite = sprite;

                //player2���̃J�[�h�𐶐�����
                Player2Card = Instantiate(CardPrefab);
                //Player2Card = PhotonNetwork.Instantiate("CardPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
                Player2Card.transform.SetParent(GameObject.Find("Player2Side").GetComponent<Transform>(), false);
                Player2Card.name = "Player2Card" + i;
                photonview = Player2Card.GetComponent<PhotonView>();
                photonview.ViewID = 4000 + i;
                //tag������
                Player2Card.tag = player2Char[i].ToString();
                cardImage = Player2Card.GetComponent<Image>();
                //�摜��ǂݍ���
                sprite = Resources.Load<Sprite>("Images/" + player2Char[i]);
                //Sprite��\��t����
                cardImage.sprite = sprite;
            }
            SendDatas();
        }
        
    }
}
