using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;


    private Animator _animator;
    private int _pillowHash;
    private int _idleHash;

    private event EventHandler OnChangedCount;
    private int _flag;

    private void Awake()
    {
        Init();
    }


    private void Init()
    {
        _missionState = GetComponent<MissionState>();
        _missionController = GetComponent<MissionController>();
        _missionState.MissionName = "베개속 두드려 펴기"; 
    }

    private void Start()
    {
        _animator = _missionController.GetMissionObj<Animator>("Pillow");
        _pillowHash = Animator.StringToHash("Pillow");
        _idleHash = Animator.StringToHash("PillowIdle");
    }

    private void OnEnable()
    {
        _flag = 1 << 0;
        _missionState.ObjectCount = 15;
        OnChangedCount += MissionClear;
    }

    private void OnDisable()
    {
        OnChangedCount -= MissionClear;
    }

    private void Update()
    {
        if (GameManager.Instance.GlobalMissionState)
        {
            gameObject.SetActive(false);
        }

        _missionController.PlayerInput();
        ShakingPillow();
    }

    /// <summary>
    /// 베개 클릭 기능
    /// </summary>
    private void ShakingPillow()
    {
        if (!_missionState.IsDetect) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (_missionState.ObjectCount >= 1 && _flag == 1 << 0)
            {
                _animator.Play(_pillowHash);
                SoundManager.Instance.SFXPlay(_missionState._clips[0]);
                _missionState.ObjectCount--;
            }
            else if (_missionState.ObjectCount < 1 && _flag == 1 << 0)
            {
                _flag = 1 << 1;
                OnChangedCount?.Invoke(this, EventArgs.Empty);
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            //베개 모션 재생 후 대기 상태로
            _animator.Play(_idleHash);
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
    private void MissionClear(object sender, EventArgs args)
    { 
        SoundManager.Instance.SFXPlay(_missionState._clips[1]);
        _missionController.MissionCoroutine(0.5f);
        IncreaseTotalScore(); 
    }
}
