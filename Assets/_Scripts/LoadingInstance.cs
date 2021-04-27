using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingInstance : MonoBehaviour
{
    public PhotonView pv;
    [Space]
    public LoadingScreen ls;
    public int loadingPlace;

    private bool doneLoading;

    AsyncOperation asyncLoad;

    void Start()
    {
        ls = FindObjectOfType<LoadingScreen>();
        asyncLoad = SceneManager.LoadSceneAsync(2);
        if (pv.IsMine)
        {
            pv.RPC("SyncLoadingID", RpcTarget.All, loadingPlace);
            StartCoroutine(Loading());
        }
    }
    
    IEnumerator Loading()
    {
        asyncLoad.allowSceneActivation = false;
        yield return new WaitForSeconds(1f);
        while (!asyncLoad.isDone)
        {
            pv.RPC("ShareProgress", RpcTarget.All, (asyncLoad.progress * 100));
            if (asyncLoad.progress >= 0.9f)
            {
                pv.RPC("ShareProgress", RpcTarget.All, 100f);
                if (doneLoading == false)
                {
                    doneLoading = true;
                    yield return new WaitForSeconds(2f);
                    pv.RPC("DoneLoading", RpcTarget.All);
                    yield return new WaitForSeconds(4f);
                }
                yield return new WaitForSeconds(1f);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    [PunRPC]
    void DoneLoading()
    {
        Debug.Log("doneLoading");
        ls.loadedPlayers += 1;
        if (ls.loadedPlayers == PhotonNetwork.PlayerList.Length)
        {
            pv.RPC("AllowLoading", RpcTarget.All);
        }
    }

    [PunRPC]
    void AllowLoading()
    {
        Debug.Log("Allow Scene activation");
        asyncLoad.allowSceneActivation = true;
    }

    [PunRPC]
    void SyncLoadingID(int lp)
    {
        loadingPlace = lp;
    }

    [PunRPC]
    void ShareProgress(float progress)
    {
        ls.loadingInfo[loadingPlace].playerProgress.text = progress.ToString() + "%";
    }

}
