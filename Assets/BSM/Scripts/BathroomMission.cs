using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BathroomMission : MonoBehaviour
{
    private int _clearCount = 5;

    private MissionController _missionController;
    private MissionState _missionState;
    private List<GameObject> _stainList = new List<GameObject>(5);
    private Coroutine _clearRoutine;

    private void Awake() => Init();
    private void OnEnable() => _missionState.ObjectCount = 5;

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>(); 
    }

    /// <summary>
    /// 미션 종료 시 모든 오브젝트 활성화
    /// </summary>
    private void OnDisable()
    {
        foreach (GameObject ele in _stainList)
        {
            ele.gameObject.SetActive(true); 
        } 
    }

    private void Update()
    {
        _missionController.PlayerInput();
        RemoveTrain();
        MissionClear();
    }


    /// <summary>
    /// 감지한 오브젝트 제거 기능
    /// </summary>
    private void RemoveTrain()
    {
        //감지한 오브젝트가 없을 경우 리턴
        if (!_missionState.IsDetect) return;

        GameObject go = _missionController._searchObj.gameObject; 
        Image image = go.GetComponent<Image>();

        if (Input.GetMouseButtonDown(0))
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.35f);

            //Alpha 값이 0이 됐을 경우 비활성화 처리 및 미션 진행 카운트 감소
            if (image.color.a < 0)
            { 
                _missionState.ObjectCount--; 
                _stainList.Add(go);
                go.SetActive(false);
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
        } 
    }

    /// <summary>
    /// 미션 클리어 시 동작 기능
    /// </summary>
    private void MissionClear()
    {
        if(_missionState.ObjectCount < 1)
        {
            _clearRoutine = StartCoroutine(ClearCoroutine());
            CoroutineManager.Instance.ManagerStartCoroutine(this, _clearRoutine);
        }
    }

    private IEnumerator ClearCoroutine()
    {
        yield return Util.GetDelay(1f);
        //미션 클리어 사운드 재생
        //총 미션 게이지 증가
        gameObject.SetActive(false);
    }


}
