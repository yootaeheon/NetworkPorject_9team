using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFireMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;

    private GameObject _fireExtinguisher; 

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "화재 진압하기";
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 3;
    }

    private void Update()
    {
        _missionController.PlayerInput();
        SelectFireExtinguisher();
    }

    private void SelectFireExtinguisher()
    {
        if (!_missionState.IsDetect) return;
        if (_missionState.MousePos.x < 400 || _missionState.MousePos.x > 1570) return;

        FireExtinguisher fire = _missionController._searchObj.GetComponent<FireExtinguisher>();

        if (Input.GetMouseButtonDown(0))
        {

            fire.IsPowder = true;
        }

        else if (Input.GetMouseButton(0))
        {
            _missionController._searchObj.transform.position = _missionState.MousePos;

        }

        else if (Input.GetMouseButtonUp(0))
        {
            fire.IsPowder = false;
        }

    }



    //MissionClear


}
