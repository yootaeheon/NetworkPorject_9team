using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AirLockMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;

    private Animator _animator;
    private int _reverseHash;
    private int _pressHash;
    private int _completeHash;

    private float _notPressTime;
    private float _elapsedTime;

    private bool IsSelect;
  
    private void Awake() => Init();
    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "에어락 문 검사하기";
  
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 3;
    }

    private void Start()
    {
        _reverseHash = Animator.StringToHash("Reverse");
        _pressHash = Animator.StringToHash("Press");
        _completeHash = Animator.StringToHash("Complete");
    }

    private void Update()
    {
        if (GameManager.Instance.GlobalMissionState)
        {
            gameObject.SetActive(false);
        }

        _missionController.PlayerInput();
        PullLever(); 
    }

 

    /// <summary>
    /// 레버 동작 기능
    /// </summary>
    private void PullLever()
    {
        if (!_missionState.IsDetect) return;

        GameObject go = _missionController._searchObj;

        _animator = _missionController.GetMissionObj<Animator>(_missionController._searchObj.name);
        
        if (_animator == null) return;

        MissionObj _obj = go.transform.GetComponent<MissionObj>();



        //누르지 않는 상태 + 미션 완료하지 않은 레버만 Rebind
        if (!IsSelect && !_obj.IsComplete)
        {
            _notPressTime += Time.deltaTime;

            if (_notPressTime > 1f)
            {
                _animator.Rebind();
                _notPressTime = 0;
            }
        }
        else
        {
            _notPressTime = 0;
        }


        if (Input.GetMouseButton(0))
        {
            IsSelect = true;
            _animator.SetFloat(_reverseHash, 1);
            _animator.SetBool(_pressHash, true);

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > 1f)
            {
                _animator.Play(_completeHash);
                _obj.IsComplete = true;
                Animator childAni = go.transform.GetChild(0).GetComponent<Animator>();
                childAni.Play("GreenLight");
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            IsSelect = false;
            _animator.SetFloat(_reverseHash, -1);
            _animator.SetBool(_pressHash, false);
            _elapsedTime = 0;
            MissionClear();
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
        if (_missionState.ObjectCount < 1)
        {
            SoundManager.Instance.SFXPlay(_missionState._clips[1]);
            _missionController.MissionCoroutine(0.5f);
            IncreaseTotalScore();
        }
    }
}
