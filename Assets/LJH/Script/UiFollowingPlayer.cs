using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiFollowingPlayer : MonoBehaviourPun
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    [SerializeField] TMP_Text nameTxt;
    [SerializeField] GameObject MasterIcon;
    [SerializeField] GameObject ReadyIcon;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient == true) 
        {
            photonView.RPC("RpciconActive", RpcTarget.AllBuffered, "Master", true);
        }


        nameTxt.text = PhotonNetwork.LocalPlayer.NickName;
        
    }
    private void Update()
    {
        Following();
    }

    public void setTarget(GameObject obj) 
    {
        target = obj.transform;
    }

    private void Following() 
    {
        if (target == null) 
        {
            return;
        }
        
        transform.position = target.position+offset;

        
    }
    public void Ready() 
    {
        photonView.RPC("RpciconActive", RpcTarget.AllBuffered, "Ready",true);

        // 레디를 받고 버튼을 누르면 보내는 기능은 어디에 해야하나?
    }

    //레디나 방장 아이콘 온 오프는 rpc로 해야 함 


    [PunRPC]

    private void RpciconActive(string name , bool isActive) 
    {
        if (name == "Ready")
        {
            ReadyIcon.SetActive(isActive);  
        }
        else if (name == "Master")
        {
            MasterIcon.SetActive(isActive);
        }
        else 
        {
            Debug.Log("바르지 않은 이름");
        }
    }
}
