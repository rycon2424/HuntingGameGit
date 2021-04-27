using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;

public class LoadingScreen : MonoBehaviour
{
    public List<LoadingScreenInfo> loadingInfo = new List<LoadingScreenInfo>();
    public int loadedPlayers;

    void Start()
    {
        PhotonNetwork.Instantiate(Path.Combine("Utility", "LoadingInstance"), Vector3.zero, Quaternion.identity).GetComponent<LoadingInstance>().loadingPlace = (PhotonNetwork.LocalPlayer.ActorNumber - 1);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            loadingInfo[i].gmInfo.SetActive(true);
            loadingInfo[i].playerName.text = PhotonNetwork.PlayerList[i].NickName;
            loadingInfo[i].playerProgress.text = "0%";
        }
    }

    [System.Serializable]
    public class LoadingScreenInfo
    {
        public Text playerName;
        public Text playerProgress;
        public GameObject gmInfo;
    }
}
