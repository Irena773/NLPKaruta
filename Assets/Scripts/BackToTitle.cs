using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour
{
   
   public void Onclicked()
    {
        //PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        //PhotonNetwork.LeaveRoom();
        //PhotonNetwork.LoadLevel("Title");
        Application.Quit();
    }
}
