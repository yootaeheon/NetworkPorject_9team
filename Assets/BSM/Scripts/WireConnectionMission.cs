using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireConnectionMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;

    private GameObject _startPos;
    private RectTransform _wire;

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

    //길이 : 마우스 좌표 - Wire 좌표 > Width
    //회전각 : 마우스 좌표에 따라 회전?

    /// <summary>
    /// 전선 연결 기능 동작
    /// </summary>
    private void WireConnection()
    {
        if (!_missionState.IsDetect) return;

        if (Input.GetMouseButtonDown(0))
        {
            //전선 시작 위치
            _startPos = _missionController._searchObj.transform.parent.GetChild(0).gameObject;
        }
        else if (Input.GetMouseButton(0))
        {
            //자식이 없거나 마우스 위치가 좌측에 있을 경우 Return
            if (_startPos.transform.childCount == 0 || _missionState.MousePos.x < 670f)
            {
                return;
            }

            //시작 위치 오브젝트의 자식 오브젝트 > wire 이미지 
            _wire = _startPos.transform.GetChild(0).GetComponent<RectTransform>();

            //wire위치 - 마우스 위치
            float wireWidth = Vector2.Distance(_wire.transform.position, _missionState.MousePos);

            //Wire 길이
            _wire.sizeDelta = new Vector2(wireWidth, 20);

            //Wire 방향
            float x = _missionState.MousePos.x;
            float y = _missionState.MousePos.y;

            //마우스 위치 - wire 위치
            Vector3 dir = new Vector3(x, y, 0) - _wire.transform.position;

            //Wire 회전각
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            //Wire 회전
            _wire.transform.rotation = Quaternion.AngleAxis(-angle, -Vector3.forward);

        }

        else if (Input.GetMouseButtonUp(0))
        {
            //뗐을 때 도착 위치가 아니면 줄어듬
            //도착지 오브젝트의 Color로 판단?  
            _wire.sizeDelta = new Vector2(0, 20);

            //도착 위치이면 길이/위치 고정
        }
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
