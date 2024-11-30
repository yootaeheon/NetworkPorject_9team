using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityColor : MonoBehaviour
{
    private MissionController _controller;


    private void Start()
    {
        _controller = GetComponent<MissionController>();
        _controller.GetMissionObj<Image>("Arm").color = MissionState.PlayerColor; 
    }

    public void GetColor(Color color)
    {
        _controller.GetMissionObj<Image>("Arm").color = color;
    }

}
