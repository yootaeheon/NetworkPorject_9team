using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirLockMission : MonoBehaviour
{

    private MissionState _missionState;
    private MissionController _missionController;

    private void Awake() => Init();


    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
       
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 3;
    }

    private void Start()
    {
        //1,2,3 레버
        //1,2,3 버튼

    }

    private void Update()
    {
        _missionController.PlayerInput();
    }

    private void PullLever()
    {
        //레버 당기고 몇 초 후 Count 감소

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
