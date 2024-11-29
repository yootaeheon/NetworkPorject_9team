using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class RPCUtil : MonoBehaviourPun
{
    public static RPCUtil Instance;
    private void Awake()
    {
        InitSingleTon();
    }

    public static void SetParent(Transform target, Transform parent)
    {
        //int targetId = target
    }

    [PunRPC]
    private void RPCSetParent(int targetID, int parentID)
    {

    }



    private void InitSingleTon()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
