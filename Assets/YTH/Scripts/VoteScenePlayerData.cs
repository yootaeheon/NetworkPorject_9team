using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VoteScenePlayerData : MonoBehaviourPun, IPunObservable
{
    [SerializeField] public Button voteButton;

    [SerializeField] private int _voteCount; // 득표 수
    public int VoteCount { get { return _voteCount; } set { _voteCount = value; } }


    [SerializeField] private bool _didVote; // 투표했는지 여부
    public bool DidVote { get { return _didVote; } set { _didVote = value; } }


    [SerializeField] private bool _isDead; // 죽었는지 여부
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }


    [SerializeField] private bool _isReporter; // 신고자인지 여부
    public bool IsReporter { get { return _isReporter; } set { _isReporter = value; } }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(VoteCount);
            stream.SendNext(DidVote);
            stream.SendNext(IsDead);
            stream.SendNext(IsReporter);
        }
        else if (stream.IsReading)
        {
            VoteCount = (int)stream.ReceiveNext();
            DidVote = (bool)stream.ReceiveNext();
            IsDead = (bool)stream.ReceiveNext();
            IsReporter = (bool)stream.ReceiveNext();
        }
    }
}
