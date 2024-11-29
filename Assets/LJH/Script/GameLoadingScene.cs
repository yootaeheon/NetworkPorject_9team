using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadingScene : MonoBehaviour
{
    // 로비씬에 넣어놓고 로비에 있는 정보를 가지고 게임으로 들어가기 
    // 씬에 입장하면 랜덤 스폰기능 
    // 투표씬 전환
    [SerializeField] Transform[] SpawnPoints;
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
    }



    private void RandomSpawner() 
    {
        //GameObject obj =  GameObject.Find("SpawnPoint");
        //Debug.Log(obj.transform.childCount);
        //obj.transform.childCount;
    }
}
