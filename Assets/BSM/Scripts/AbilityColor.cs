using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityColor : MonoBehaviour
{
    private MissionController _controller;
    private MissionState _missionState;
     
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        _controller = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
    }

    private void Start()
    {
        
        _controller.GetMissionObj<Image>("Arm").color = MissionState.PlayerColor; 
    }



    private void Update()
    {

    }

    public void GetColor(Color color)
    {
        _controller.GetMissionObj<Image>("Arm").color = color;
    }

}
