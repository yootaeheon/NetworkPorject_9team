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


    public GameObject _searchObj;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _grCanvas = transform.parent.GetComponent<Canvas>();
        _graphicRaycaster = _grCanvas.GetComponent<GraphicRaycaster>();
        GetMissionComponent<Button>("MissionCloseButton").onClick.AddListener(CloseMissionPopUp);   
    }


    public void PlayerInput()
    { 
        _ped.position = Input.mousePosition;

        _rayResult.Clear(); 

        _graphicRaycaster.Raycast(_ped, _rayResult);
  
        if (_rayResult.Count > 0)
        {
            //감지한 오브젝트를 넘겨줌
            _searchObj = _rayResult[0].gameObject;
            
        } 
    }


    private void CloseMissionPopUp()
    {
        gameObject.SetActive(false);
    }

}
