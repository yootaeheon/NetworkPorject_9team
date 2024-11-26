using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireConnectionMission : MonoBehaviour
{

    private MissionController _missionController;
    private MissionState _missionState;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "연결 경로 변경하기";
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 4;
    }

    private void Start()
    {
        
    }

    //좌측 전선 클릭 시 마우스 좌표로 전선 이동
    //다른 색깔과 연결 시 연결 안됨


    //IF 같은 색깔과 연결
    //배선은 프리팹으로 생성?
    //전선 연결
    //미션 카운트 --
    //우측 전선 알파값 1
    //전선 고정
    //전선 연결 사운드 재생

    //ELSE
    //전선 연결 안됨
    //전선 다시 줄어듬


    private void Update()
    {
        _missionController.PlayerInput();
        WireConnection();
    }

    private void WireConnection()
    {


    }


    private void IncreaseTotalScore()
    {
        //Player의 타입을 받아 올 수 있으면 좋음
        PlayerType type = PlayerType.Goose;

        if (type.Equals(PlayerType.Goose))
        {
            //전체 미션 점수 증가
            //미션 점수 동기화 필요 > 어디서 가져올건지
            GameManager.Instance.TEST();
        }
    }

    /// <summary>
    /// 미션 클리어 시 동작 기능
    /// </summary>
    private void MissionClear()
    {
        if (_missionState.ObjectCount < 1)
        {
            SoundManager.Instance.SFXPlay(_missionState._clips[1]);
            _missionController.MissionCoroutine(0.5f);
            IncreaseTotalScore();
        }
    }

}
