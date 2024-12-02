using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPerform : MonoBehaviour
{
    [Header("전체적인 미션 관리 리스트")]
    [SerializeField] private List<MonoBehaviour> _missionList = new List<MonoBehaviour>();
     
    [Header("진행 가능한 미션 리스트")]
    [SerializeField] private List<TextMeshProUGUI> _textList = new List<TextMeshProUGUI>();
    [SerializeField] private List<MissionState> _assignList = new List<MissionState>();

    [Header("Mission List Hide/Show")]
    [SerializeField] private Button _arrowBtn; 
    [SerializeField] private GameObject _taskObj;
    [SerializeField] private RectTransform _globalTaskRect;
    [SerializeField] private Image _arrowImage;

    private MissionState _missionState; 
    private int randIndex = 0;
    private bool isTaskState;

    private void Awake() => Init();
    private void Start()
    {
        SetMissionList();
    }
    
    private void Init()
    {
        _arrowBtn.onClick.AddListener(HideAndShowTask);
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
            //할당된 State Get
            assignState = _assignList[i];
             
            //Clear된 미션인지?
            if(!assignState.IsAssign && assignState.TextIndex != (-1))
            {
                nextMissionIndex = Random.Range(0, 8);
                 
                ///Random으로 돌린 State가 할당된 리스트에 포함되어 있을 경우 재배정
                if (_assignList.Contains(_missionList[nextMissionIndex].GetComponent<MissionState>()))
                { 
                    while (true)
                    {
                        nextMissionIndex = Random.Range(0, 8);

                        MissionState nextState = _missionList[nextMissionIndex].GetComponent<MissionState>();

                        //State가 할당된 리스트에 없을 경우 미션 교체
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
     
    private void HideAndShowTask()
    {
        isTaskState = !isTaskState;

        Debug.Log($"task:{isTaskState}");

        if (isTaskState)
        {
            _taskObj.SetActive(true);
            _globalTaskRect.anchoredPosition = new Vector2(105f, -131f);
            _arrowImage.transform.rotation = Quaternion.Euler(new Vector3(0f,0f,180f));   

        }
        else
        {
            _taskObj.SetActive(false);
            _globalTaskRect.anchoredPosition = new Vector2(105f, 124f);
            _arrowImage.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        } 
    }
     
}
