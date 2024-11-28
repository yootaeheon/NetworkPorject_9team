using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestSceneRpc : MonoBehaviourPun
{

    private void Awake()
    {
        photonView.ViewID += 2;
    }

    private void Start()
    {
        GameObject game = PhotonNetwork.Instantiate("NSJ_Player",Vector3.zero, Quaternion.identity);

        StartCoroutine(testRoutine());
    }

    IEnumerator testRoutine()
    {
        while (true)
        {
            yield return 1f.GetDelay();
            photonView.RPC(nameof(Test), RpcTarget.All);
        } 
    }

    [PunRPC]
    private void Test()
    {
        Debug.Log("RPC Test");
    }
}
