using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadingScene : MonoBehaviourPun
{
    // 로비씬에 넣어놓고 로비에 있는 정보를 가지고 게임으로 들어가기 
    // 씬에 입장하면 랜덤 스폰기능 
    // 투표씬 전환
    [SerializeField] Transform[] SpawnPoints;


    private bool isOnGame= false;

    public static GameLoadingScene Instance;
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
        SpawnPoints = new Transform[6];
    }
    private void Start()
    {   
        
    }

    private void Update()
    {
        
    }

    public void GameStart() 
    {
        SceneChanger.LoadLevel(1);
        isOnGame = true;
        StartCoroutine(Delaying());
    }

    IEnumerator Delaying() 
    {
        yield return 2f.GetDelay();
        
        RandomSpawner(); // 스폰 지정 및 소환 
        PlayerDataContainer.Instance.RandomSetjob(); // 랜덤 직업 설정 
        player.GetComponent<PlayerController>().SettingColor(color.r, color.g, color.b);  // 일단 보류 색 보존이 안됨 
        player.GetComponent<PlayerController>().SetJobs(); 
    }

    private void RandomSpawner()   
    {
        photonView.RPC("RpcRandomSpawner", RpcTarget.All);
    }
    private GameObject player;
    private Color color;
    private void spawnPlayer(Vector3 Pos) 
    {
        player = PhotonNetwork.Instantiate("LJH_Player", Pos, Quaternion.identity);
        color = PlayerDataContainer.Instance.GetPlayerData(PhotonNetwork.LocalPlayer.ActorNumber).PlayerColor;
        GameObject panel = PhotonNetwork.Instantiate("NamePanel", Pos, Quaternion.identity);
        panel.GetComponent<UiFollowingPlayer>().setTarget(player);
       
    }

    [PunRPC]
    private void RpcRandomSpawner() 
    {
        GameObject obj = GameObject.Find("SpawnPoint");
        
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            SpawnPoints[i] = obj.transform.GetChild(i);
        }
        int x = Random.Range(0, obj.transform.childCount);

        spawnPlayer(SpawnPoints[x].position);
    }
}
