using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.Rendering;

[System.Serializable]
public class VoteData : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private int _voteCount; // 득표 수
    public int VoteCount { get { return _voteCount; } set { _voteCount = value; } }


    [SerializeField] private int _skipCount; // 스킵한 사람 수
    public int SkipCount { get { return _skipCount; } set { _skipCount = value; } }


    [SerializeField] private float _reportTimeCount; // 신고자만 말할 수 있는 시간
    public float ReportTimeCount { get { return _reportTimeCount; } set { _reportTimeCount = value; } }


    [SerializeField] public float _voteTimeCount; // 투표 가능 시간
    public float VoteTimeCount { get { return _voteTimeCount; } set { _voteTimeCount = value; } }


    [SerializeField] private bool _didVote; // 투표했는지 여부
    public bool DidVote { get { return _didVote; } set { _didVote = value; } }


    [SerializeField] private bool _isDead; // 죽었는지 여부
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }


    [SerializeField] private bool _isReporter;
    public bool IaReporter { get { return _isReporter; } set { _isReporter = value; } }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_voteCount);
            stream.SendNext(_skipCount);
            stream.SendNext(_reportTimeCount);
            stream.SendNext(_voteTimeCount);
            stream.SendNext(_didVote);
            stream.SendNext(_isDead);
            stream.SendNext(_isReporter);
        }
        else if (stream.IsReading)
        {
            _voteCount = (int)stream.ReceiveNext();
            _skipCount = (int)stream.ReceiveNext();
            _reportTimeCount = (int)stream.ReceiveNext();
            _voteTimeCount = (int)stream.ReceiveNext();
            _didVote = (bool)stream.ReceiveNext();
            _isDead = (bool)stream.ReceiveNext();
            _isReporter = (bool)stream.ReceiveNext();
        }
    }
}
