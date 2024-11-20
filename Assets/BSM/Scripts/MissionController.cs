using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionController : BaseMission
{
    private GraphicRaycaster _graphicRaycaster;
    private Canvas _grCanvas;
    private PointerEventData _ped = new PointerEventData(EventSystem.current);
    private List<RaycastResult> _rayResult = new List<RaycastResult>();

    private MissionState _missionState;
    public GameObject _searchObj;

 
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 얼룩 청소 미션 오브젝트 초기화
    /// </summary>
    private void Init()
    {
        _missionState = GetComponent<MissionState>();
        _grCanvas = transform.parent.GetComponent<Canvas>();
        _graphicRaycaster = _grCanvas.GetComponent<GraphicRaycaster>();
        GetMissionComponent<Button>("MissionCloseButton").onClick.AddListener(CloseMissionPopUp);   
    }

    /// <summary>
    /// 미션 진행 중 오브젝트 감지
    /// </summary>
    public void PlayerInput()
    { 
        _ped.position = Input.mousePosition;

        _rayResult.Clear(); 

        _graphicRaycaster.Raycast(_ped, _rayResult);
  
        if (_rayResult.Count > 0)
        {
            if (_rayResult[0].gameObject.name.Equals("MissionCloseButton"))
                return;

            //감지한 오브젝트를 넘겨줌
            _searchObj = _rayResult[0].gameObject;
            _missionState.IsDetect = true;
        }
        else
        {
            _missionState.IsDetect = false;
        }
    }


    /// <summary>
    /// 미션 팝업 종료
    /// </summary>
    private void CloseMissionPopUp()
    {
        gameObject.SetActive(false);
    }

}
