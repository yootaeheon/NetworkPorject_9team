using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WireConnectionMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;

    private List<GameObject> _wireList;
    private List<GameObject> _endPointList;
    private List<Image> _startPointList;

    private Dictionary<GameObject, Color> _colorDict;

    private GameObject _startPos;
    private RectTransform _wire;
    private bool isComplete;

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
        _endPointList = new List<GameObject>(_missionState.ObjectCount);
        _startPointList = new List<Image>(_missionState.ObjectCount);

        _colorDict = new Dictionary<GameObject, Color>();
        isComplete = false;
    }

    private void OnDisable()
    {
        //미션 팝업 창 종료 시 기존 전선 사이즈 원상복구
        foreach (GameObject ele in _wireList)
        {
            RectTransform rect = ele.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 20);
        }

        //endPoint 상태 원상복구
        foreach (GameObject ele in _endPointList)
        {
            Image image = ele.GetComponent<Image>();
            image.color = _colorDict[ele];
            image.raycastTarget = true;
            image.transform.GetChild(0).GetComponent<Image>().color = _colorDict[ele];
        }

        //startPoint 상태 원상복구
        foreach (Image img in _startPointList)
        {
            img.raycastTarget = true;
        }
    }

    private void Update()
    {
        _missionController.PlayerInput();
        WireConnection();

        if (_missionController._searchObj != null)
            Debug.Log(_missionController._searchObj.name);
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

            //선택한 오브젝트가 InnerImage가 아니면 저장 x
            if (_missionController._searchObj.name.Equals("InnerImage"))
            {
                if (!_startPointList.Contains(_missionController._searchObj.GetComponent<Image>()))
                {
                    _startPointList.Add(_missionController._searchObj.GetComponent<Image>());
                }
            }

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

            MouseTrackingWire(_wire);
        }

        else if (Input.GetMouseButtonUp(0))
        {
            CompareColor();
            MissionClear();
        }
    }


    /// <summary>
    /// 전선 마우스 위치 트래킹 기능
    /// </summary>
    /// <param name="wire"></param>
    private void MouseTrackingWire(RectTransform wire)
    {
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


    /// <summary>
    /// 색깔 비교 기능
    /// </summary>
    private void CompareColor()
    {
        if (_wire == null) return;

        Image endPointImage = _missionController._searchObj.GetComponent<Image>();
        Color endPointColor = endPointImage.color;

        //도착지와 Wire의 색 비교
        if (endPointColor.CompareRGB(_wire.GetComponent<Image>().color) && _missionState.MousePos.x > 700f)
        {
            float endR = endPointColor.r;
            float endG = endPointColor.g;
            float endB = endPointColor.b;

            endPointImage.color = new Color(endR, endG, endB, 1);
            Image childGlow = endPointImage.transform.GetChild(0).GetComponent<Image>();
            childGlow.color = new Color(endR, endG, endB, 1);

            //리스트에 오브젝트가 없을 경우 추가
            if (!_endPointList.Contains(endPointImage.gameObject))
            {
                _endPointList.Add(endPointImage.gameObject);

                //추가된 오브젝트의 키가 없을 경우 추가
                if (!_colorDict.ContainsKey(endPointImage.gameObject))
                {
                    _colorDict[endPointImage.gameObject] = new Color(endR, endG, endB, 0.37f);
                }
            }

            //연결이 완료됐으면 wire는 선택 해제
            _wire = null;

            //연결된 같은 색상을 찾아서 RaycastTarget Off
            foreach (Image img in _startPointList)
            {
                if (img.color.CompareRGB(endPointColor))
                {
                    img.raycastTarget = false;
                    endPointImage.raycastTarget = false;
                }
            }

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
        PlayerType type = _missionState.MyPlayerType;

        if (type.Equals(PlayerType.Goose))
        {
            GameManager.Instance.AddMissionScore();
        }
    }

    /// <summary>
    /// 미션 클리어 시 동작 기능
    /// </summary>
    private void MissionClear()
    {
        if (_missionState.ObjectCount < 1 && !isComplete)
        {
            isComplete = true;
            SoundManager.Instance.SFXPlay(_missionState._clips[1]);
            _missionController.MissionCoroutine(0.5f);
            IncreaseTotalScore();
        }
    }

}
