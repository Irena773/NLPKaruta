using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dealer : MonoBehaviourPunCallbacks
{
    //カードのプレハブ
    [SerializeField] GameObject CardPrefab;
    [SerializeField] GameObject Player1Side;
    [SerializeField] GameObject Player2Side;
   

    private string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private List<int> numbersList = new List<int>();
    const int START = 0;
    const int END = 26;
    //プレイヤー側に表示させるカードの名前を保持するリスト
    private char[] player1Char = new char[13];
    //CPU側に表示させるカードの名前を保持するリスト
    private char[] player2Char = new char[13];

    private PhotonView _PhotonViewControl;
    
    public void SendDatas()
    {
        //送信メソッド
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
        //相手から来たデータを自分の受け皿に上書き保存
        //TestText.text = string.Join(" ", player1Char);
        if (!PhotonNetwork.IsMasterClient)
        {
            Image cardImage;
            Sprite sprite;
            GameObject playerCard;
            PhotonView photonview;
            for (int i = 0; i < 13; i++)
            {   //player1側のカードを生成する
                playerCard = Instantiate(CardPrefab);
                //playerCard = PhotonNetwork.Instantiate("CardPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
                playerCard.transform.SetParent(GameObject.Find("Player2Side").GetComponent<Transform>(), false);
                playerCard.name = "Player2Card" + i.ToString();
                photonview = playerCard.GetComponent<PhotonView>();
                photonview.ViewID = 3000 + i;
                //tagをつける
                string player1Str = new string(player1Char);
                
                playerCard.tag = player1Char[i].ToString();
                cardImage = playerCard.GetComponent<Image>();
                //画像を読み込む
                sprite = Resources.Load<Sprite>("Images/" + player1Char[i]);
                //Spriteを貼り付ける
                cardImage.sprite = sprite;
            }
        }
    }
    [PunRPC]//RPCで呼び出したいメソッド
    private void SendPlayer2Card(string SendedData)
    {
        //相手が送ってきたときに自動的に発動
        player2Char = SendedData.ToCharArray();
        //相手から来たデータを自分の受け皿に上書き保存
        //TestText.text = string.Join(" ", player2Char);
        if (!PhotonNetwork.IsMasterClient)
        {
            Image cardImage;
            Sprite sprite;
            GameObject Player2Card;
            PhotonView photonview;
            for (int i = 0; i < 13; i++)
            {
                //Player2側のカードを生成する
                Player2Card = Instantiate(CardPrefab);
                //Player2Card = PhotonNetwork.Instantiate("CardPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
                Player2Card.transform.SetParent(GameObject.Find("Player1Side").GetComponent<Transform>(), false);
                Player2Card.name = "PlayerCard" + i;
                photonview = Player2Card.GetComponent<PhotonView>();
                photonview.ViewID = 4000 + i;
                //tagをつける
                Player2Card.tag = player2Char[i].ToString();
                cardImage = Player2Card.GetComponent<Image>();
                //画像を読み込む
                sprite = Resources.Load<Sprite>("Images/" + player2Char[i]);
                //Spriteを貼り付ける
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
            //0~25の数字の中からランダムで13個の数字を生成する
            for (int count = 13; 0 < count; count--)
            {
                //乱数を生成する
                int rndindex = Random.Range(0, numbersList.Count);
                int randomNum = numbersList[rndindex];
                //player1側のアルファベットを格納する
                player1Char[index] = alphabets[randomNum];
                //重複を防ぐために生成された数字はリストから削除する
                numbersList.RemoveAt(rndindex);
                index++;
            }

            //player2側のアルファベットを格納する
            for (int i = 0; i < 13; i++) player2Char[i] = alphabets[numbersList[i]];
            Image cardImage;
            Sprite sprite;
            GameObject playerCard;
            GameObject Player2Card;
            PhotonView photonview;
            for (int i = 0; i < 13; i++)
            {   //player1ー側のカードを生成する

                //playerCard = PhotonNetwork.Instantiate("CardPrefab", new Vector3(0,0,0), Quaternion.identity, 0);
                playerCard = Instantiate(CardPrefab); 
                playerCard.transform.SetParent(GameObject.Find("Player1Side").GetComponent<Transform>(), false);
                playerCard.name = "PlayerCard" + i.ToString();
                //tagをつける
                string player1Str = new string(player1Char);
                photonview = playerCard.GetComponent<PhotonView>();
                photonview.ViewID = 3000 + i;
                playerCard.tag = player1Char[i].ToString();
                cardImage = playerCard.GetComponent<Image>();
                //画像を読み込む
                sprite = Resources.Load<Sprite>("Images/" + player1Char[i]);
                //Spriteを貼り付ける
                cardImage.sprite = sprite;

                //player2側のカードを生成する
                Player2Card = Instantiate(CardPrefab);
                //Player2Card = PhotonNetwork.Instantiate("CardPrefab", new Vector3(0, 0, 0), Quaternion.identity, 0);
                Player2Card.transform.SetParent(GameObject.Find("Player2Side").GetComponent<Transform>(), false);
                Player2Card.name = "Player2Card" + i;
                photonview = Player2Card.GetComponent<PhotonView>();
                photonview.ViewID = 4000 + i;
                //tagをつける
                Player2Card.tag = player2Char[i].ToString();
                cardImage = Player2Card.GetComponent<Image>();
                //画像を読み込む
                sprite = Resources.Load<Sprite>("Images/" + player2Char[i]);
                //Spriteを貼り付ける
                cardImage.sprite = sprite;
            }
            SendDatas();
        }
        
    }
}
