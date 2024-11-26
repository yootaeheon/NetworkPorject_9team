using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActiveMission : MonoBehaviour
{
    [SerializeField] private GameObject _missionPrefab;
    [SerializeField] private GameObject _missionCanvas;

    private MissionState _missionState;
    private GameObject _mission; 

    private void Awake()
    {
        _missionState = GetComponent<MissionState>();
    }

  
    public void GetMission(Color color)
    {
        _missionPrefab.SetActive(true);
        MissionState.PlayerColor = color;
    } 
}
