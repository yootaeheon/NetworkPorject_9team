using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalBreakerMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController; 

    private void Awake()
    {
        Init();
    }
     
    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "전등 고치기";
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 8;
    } 

    private void Update()
    {
        //클라이언트에서 미션을 클리어 했을 경우 모든 클라이언트 미션 팝업창 비활성화
        if (GameManager.Instance._globalMissionClear)
        {
            gameObject.SetActive(false);
        }

        _missionController.PlayerInput(); 
        Interaction();
    }
     
    /// <summary>
    /// 스위치 이미지와의 상호작용
    /// </summary>
    private void Interaction()
    {
        if (!_missionState.IsDetect) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            GlobalButton global = _missionController._searchObj.GetComponent<GlobalButton>();
            global.PlayAnimation();
            global.PowerCount--;
             
            if(global.PowerCount < 0)
            {
                if (!(global.PowerCount == -1 && !global.ButtonCheck))
                {
                    _missionState.ObjectCount += global.ButtonCheck ? -1 : 1;
                } 
            } 
            MissionClear();
        } 
    }

    private void MissionClear()
    {
        if (_missionState.ObjectCount > 0) return;

        SoundManager.Instance.SFXPlay(_missionState._clips[1]);
        GameManager.Instance.CompleteGlobalMission();
        _missionController.MissionCoroutine(0.5f); 
    }


}
