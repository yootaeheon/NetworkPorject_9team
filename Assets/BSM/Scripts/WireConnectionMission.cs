using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WireConnectionMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;

    private List<GameObject> _wireList;
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
        _wireList = new List<GameObject>(_missionState.ObjectCount);
    }

    private void OnDisable()
    {
        //미션 팝업 창 종료 시 기존 전선 사이즈 원상복구
        foreach(GameObject ele in _wireList)
        {
            RectTransform rect = ele.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 20); 
        } 
    }
     
    private void Update()
    {
        _missionController.PlayerInput();
        WireConnection();
    } 

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

            if (!_wireList.Contains(_wire.gameObject))
            {
                _wireList.Add(_wire.gameObject);
            }


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
            CompareColor();
            MissionClear();
        }
    }

    /// <summary>
    /// 색깔 비교 기능
    /// </summary>
    public void CompareColor()
    {
        Image endPointImage = _missionController._searchObj.GetComponent<Image>();
        Color endPointColor = endPointImage.color;

        //도착지와 Wire의 색 비교
        if (endPointColor.CompareRGB(_wire.GetComponent<Image>().color))
        {
            float endR = endPointColor.r;
            float endG = endPointColor.g;
            float endB = endPointColor.b;

            endPointImage.color = new Color(endR, endG, endB, 1);
            Image childGlow = endPointImage.transform.GetChild(0).GetComponent<Image>();
            childGlow.color = new Color(endR,endG,endB,1);

            SoundManager.Instance.SFXPlay(_missionState._clips[0]);
            _missionState.ObjectCount--;
        }
        else
        {
            _wire.sizeDelta = new Vector2(0, 20);
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
