using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviourPunCallbacks   // 나중에 떼야함
{
    public static GameFlowManager Instance;

    private float Succ = 0f;
    public const string RoomName = "TestRoom";


    //필요한 데이터 : 미션 게이지 , 

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스가 있으면 새로 생성된 객체를 제거
        }
    }
    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();

    }
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false; // 비공개 방

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        // 방에 들어가면 ~   
        StartCoroutine(StartDelayRoutine());
    }
    IEnumerator StartDelayRoutine()
    {
        yield return 1.5f.GetDelay(); // 네트워크 준비용 대기시간 필요 
        TestGameStart();
    }

    public void TestGameStart()
    {
        // 게임 시작 

        PlayerSpawn();
    }




    public void ReportingOn() 
    {
        // Todo : 신고되면 투표씬으로 이동
        Debug.Log("투표 시작 !");
    }
    public void MissionTest() 
    {
        Succ += 5f;

        if (Succ > 100f) 
        {
            MissonClearWin();
        }
    }
    public void MissonClearWin() 
    {
        Debug.Log("미션 승리 !");
    }
    public void SabotageSucc() 
    {
        Debug.Log("사보타지 !");
    }

    //public void DuckWin() { }

    private void PlayerSpawn()
    {
        Vector2 randPos = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));
        // 스폰 자리를 배열로 받아놓고 랜덤으로 남은 플레이어들 소환시키면 될듯 

        GameObject obj = PhotonNetwork.Instantiate("LJH_Player", randPos, Quaternion.identity);
        GameObject panel = PhotonNetwork.Instantiate("NamePanel", randPos, Quaternion.identity);
        panel.GetComponent<UiFollowingPlayer>().setTarget(obj);
    }
}
