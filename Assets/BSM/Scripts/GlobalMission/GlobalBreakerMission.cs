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
            


        }


    }


}
