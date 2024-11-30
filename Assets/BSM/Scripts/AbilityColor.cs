using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityColor : MonoBehaviour
{

    private MissionState _state;
    private MissionController _controller;


    private void Start()
    {
        _controller = GetComponent<MissionController>();
        _state = GetComponent<MissionState>();

        _controller.GetMissionObj<Image>("Arm").color = MissionState.PlayerColor;

    }
}
