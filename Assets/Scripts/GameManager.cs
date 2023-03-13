using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Instractor;
    [SerializeField] GameObject Panel;
    [SerializeField] AudioClip[] audioClip;
    [SerializeField] GameObject Result;

    private TextMeshProUGUI InstractorText;
    private  AudioSource audioSource;
    private string nowQuestion;
    public string getNowQuestion() { return nowQuestion; }
    public string[] getOrderList() { return orderList; }
    private PhotonView _PhotonViewControl;
    private bool isSendDone;

    void Start()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            var position = new Vector3(-42.5f, 3f);
            PhotonNetwork.Instantiate("PlayerPrefab", position, Quaternion.identity);
        }
        else
        {
            var position = new Vector3(-42.5f, -3f);
            PhotonNetwork.Instantiate("PlayerPrefab", position, Quaternion.identity);
        }
       
        InstractorText = Instractor.GetComponent<TextMeshProUGUI>();
        InstractorText.text = "";
        InstractorText.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        audioSource = gameObject.GetComponent<AudioSource>();
        _PhotonViewControl = GetComponent<PhotonView>();
        isSendDone = false;
        Result.SetActive(false);
        StartCoroutine("PanelFadeIn");
        if (PhotonNetwork.IsMasterClient)
        {
            ReadAloud();
            isSendDone = true;
        }
               
    }
    private void Update()
    {
        if(isSendDone == true) StartCoroutine(AudioCor(orderNumList, orderList));
        //Debug.Log(string.Join(" ", orderList));
    }

    // 画面をフェードインさせるコールチン
    IEnumerator PanelFadeIn()
    {
        Image fade = Panel.GetComponent<Image>();
        // フェード元の色を設定（黒）
        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (255.0f / 255.0f));
        // フェードインにかかる時間（秒）
        const float fade_time = 1.0f;
        // ループ回数
        const int loop_count = 10;
        // ウェイト時間算出
        float wait_time = fade_time / loop_count;
        // 色の間隔を算出
        float alpha_interval = 255.0f / loop_count;

        // 色を徐々に変えるループ
        for (float alpha = 255.0f; alpha >= 0.0f; alpha -= alpha_interval)
        {
            // 待ち時間
            yield return new WaitForSeconds(wait_time);

            // Alpha値を少しずつ下げる
            Color new_color = fade.color;
            new_color.a = alpha / 255.0f;
            fade.color = new_color;
        }
        StartCoroutine("TextFadeIn");
        yield return new WaitForSeconds(3.0f);
        Instractor.SetActive(false);
    }
    
    IEnumerator TextFadeIn()
    {
        InstractorText.text = "競技開始";
        while (true)
        {
            for (int i = 0; i < 255; i++)
            {
                InstractorText.color = InstractorText.color + new Color32(0, 0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    
    //読む順番を保持するリスト
    private string[] orderList = new string[26];
    private int[] orderNumList = new int[26];

    public void SendDatas()
    {
        _PhotonViewControl.RPC("SendOrderList", RpcTarget.All, orderList);
        _PhotonViewControl.RPC("SendOrderNumList", RpcTarget.All, orderNumList);
        _PhotonViewControl.RPC("SendDone", RpcTarget.All, true);
    }
    
    [PunRPC]
    private void SendOrderList(string[] SendedData)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            orderList = SendedData;
            Debug.Log(string.Join(" ", orderList));
        }
    }

    [PunRPC]
    private void SendOrderNumList(int[] SendedData)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            orderNumList = SendedData;
        }
    }

    [PunRPC]
    private void SendDone(bool flag)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
           isSendDone = flag;
        }
    }
    //カード読み上げのための準備
    private void ReadAloud()
    {
        string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int START = 0;
        int END = 26;
        int index = 0;
        //0から25の数字を保持するリスト
        List<int> numbersList = new List<int>();
        
        //読む順番を決める
        for (int i = START; i < END; i++) numbersList.Add(i);
        for (int i = 26; 0 < i; i--)
        {
            //乱数を生成する
            int rndindex = Random.Range(0, numbersList.Count);
            int randomNum = numbersList[rndindex];
            
            //アルファベットを格納する
            orderList[index] = alphabets[randomNum].ToString();
            //数字を格納する
            orderNumList[index]  = randomNum;
            //重複を防ぐために生成された数字はリストから削除する
            numbersList.RemoveAt(rndindex);
            index++;
        }
        
        string path = Application.dataPath + "/StreamingAssets/card.txt";
        string data = File.ReadAllText(path);
        // 改行区切りにする
        string[] lineSepDatas = data.Split('\n');
        // ,区切りにする
        string[] commaSepDatas = new string[26];
        for(int i = START; i < END; i++)
        {
            commaSepDatas[i] = lineSepDatas[i].Split(',')[0]; 
        }
        SendDatas();
    }

    //カードの読み上げ
    IEnumerator AudioCor(int[] numList, string[] orderList)
    {
        isSendDone = false;
        //Debug.Log(string.Join(" ", orderList));
        yield return new WaitForSeconds(5.0f);
        for (int i = 0; i < 26; i++)
        {
            nowQuestion = orderList[i];

            //2回ずつ読み上げる
            for (int j = 0; j < 2; j++)
            {
                audioSource.clip = audioClip[numList[i]];
                audioSource.Play();
                yield return new WaitWhile(() => audioSource.isPlaying);
                yield return new WaitForSeconds(2.0f);
            }
            yield return new WaitForSeconds(2.0f);
        }
        Result.SetActive(true);
        /* //結果を表示する

         if (PhotonNetwork.IsMasterClient)
         {
             GameObject playerPrefab = GameObject.Find("PlayerPrefab(Clone)");
             playerPrefab.transform.SetParent(GameObject.Find("ResultCanvas").GetComponent<Transform>(), false);
             playerPrefab.transform.position = new Vector3(0f,0f, 0f);
         }
         else
         {
             GameObject playerPrefab = GameObject.Find("PlayerPrefab(Clone)");
             playerPrefab.transform.SetParent(GameObject.Find("ResultCanvas").GetComponent<Transform>(), false);
             playerPrefab.transform.position = new Vector3(0f, 0f, 0f);
         }*/

    }
}
