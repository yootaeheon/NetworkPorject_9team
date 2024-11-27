using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestGameScene3 : MonoBehaviourPunCallbacks
{

    public const string RoomName = "TestRoom";
    private void Start()
    {
        PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(1000, 10000)}";
        PhotonNetwork.ConnectUsingSettings();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PlayerSpawn();
        }
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
        yield return new WaitForSeconds(1.5f); // 네트워크 준비용 대기시간 필요 
        TestGameStart();
    }

    public void TestGameStart()
    {
        // 게임 시작 

        PlayerSpawn();
    }


    private void PlayerSpawn()
    {
        Vector2 randPos = new Vector3(Random.Range(-5, 5),Random.Range(-5, 5));

        
        GameObject obj = PhotonNetwork.Instantiate("LJH_Player", randPos, Quaternion.identity);
        GameObject panel = PhotonNetwork.Instantiate("NamePanel", randPos, Quaternion.identity);
        panel.GetComponent<UiFollowingPlayer>().setTarget(obj);
    }
}
