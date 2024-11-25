using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.Rendering;

[System.Serializable]
public class VoteSceneData : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int _skipCount; // 스킵한 사람 수
    public int SkipCount { get { return _skipCount; } set { _skipCount = value; } }


    [SerializeField] private float _reportTimeCount; // 신고자만 말할 수 있는 시간
    public float ReportTimeCount { get { return _reportTimeCount; } set { _reportTimeCount = value; } }


    [SerializeField] public float _voteTimeCount; // 투표 가능 시간
    public float VoteTimeCount { get { return _voteTimeCount; } set { _voteTimeCount = value; } }


    [SerializeField] private Transform _playerPanelParent; // 플레이어 패널 생성 시 부모 오브젝트
    public Transform PlayerPanelParent { get { return _playerPanelParent; } set { _playerPanelParent = value; } }   


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
           
            stream.SendNext(SkipCount);
            stream.SendNext(ReportTimeCount);
            stream.SendNext(VoteTimeCount);
           
            stream.SendNext(PlayerPanelParent);
        }
        else if (stream.IsReading)
        {
          
            SkipCount = (int)stream.ReceiveNext();
            ReportTimeCount = (int)stream.ReceiveNext();
            VoteTimeCount = (int)stream.ReceiveNext();
          
            PlayerPanelParent = (Transform)stream.ReceiveNext();
        }
    }
}
