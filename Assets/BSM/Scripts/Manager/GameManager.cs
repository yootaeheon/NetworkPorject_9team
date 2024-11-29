using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPun
{
    //테스트용 코드

    public static GameManager Instance { get; private set; } 

    [SerializeField] public Slider _missionScoreSlider;

    private int _totalMissionScore = 30;
    private int _clearMissionScore = 0;
    
     
    //글로벌 미션 팝업창 종료 변수
    public bool _globalMissionClear;
    public bool _firstGlobalFire;
    public bool _secondGlobalFire;
    public int _globalFireCount = 2;


    //테스트용
    public int _myScore = 0;

    //각 글로벌 미션에서 클리어 했을 시 사용할 변수 목록 필요

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log($"남은 미션 :{_clearMissionScore}");
        Debug.Log($"불끄기 횟수 :{_globalFireCount} / GlobalMission{_globalMissionClear}");
    }
     
    /// <summary>
    /// 각 클라이언트에서 미션 클리어 시마다 점수 증가
    /// </summary>
    public void AddMissionScore()
    {
        _myScore++;
        photonView.RPC(nameof(MissionTotalScore), RpcTarget.AllViaServer, 1); 
    }

    /// <summary>
    /// 점수 동기화
    /// </summary>
    /// <param name="score"></param>
    [PunRPC]
    public void MissionTotalScore(int score)
    {
        _clearMissionScore += score; 
        _missionScoreSlider.value = (float)_clearMissionScore / (float)_totalMissionScore;
    }

    /// <summary>
    /// 불끄기 미션 개수
    /// </summary>
    public void GlobalFire()
    {
        photonView.RPC(nameof(FireCountSync), RpcTarget.AllViaServer, 1);
    }

    public void FirstFire()
    {
        photonView.RPC(nameof(FirstFireRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void FirstFireRPC(bool value)
    {
        _firstGlobalFire = value; 
    }

    public void SecondFire()
    {
        photonView.RPC(nameof(SecondFireRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void SecondFireRPC(bool value)
    {
        _secondGlobalFire = value;
    }



    /// <summary>
    /// 불끄기 미션 카운트 동기화
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void FireCountSync(int value)
    {
        _globalFireCount -= value;

        if(_globalFireCount < 1)
        {
            photonView.RPC(nameof(GlobalMissionRPC), RpcTarget.AllViaServer, true);
        } 
    }

    /// <summary>
    /// 각 클라이언트에서 사보타지 능력 사용한 미션 클리어 여부
    /// </summary>
    public void CompleteGlobalMission()
    {
        photonView.RPC(nameof(GlobalMissionRPC), RpcTarget.AllViaServer, true);
    }

    /// <summary>
    /// 사보타지 미션 클리어 여부 동기화
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void GlobalMissionRPC(bool value)
    {
        _globalMissionClear = value;

    }

}
