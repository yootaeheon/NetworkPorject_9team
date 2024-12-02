using Photon.Pun;
using Photon.Pun.UtilityScripts;
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
        name = PhotonNetwork.LocalPlayer.NickName; // 바꿔야 할듯? 

        if (PhotonNetwork.IsMasterClient == true) 
        {
            if(photonView.IsMine == true)
                photonView.RPC("RpciconActive", RpcTarget.AllBuffered, "Master", true);
        }
        if (photonView.IsMine == true)
        {
            photonView.RPC("RpcSetNicknamePanel", RpcTarget.AllBuffered, name);
            gameObject.AddComponent<TestNamePanelHide>();
        }
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
    PlayerType[] playerTypes;
    private void NameToRed()
    {
        for (int i = 0; i < PlayerDataContainer.Instance.playerDataArray.Length; i++)
        {
            playerTypes[i] = PlayerDataContainer.Instance.GetPlayerJob(i);
        }

        if (PlayerDataContainer.Instance.GetPlayerJob(PhotonNetwork.LocalPlayer.GetPlayerNumber()) == PlayerType.Duck)
        {
            //
        }
    }

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

    [PunRPC]

    private void RpcSetNicknamePanel(string name) 
    {
        
            nameTxt.text =name;
    }
}
