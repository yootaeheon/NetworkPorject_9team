using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameScene4 : MonoBehaviour
{
    public const string RoomName = "TestRoom";

    private void Start()
    {
        TestGameStart();
    }

    public void TestGameStart()
    {
        // 게임 시작 

        PlayerSpawn();
    }


    private void PlayerSpawn()
    {
        Vector2 randPos = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5));


        GameObject obj = PhotonNetwork.Instantiate("LJH_Player", randPos, Quaternion.identity);
        GameObject panel = PhotonNetwork.Instantiate("NamePanel", randPos, Quaternion.identity);
        panel.GetComponent<UiFollowingPlayer>().setTarget(obj);
    }
}
