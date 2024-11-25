using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : MonoBehaviourPunCallbacks
{
    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VoteScenePlayerData _playerData;

    [SerializeField] VotePanel _votePanel;

    public int[] voteCounts; // 각 플레이어의 득표수를 배열로 저장

    public void OnClickPlayerPanel(int index) // 플레이어 패널을 눌러 투표
    {
        photonView.RPC("VotePlayerRPC", RpcTarget.All, index);
        //_playerData.DidVote =true;
    }

    [PunRPC]
    public void VotePlayerRPC (int index)
    {
        voteCounts[index]++;
        Debug.Log(index);
    }

    public void OnClickSkip() // 스킵 버튼 누를 시
    {
        _voteData.SkipCount++;

      // if (photonView.IsMine == false)
      //     return;
      // _playerData.DidVote = true;
    }



}
