using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFireMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;


    private GameObject _fireObjects;
    private bool IsBurn;

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
        IsBurn = false;
        _missionState.ObjectCount = 3;
    }

    private void Start()
    {
        _fireObjects = _missionController.GetMissionObj("FireObjects");
    }

    private void Update()
    {
        if (GameManager.Instance.FirstGlobalFire)
        {
            gameObject.SetActive(false);
        }

        _missionController.PlayerInput();
        SelectFireExtinguisher();
        OffFireCheck();
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
            fire.FireCheck();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            fire.IsPowder = false;
        }

    }

    private void OffFireCheck()
    {
        int count = 0;

        for (int i = 0; i < _fireObjects.transform.childCount; i++)
        {
            if (_fireObjects.transform.GetChild(i).gameObject.activeSelf)
            {
                count++;
            }
        }
        _missionState.ObjectCount = count;


        if (_missionState.ObjectCount > 0) return;
            MissionClear();
    }

    private void MissionClear()
    {
        if (IsBurn) return;
        IsBurn = true;

        Debug.Log("미션 클리어");
        SoundManager.Instance.SFXPlay(_missionState._clips[1]);
        GameManager.Instance.GlobalFire();
        GameManager.Instance.FirstFire();
        _missionController.MissionCoroutine(0.5f);
    }
}
