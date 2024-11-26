using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VoteScenePlayerData : MonoBehaviourPun, IPunObservable
{
    [SerializeField] public Button _voteButton;
    public Button VoteButton { get { return _voteButton; }  set { _voteButton = value; } }


    [SerializeField] private bool _didVote; // 투표했는지 여부
    public bool DidVote { get { return _didVote; } set { _didVote = value; } }


    [SerializeField] private bool _isDead; // 죽었는지 여부
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }


    [SerializeField] private bool _isReporter; // 신고자 여부
    public bool IsReporter { get { return _isReporter; } set { _isReporter = value; } }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(DidVote);
            stream.SendNext(IsDead);
            stream.SendNext(IsReporter);
        }
        else if (stream.IsReading)
        {
            DidVote = (bool)stream.ReceiveNext();
            IsDead = (bool)stream.ReceiveNext();
            IsReporter = (bool)stream.ReceiveNext();
        }
    }
}
