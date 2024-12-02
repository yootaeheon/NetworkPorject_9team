using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportingObject : MonoBehaviourPun
{

    public void Reporting()
    {
        Debug.Log("시체 찾는중");
        GameObject[] Corpse = GameObject.FindGameObjectsWithTag("Dead");

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
