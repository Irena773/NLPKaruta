using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Realtime;

public class Card : MonoBehaviourPunCallbacks
{
    private string nowQuestion;
    private GameManager GameManager;
    private TextMeshProUGUI TestText;
    private PhotonView _PhotonViewControl;
    PlayerScore PlayerScore;
    AudioSource audioSource;
    [SerializeField] AudioClip se;

    public void OnClickCard()
    {    
        if (this.gameObject.tag == nowQuestion)
        {
            _PhotonViewControl.RPC("DestroyCard", RpcTarget.All);
             //スコアを更新する
             PlayerScore.setplayer1Ans(1);
        }
        
    }

    [PunRPC]
    public void DestroyCard()
    {
        audioSource.PlayOneShot(se);
        Destroy(gameObject,1.0f);

    }

    public void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        PlayerScore = GameObject.Find("PlayerScore").GetComponent<PlayerScore>();
       
        audioSource = GetComponent<AudioSource>();
        _PhotonViewControl = GetComponent<PhotonView>();
    }


    public void Update()
    {
        nowQuestion = GameManager.getNowQuestion();
    }
}
