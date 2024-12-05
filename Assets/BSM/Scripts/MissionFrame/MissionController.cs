using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionController : BaseMission
{ 
    //UI 상호작용
    private GraphicRaycaster _graphicRaycaster;
    private Canvas _grCanvas;
    private PointerEventData _ped = new PointerEventData(EventSystem.current);
    private List<RaycastResult> _rayResult = new List<RaycastResult>();

    //팝업 종료 코루틴
    private Coroutine _closeCo; 
    private MissionState _missionState;
    [HideInInspector] public GameObject _searchObj;
 

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

    private void Update()
    {
        if (VoteScene.Instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    /// <summary>
    /// 미션 진행 중 오브젝트 감지
    /// </summary>
    public void PlayerInput()
    {
        _missionState.MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y); 

        _ped.position = _missionState.MousePos;

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
        SoundManager.SFXPlay(_missionState._clips[2]);
        MissionCoroutine(0.5f);
    }

    /// <summary>
    /// 공용으로 사용할 팝업 종료 애니메이션 코루틴
    /// </summary>
    public void MissionCoroutine(float delay)
    {
        _closeCo = StartCoroutine(CloseMission(delay)); 
    }

    private IEnumerator CloseMission(float delay)
    {
        yield return Util.GetDelay(delay);
        _missionState.ClosePopAnim();
        yield return Util.GetDelay(delay);
        gameObject.SetActive(false);
    }

    public T GetMissionObj<T>(string name) where T : Component
    { 
        return GetMissionComponent<T>(name);
    }

    public GameObject GetMissionObj(string name)
    { 
        return GetMissionObject(name);
    }


}
