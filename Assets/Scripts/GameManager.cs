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

    // ��ʂ��t�F�[�h�C��������R�[���`��
    IEnumerator PanelFadeIn()
    {
        Image fade = Panel.GetComponent<Image>();
        // �t�F�[�h���̐F��ݒ�i���j
        fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (255.0f / 255.0f));
        // �t�F�[�h�C���ɂ����鎞�ԁi�b�j
        const float fade_time = 1.0f;
        // ���[�v��
        const int loop_count = 10;
        // �E�F�C�g���ԎZ�o
        float wait_time = fade_time / loop_count;
        // �F�̊Ԋu���Z�o
        float alpha_interval = 255.0f / loop_count;

        // �F�����X�ɕς��郋�[�v
        for (float alpha = 255.0f; alpha >= 0.0f; alpha -= alpha_interval)
        {
            // �҂�����
            yield return new WaitForSeconds(wait_time);

            // Alpha�l��������������
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
        InstractorText.text = "���Z�J�n";
        while (true)
        {
            for (int i = 0; i < 255; i++)
            {
                InstractorText.color = InstractorText.color + new Color32(0, 0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    
    //�ǂޏ��Ԃ�ێ����郊�X�g
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
    //�J�[�h�ǂݏグ�̂��߂̏���
    private void ReadAloud()
    {
        string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int START = 0;
        int END = 26;
        int index = 0;
        //0����25�̐�����ێ����郊�X�g
        List<int> numbersList = new List<int>();
        
        //�ǂޏ��Ԃ����߂�
        for (int i = START; i < END; i++) numbersList.Add(i);
        for (int i = 26; 0 < i; i--)
        {
            //�����𐶐�����
            int rndindex = Random.Range(0, numbersList.Count);
            int randomNum = numbersList[rndindex];
            
            //�A���t�@�x�b�g���i�[����
            orderList[index] = alphabets[randomNum].ToString();
            //�������i�[����
            orderNumList[index]  = randomNum;
            //�d����h�����߂ɐ������ꂽ�����̓��X�g����폜����
            numbersList.RemoveAt(rndindex);
            index++;
        }
        
        string path = Application.dataPath + "/StreamingAssets/card.txt";
        string data = File.ReadAllText(path);
        // ���s��؂�ɂ���
        string[] lineSepDatas = data.Split('\n');
        // ,��؂�ɂ���
        string[] commaSepDatas = new string[26];
        for(int i = START; i < END; i++)
        {
            commaSepDatas[i] = lineSepDatas[i].Split(',')[0]; 
        }
        SendDatas();
    }

    //�J�[�h�̓ǂݏグ
    IEnumerator AudioCor(int[] numList, string[] orderList)
    {
        isSendDone = false;
        //Debug.Log(string.Join(" ", orderList));
        yield return new WaitForSeconds(5.0f);
        for (int i = 0; i < 26; i++)
        {
            nowQuestion = orderList[i];

            //2�񂸂ǂݏグ��
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
        /* //���ʂ�\������

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
