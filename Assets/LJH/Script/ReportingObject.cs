using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ReportingObject : MonoBehaviourPun
{
    public Color CorpseColor;

    private void Start()
    {
        CorpseColor=PlayerDataContainer.Instance.GetPlayerData(photonView.Owner.GetPlayerNumber()).PlayerColor;
    }

    public void Reporting()
    {
        GameObject[] Corpse = GameObject.FindGameObjectsWithTag("Dead");
        Debug.Log(Corpse.Length);
        for (int i = 0; i < Corpse.Length; i++)
        {   
            PhotonView targetView = Corpse[i].GetComponent<PhotonView>();
            if (targetView.IsMine == true)
            {
                targetView.RPC("RpcUnActive", RpcTarget.All);
            }
            else if (targetView.IsMine == false) 
            {
                targetView.TransferOwnership(PhotonNetwork.LocalPlayer);
                
                targetView.RPC("RpcUnActive", RpcTarget.All);
            }            
        }
        
    }

    [PunRPC]
    private void RpcUnActive()
    {
       gameObject.SetActive(false);
    }
}
