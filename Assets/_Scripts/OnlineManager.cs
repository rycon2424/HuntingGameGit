using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class OnlineManager : MonoBehaviour
{
    void Start()
    {
        PhotonNetwork.Instantiate(Path.Combine("Player", "OnlinePlayer"), Vector3.zero, Quaternion.identity);
    }
}
