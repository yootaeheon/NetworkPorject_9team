using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPurifierMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;

    private Vector2 _offset = new Vector2(30, -80);
    private GameObject _cord;

    private Animator _cordAnimator;
    private int _cordHash;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        //코드 애니메이션으로 변경 필요
        _cordAnimator = _missionController.GetMissionObj<Animator>("Spray");
        _cordHash = Animator.StringToHash("SprayBody");

        _cord = _missionController.GetMissionObj("CordObject");
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "정수기 수리하기";
    }


    private void OnEnable()
    {
        //코드 위치 초기화
        
        _missionState.ObjectCount = 1;
    }

    


    private void Update()
    {
        _cord.transform.position = _missionState.MousePos + _offset;

    }

    private void DrawLineRenderer()
    {
        //라인 렌더러 위치 정수기 몸체 > 코드까지

    }

    private void MoveCord()
    {
        //마우스 위치를 따라다니게
        //마우스에서 떼면 위치 초기화

    }





    private void IncreaseTotalScore()
    {
        //Player의 타입을 받아 올 수 있으면 좋음
        PlayerType type = PlayerType.Duck;

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
            _missionController.MissionCoroutine();
            IncreaseTotalScore();
        }
    }


}
