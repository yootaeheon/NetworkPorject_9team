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
        _missionController.PlayerInput(); 
        Interaction();
    }
     
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
        _missionController.MissionCoroutine(0.5f);
        //GameManager한테 글로벌 미션 클리어 했다고 전달?
        //RPC로 전달
    }


}
