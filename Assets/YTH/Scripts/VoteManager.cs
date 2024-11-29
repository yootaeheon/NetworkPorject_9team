using Photon.Pun;
using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class VoteManager : MonoBehaviourPunCallbacks
{
    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VotePanel _votePanel;

    [SerializeField] VoteScenePlayerData[] _playerData;

    public int[] _voteCounts; // 각 플레이어의(ActorNumber와 연결된 인덱스 번호)의 득표수를 배열로 저장

    [SerializeField] Button[] _voteButtons;

    // IsDead == false && DidVote == false 일때만 스킵 가능하게 조건 추가
    public void Vote(int index) // 플레이어 패널을 눌러 투표
    {
        photonView.RPC("VotePlayerRPC", RpcTarget.All, index);
        _votePanel.DisableButton();
    }

    [PunRPC]
    public void VotePlayerRPC(int index)
    {
        _voteCounts[index]++;
        Debug.Log($"{index}번 플레이어 득표수 {_voteCounts[index]} ");
    }

    // IsDead == false && DidVote == false 일때만 스킵 가능하게 조건 추가
    public void OnClickSkip() // 스킵 버튼 누를 시
    {
        photonView.RPC("OnClickSkipRPC", RpcTarget.AllBuffered);
        _votePanel.DisableButton();
    }

    [PunRPC]
    public void OnClickSkipRPC()
    {
        _voteData.SkipCount++;
        Debug.Log($" 스킵 수 : {_voteData.SkipCount}");
    }

    // 투표 종료 후 집계 기능
    public void GetVoteResult()
    {
        // 최다 득표자 찾는 기능
        int top = -1;
        int top2 = -1;
        int playerIndex = -1;

        for (int i = 0; i < 12; i++)
        {
            if (_voteCounts[i] > top)
            {
                top = _voteCounts[i];
                playerIndex = i;  
            }
            else if (_voteCounts[i] == top)
            {
                top2 = _voteCounts[i];
                Debug.Log("동점표로 없던 일~");
                return;
            }
        }
        Debug.Log($"{_voteData.SkipCount}표 기권!");
        Debug.Log($"{playerIndex}번 플레이어 당선 {top}표 : 추방됩니다");
        //TODO : 고스트가 되는 기능
        return;
    }
}
