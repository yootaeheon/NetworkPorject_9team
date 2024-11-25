using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class VoteCustomProperty
{
    // 모든 플레이어에게 ["VotedCount"]를 키 값으로 가지는 커스텀프로퍼티 생성
    public static void InitCustomProperties(this Player[] playerList)
    {
        foreach (Player player in  PhotonNetwork.PlayerList)
        {
            PhotonHashtable properties = new PhotonHashtable();
            properties["VotedCount"] = 0;
            player.SetCustomProperties(properties);
        }
    }

    // 플레이어 패널을 눌렀을 때 호출되는 커스텀프로퍼티 함수
    // ["VotedCount"] 에 변화가 생기면 targetPlayer의 curVotedNum + 1 (현재 득표 수 + 1)
    public static void VotePlayer(this Player targetPlayer)
    {
        int curVotedNum = targetPlayer.CustomProperties.ContainsKey("VotedNum") ? (int)targetPlayer.CustomProperties["VotecdNum"] : 0;

        PhotonHashtable properties = new PhotonHashtable();
        properties["VotedNum"] = curVotedNum + 1;
        targetPlayer.SetCustomProperties(properties);

        Debug.Log($"{targetPlayer.NickName} 의 득표 수 : {curVotedNum + 1} ");
    }

    //votepanel 로 이동 할 것
    // 투표 결과를 알려주는 함수
    // 모든 플레이어의 ["VotedCount"] 의 변화 결과를 알려줌
    public static void GetVoteResult()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            int votedResults = player.CustomProperties.ContainsKey("VoteCount") ? (int)player.CustomProperties["VotedNum"] : 0;
            Debug.Log($"{player.NickName} 의 총 득표 수 : {votedResults}");
            // votedResults 만큼 targetPlayer의 패널에 익명 이미지 생성
        }

        // 득표수가 가장 많은 플레이어 당선
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            //TODO : 반복문 돌려서 찾아내기
        }
    }

  //  if (properties.ContainsKey(CustomProperty.READY))
  //      {
  //          UpdatePlayers();
} //

