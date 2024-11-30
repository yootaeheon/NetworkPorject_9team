using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GlobalLifeSupportMission : MonoBehaviour
{

    [SerializeField] private List<Chips> _leftSlots = new List<Chips>(9);
    [SerializeField] private List<Chips> _waitSlots = new List<Chips>(9);
    [SerializeField] private List<GameObject> _rightSlots = new List<GameObject>(9);
    [SerializeField] private AudioClip _failCilp;

    private List<GameObject> _returnSlots = new List<GameObject>();

    private bool[] _emptyArr = new bool[9];

    private MissionController _missionController;
    private MissionState _missionState;

    private Vector2 _tempVector;
    private int _randIndex;
    private bool nullCheck;
    private bool compareCheck;

    private RectTransform _emptyRect;
    private GameObject _waitObj;
    private GameObject _rightObj;
    private GameObject _compareSlotObj;

    public event EventHandler OnCheckedSlot;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();

        _missionState.MissionName = "생명 유지 장치 재설정하기";
    }

    private void OnEnable()
    {
        for (int i = 0; i < _emptyArr.Length; i++)
        {
            _emptyArr[i] = true;
        }


        OnCheckedSlot += CompareSlot;

        //리스트 순서 셔플
        LeftSlotShuffle();
        WaitSlotShuffle();
    }

    private void OnDisable()
    {
        OnCheckedSlot -= CompareSlot;
        
        //요소 추가 반복문
        for (int i = 0; i < _rightSlots.Count; i++)
        {
            //오른쪽 슬롯에 배치된 오브젝트가 있다면
            if (_rightSlots[i].transform.childCount != 0)
            {
                for (int j = 0; j < _emptyArr.Length; j++)
                {   
                    //대기열 체크 배열에서 false를 만나면
                    if (!_emptyArr[j])
                    {
                        //비어있는 요소에 추가
                        _waitSlots[j] = _rightSlots[i].transform.GetChild(0).GetComponent<Chips>();
                        _emptyArr[j] = true;
                        break;
                    }
                }
            }
        }



        for (int i = 0; i < _rightSlots.Count; i++)
        {
            //오른쪽 슬롯에 배치된 오브젝트가 있다면
            if (_rightSlots[i].transform.childCount != 0)
            {
                for (int j = 0; j < _returnSlots.Count; j++)
                { 
                    if (_returnSlots[j].transform.childCount != 0) continue;

                    //raycast 설정 변경
                    _rightSlots[i].GetComponent<Image>().raycastTarget = true;
                    
                    //하이어라키 오브젝트 위치 변경
                    _rightSlots[i].transform.GetChild(0).SetParent(_returnSlots[j].transform);

                    //raycast 설정 변경
                    _returnSlots[j].transform.GetChild(0).GetChild(0).GetComponent<Image>().raycastTarget = true;
                    
                    //이동 후 화면상 위치 재설정
                    _emptyRect = _returnSlots[j].transform.GetChild(0).GetComponent<RectTransform>();
                    _emptyRect.anchoredPosition = new Vector2(0, 0);
                    break;
                }

            }
        } 
    }


    private void Update()
    {
        _missionController.PlayerInput();
        SelectChip();
    }

    /// <summary>
    /// 왼쪽 슬롯 셔플 기능
    /// </summary>
    private void LeftSlotShuffle()
    {
        for (int i = 0; i < _leftSlots.Count; i++)
        {
            _randIndex = UnityEngine.Random.Range(0, 9);

            //현재 인덱스와 같지 않은 랜덤 인덱스 선택
            if (i == _randIndex)
            {
                while (i != _randIndex)
                {
                    _randIndex = UnityEngine.Random.Range(0, 9);
                }
            }

            //이전 위치의 Rect
            RectTransform prevRect = _leftSlots[i].transform.parent.GetComponent<RectTransform>();

            //다음 위치의 Rect
            RectTransform nextRect = _leftSlots[_randIndex].transform.parent.GetComponent<RectTransform>();

            //위치 교환
            _tempVector = prevRect.anchoredPosition;
            prevRect.anchoredPosition = nextRect.anchoredPosition;
            nextRect.anchoredPosition = _tempVector;

            //리스트 요소 교환
            Chips tempElement = _leftSlots[i];
            _leftSlots[i] = _leftSlots[_randIndex];
            _leftSlots[_randIndex] = tempElement;
        }


    }

    /// <summary>
    /// 대기 슬롯 셔플 기능
    /// </summary>
    private void WaitSlotShuffle()
    {
        for (int i = 0; i < _waitSlots.Count; i++)
        {
            _randIndex = UnityEngine.Random.Range(0, 9);

            if (i == _randIndex)
            {
                while (i != _randIndex) _randIndex = UnityEngine.Random.Range(0, 9);
            }

            RectTransform prevRect = _waitSlots[i].transform.parent.GetComponent<RectTransform>();
            RectTransform nextRect = _waitSlots[_randIndex].transform.parent.GetComponent<RectTransform>();

            //위치 교환
            _tempVector = prevRect.anchoredPosition;
            prevRect.anchoredPosition = nextRect.anchoredPosition;
            nextRect.anchoredPosition = _tempVector;

            //리스트 요소 교환
            Chips tempElement = _waitSlots[i];
            _waitSlots[i] = _waitSlots[_randIndex];
            _waitSlots[_randIndex] = tempElement;
        }

    }

    /// <summary>
    /// 칩 선택 후 이동 기능
    /// </summary>
    private void SelectChip()
    {
        if (!_missionState.IsDetect) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_missionController._searchObj.CompareTag("WaitSlot"))
            {
                _waitObj = _missionController._searchObj;
                _waitObj.transform.localScale = new Vector2(0.8f, 0.8f);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (_waitObj == null) return;

            _waitObj.transform.position = _missionState.MousePos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _compareSlotObj = _missionController._searchObj.gameObject;

            //놓은 곳이 오른쪽 슬롯인지
            if (_compareSlotObj.CompareTag("RightSlot"))
            {
                if (_waitObj == null) return;

                //아무것도 배치되어 있지 않은 슬롯인지 검사
                if (_compareSlotObj.transform.childCount == 0)
                {
                    //대기열로 돌아갈 오브젝트 위치 저장
                    _returnSlots.Add(_waitObj.transform.parent.parent.gameObject);

                    //오른쪽 슬롯으로 이동
                    _waitObj.transform.parent.SetParent(_compareSlotObj.transform);

                    RectTransform child = _waitObj.GetComponent<RectTransform>();
                    RectTransform parent = _waitObj.transform.parent.GetComponent<RectTransform>();
                    Chips chip = _waitObj.transform.parent.GetComponent<Chips>();

                    //슬롯 이동 후 위치 재설정
                    child.anchoredPosition = new Vector2(0, 0);
                    parent.anchoredPosition = new Vector2(0, 0);
                    _waitObj.GetComponent<Image>().raycastTarget = false;
                    _compareSlotObj.GetComponent<Image>().raycastTarget = false;
                     
                    //대기열로 돌아갈 인덱스
                    int index = _waitSlots.IndexOf(chip);
                    
                    //배열의 몇 번째 인덱스에서 빠졌는지 저장
                    _emptyArr[index] = false;
                    _waitSlots[index] = null;
                     
                    SoundManager.Instance.SFXPlay(_missionState._clips[0]);

                }
                else
                {
                    _waitObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                }
            }
            else
            {
                if (_waitObj == null) return;

                _waitObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            }

            _waitObj.transform.localScale = new Vector2(1, 1);
            _waitObj = null;

            //미션 클리어 여부 조건 검사
            for (int i = 0; i < _waitSlots.Count; i++)
            { 
                nullCheck = _waitSlots[i] == null ? true : false;

                if (!nullCheck)
                {
                    break;
                }
            }

            if (nullCheck)
            {
                OnCheckedSlot?.Invoke(this, EventArgs.Empty);
            } 
        }
    }


    /// <summary>
    /// 왼쪽, 오른쪽 슬롯이 서로 동일하게 배치되었는지 검사
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void CompareSlot(object sender, EventArgs args)
    {

        for (int i = 0; i < _leftSlots.Count; i++)
        {
            Chips child = _rightSlots[i].transform.GetChild(0).GetComponent<Chips>();

            if (_leftSlots[i]._chipType == child._chipType)
            {
                compareCheck = true;
            }
            else
            {
                compareCheck = false;
                break;
            }
        }

        if (compareCheck)
        {
            Debug.Log("미션 클리어");

            MissionClear();
        }
        else
        {
            MissionFail();
        }
    }

    private void MissionClear()
    {
        SoundManager.Instance.SFXPlay(_missionState._clips[1]);
        GameManager.Instance.CompleteGlobalMission();
        _missionController.MissionCoroutine(0.5f);
    }

    private void MissionFail()
    {
        SoundManager.Instance.SFXPlay(_failCilp);
        _missionController.MissionCoroutine(0.5f);


    }

}