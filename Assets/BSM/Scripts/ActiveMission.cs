using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveMission : BaseMission
{
    [SerializeField] private GameObject _missionPrefab;
    [SerializeField] private GameObject _missionCanvas;
     
    public void GetMission()
    {
        GameObject mission = Instantiate(_missionPrefab);
        mission.transform.SetParent(_missionCanvas.transform);  
        _missionPrefab.SetActive(true);
    }



}
