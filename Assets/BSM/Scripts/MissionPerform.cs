using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPerform : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _missionList = new List<MonoBehaviour>();
    [SerializeField] private List<TextMeshProUGUI> _textList = new List<TextMeshProUGUI>();

    [SerializeField] private List<MissionState> _assignList = new List<MissionState>();

    private MissionState _missionState;
    private MonoBehaviour _parentClass;
    private int randIndex = 0;

    private void Start()
    {
        SetMissionList();
    }
     
    public void SetMissionList()
    {
        for (int i = 0; i < 3; i++)
        {
            randIndex = Random.Range(0, 8);

            _missionState = _missionList[randIndex].gameObject.GetComponent<MissionState>();
             
            if (_missionState.IsAssign)
            {
                while (true)
                {
                    randIndex = Random.Range(0, 8);
                    _missionState = _missionList[randIndex].gameObject.GetComponent<MissionState>();

                    if (!_missionState.IsAssign)
                    {
                        break;
                    }
                }
            }

            _textList[i].text = _missionState.MissionName;
            _assignList.Add(_missionState);
            _missionState.IsPerform = true;
            _missionState.IsAssign = true;
            _missionState.TextIndex = i;
        }
        
    }

    private void Update()
    {
        UpdateMissionList();
    }

    private void UpdateMissionList()
    {
        int nextMissionIndex = 0;
        MissionState assignState = null;
  
        for (int i = 0; i < _assignList.Count; i++)
        {
            assignState = _assignList[i];
             
            if(!assignState.IsAssign && assignState.TextIndex != (-1))
            {
                nextMissionIndex = Random.Range(0, 8);
                 
                if (_assignList.Contains(_missionList[nextMissionIndex].GetComponent<MissionState>()))
                { 
                    while (true)
                    {
                        nextMissionIndex = Random.Range(0, 8);

                        MissionState nextState = _missionList[nextMissionIndex].GetComponent<MissionState>();

                        if (!_assignList.Contains(nextState))
                        {
                            nextState.TextIndex = assignState.TextIndex;
                            nextState.IsPerform = true;
                            nextState.IsAssign = true;
                            _textList[i].text = nextState.MissionName;
                            _assignList[i] = nextState;
                            assignState.TextIndex = -1;
                            break;
                        } 
                    }
                } 
            } 
        }
         
    } 
    //mission state의 perform이 true인 애들만 미션 수행 가능하게.
    //perform이 True인 애들만 Text에 State의 MissionName 셋팅

    //미션을 클리어 했으면 해당 미션의 perform값 false로 변경하고
    //다른 미션을 또 True로 변경하여 Text 셋팅

    //미션 리스트 확장/축소 애니메이션 필요

}
