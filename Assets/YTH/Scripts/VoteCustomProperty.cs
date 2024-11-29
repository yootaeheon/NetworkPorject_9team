using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class VoteCustomProperty
{
   private static VoteScenePlayerData _playerData;

   private const string VOTEDCOUNT = "VotedCount";


   // 모든 플레이어에게 ["VotedCount"]를 키 값으로 가지는 커스텀프로퍼티 생성
   public static void InitCustomProperties(this Player[] playerList)
   {
       foreach (Player player in  PhotonNetwork.PlayerList)
       {
           PhotonHashtable properties = new PhotonHashtable();
           properties[VOTEDCOUNT] = 0;
           player.SetCustomProperties(properties);
       }
   }


   // 플레이어 패널을 눌렀을 때 호출되는 커스텀프로퍼티 함수
   // ["VotedCount"] 에 변화가 생기면 targetPlayer의 curVotedNum + 1 (현재 득표 수 + 1)
   public static void VotePlayer(this Player targetPlayer)
   {
       // Containskey가 true 이면 값을 넘겨주고
       // false 이면 0을 넘겨줌
       int num = targetPlayer.CustomProperties.ContainsKey(VOTEDCOUNT) ? (int)targetPlayer.CustomProperties[VOTEDCOUNT] : 0;
     
       PhotonHashtable properties = new PhotonHashtable();
       properties[VOTEDCOUNT] = num++;
       targetPlayer.SetCustomProperties(properties);

       Debug.Log($"{targetPlayer.NickName} 의 득표 수 : {_playerData.VoteCount + 1} ");
   }


   //votepanel 로 이동 할 것
   // 투표 결과를 알려주는 함수
   // 모든 플레이어의 ["VotedCount"] 의 변화 결과를 알려줌
   public static void GetVoteResult(this Player player)
   {
       foreach (Player players in PhotonNetwork.PlayerList)
       {
           int votedResults = player.CustomProperties.ContainsKey(VOTEDCOUNT) ? (int)player.CustomProperties[VOTEDCOUNT] : 0;
           Debug.Log($"{player.NickName} 의 총 득표 수 : {_playerData.VoteCount}");
           //TODO : votedResults 만큼 targetPlayer의 패널에 익명 이미지 생성
       }

       // 득표수가 가장 많은 플레이어 당선 로직
       foreach (Player players in PhotonNetwork.PlayerList)
       {
           //TODO : 반복문 돌려서 찾아내기
       }

   }
} 

