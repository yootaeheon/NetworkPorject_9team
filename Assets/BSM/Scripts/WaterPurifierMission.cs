using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WaterPurifierMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;
     
    private Vector2 _socketLocation = new Vector2(-397, 121);
    
    private Vector2 _startPos;
    private GameObject _cord;
    private LineRenderer _linerenderer;

    private Animator _cordAnimator;
    private int _cordHash;
    private Image _test;

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
        _startPos = _cord.gameObject.transform.position;
        _linerenderer = _missionController.GetMissionObj<LineRenderer>("SocketObject");
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
        SelectCordObj();
        DrawLineRenderer(); 
        MissionClear();
        
        
        Debug.Log($"{_cord.transform.position}");
    }

    private void SelectCordObj()
    { 
        if (Input.GetMouseButton(0))
        {
            _missionController.PlayerInput();
            _cord.transform.position = _missionState.MousePos; 
        }

        else if (Input.GetMouseButtonUp(0))
        {
            LocationOfSocket();

            _cord.transform.position = _startPos; 
        } 
    }

    private void DrawLineRenderer()
    {
        //라인 렌더러 위치 정수기 몸체 > 코드까지
        //_linerenderer. 
        //UI Linerender 그려줌

    }

    private void LocationOfSocket()
    {
        //마우스 뗐을 때 콘센트의 위치인가?
        //애니메이션 재생
        //Count--;
        //MissionClear;
        //콘센트의 위치가 아니라면 코드의 위치는 처음 위치로
        


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
