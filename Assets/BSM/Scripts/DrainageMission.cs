using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrainageMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;

    private List<GameObject> _goList; 
    private GameObject _go;

    private RectTransform _featherSpawn;
    private RectTransform _eggShellSpawn;
    private RectTransform _friedEggSpawn;
    private RectTransform _dustSpawn1;
    private RectTransform _dustSpawn2;
    private RectTransform _dustSpawn3;

    private Animator _animator;
    private Coroutine _aniCo;

    private bool IsSelect;
    private float _curObjPosX;
    private float _curObjPosY;
    private int _commonHash;
  

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionState = GetComponent<MissionState>();
        _missionController = GetComponent<MissionController>();
        _missionState.MissionName = "막힌 샤워 배수구 뚫기";
        _commonHash = Animator.StringToHash("CommonClip");
    }

    private void OnEnable()
    {

        _missionState.ObjectCount = 6;
        _goList = new List<GameObject>(_missionState.ObjectCount);
    }

    private void Start()
    {
       
        SetObjectPos();
    }


    private void OnDisable()
    {
        foreach (GameObject element in _goList)
        {
            element.SetActive(true);
            ResetObjPos(element.gameObject); 
        } 
    }


    private void Update()
    {
        _missionController.PlayerInput();
        CleaningHole();
    }

    /// <summary>
    /// 각 오브젝트들 초기화될 위치
    /// </summary>
    private void SetObjectPos()
    {

        _featherSpawn = _missionController.GetMissionObj<RectTransform>("FeatherSpawn");
        _eggShellSpawn = _missionController.GetMissionObj<RectTransform>("EggShellSpawn");
        _friedEggSpawn = _missionController.GetMissionObj<RectTransform>("FriedEggSpawn");
        _dustSpawn1 = _missionController.GetMissionObj<RectTransform>("Dust1Spawn");
        _dustSpawn2 = _missionController.GetMissionObj<RectTransform>("Dust2Spawn");
        _dustSpawn3 = _missionController.GetMissionObj<RectTransform>("Dust3Spawn");

    }


    /// <summary>
    /// GameObject 위치 재정렬
    /// </summary>
    /// <param name="go"></param>
    private void ResetObjPos(GameObject go)
    {
        DraingeObjType type = go.GetComponent<MissionObj>().type;
        RectTransform _goRect = go.GetComponent<RectTransform>();
        
        switch (type)
        {
            case DraingeObjType.Feather:
                _goRect.anchoredPosition = _featherSpawn.anchoredPosition;
                break;

            case DraingeObjType.FriedEgg:
                _goRect.anchoredPosition = _friedEggSpawn.anchoredPosition;
                break;

            case DraingeObjType.EggShell:
                _goRect.anchoredPosition = _eggShellSpawn.anchoredPosition;
                break;

            case DraingeObjType.Dust1:
                _goRect.anchoredPosition = _dustSpawn1.anchoredPosition;
                break;

            case DraingeObjType.Dust2:
                _goRect.anchoredPosition = _dustSpawn2.anchoredPosition;
                break;

            case DraingeObjType.Dust3:
                _goRect.anchoredPosition = _dustSpawn3.anchoredPosition;
                break;
        }

    }


    /// <summary>
    /// 클릭한 오브젝트 동작 기능
    /// </summary>
    private void CleaningHole()
    {
        if (!_missionState.IsDetect) return;
         
        if (Input.GetMouseButtonDown(0))
        {
            //오브젝트를 선택하지 않은 상태에서만 오브젝트 선택
            if (!IsSelect)
            {
                _go = _missionController._searchObj;

                if (!_goList.Contains(_go))
                {
                    _goList.Add(_go);
                }
                _curObjPosX = _go.GetComponent<RectTransform>().anchoredPosition.x;
                _curObjPosY = _go.GetComponent<RectTransform>().anchoredPosition.y;
            }
        }


        if (Input.GetMouseButton(0))
        {
            IsSelect = true;
            _go.transform.position = _missionState.MousePos;
            _go.transform.localScale = new Vector3(0.8f, 0.8f);

        }

        else if (Input.GetMouseButtonUp(0))
        {
            RectTransform _rect = _go.GetComponent<RectTransform>();

            //이동한 오브젝트 위치
            (float, float) _rectPos = (_rect.anchoredPosition.x, _rect.anchoredPosition.y);

            //시작 위치의 xPos의 여유값
            (float, float) _xPos = (_curObjPosX - 60, _curObjPosX + 60);

            //시작 위치의 yPos의 여유값
            (float, float) _yPos = (_curObjPosY - 60, _curObjPosY + 60);

            _animator = _missionController.GetMissionObj<Animator>(_go.gameObject.name);
            
            //좌,우 이동한 거리 비교
            if (_rectPos.Item1 < _xPos.Item1 || _rectPos.Item1 > _xPos.Item2)
            {
                _aniCo = StartCoroutine(AnimationCoroutine());
            }
            //상,하 이동한 거리 비교
            else if (_rectPos.Item2 < _yPos.Item1 || _rectPos.Item2 > _yPos.Item2)
            {
                _aniCo = StartCoroutine(AnimationCoroutine());
            }

            IsSelect = false;
            _go.transform.localScale = new Vector3(1, 1);
            MissionClear();
        }
    }

    /// <summary>
    /// 오브젝트 제거 동작 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator AnimationCoroutine()
    {
        _animator.Play(_commonHash);
        SoundManager.Instance.SFXPlay(_missionState._clips[0]);
        _missionState.ObjectCount--;
        yield return Util.GetDelay(0.2f);
        _go.SetActive(false);
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
