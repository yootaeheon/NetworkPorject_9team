using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BathroomMission : MonoBehaviour
{
     
    private MissionController _missionController;
    private MissionState _missionState;
    private List<GameObject> _stainList = new List<GameObject>(5);

    private Vector2 _offset = new Vector2(30, -80);
    private GameObject _spray;
    private Animator _sprayAnim;
    private int _sprayHash;
     
    private void Awake() => Init();

    private void Start()
    {
        _sprayAnim = _missionController.GetMissionObj<Animator>("Spray");
        _sprayHash = Animator.StringToHash("SprayBody");
        _spray = _missionController.GetMissionObj("Spray");
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 5;
    }

    private void Init()
    { 
        _missionController = GetComponent<MissionController>(); 
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "목욕탕 청소하기";
    }

    /// <summary>
    /// 미션 종료 시 모든 오브젝트 활성화
    /// </summary>
    private void OnDisable()
    {
        foreach (GameObject element in _stainList)
        {
            Image image = element.GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1); 
            element.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GlobalMissionState)
        {
            gameObject.SetActive(false);
        }

        _spray.transform.position = _missionState.MousePos + _offset;

        _missionController.PlayerInput();
        RemoveTrain(); 
    }


    /// <summary>
    /// 감지한 오브젝트 제거 기능
    /// </summary>
    public void RemoveTrain()
    {
        //감지한 오브젝트가 없을 경우 리턴
        if (!_missionState.IsDetect) return;

        GameObject go = _missionController._searchObj.gameObject;
        Image image = go.GetComponent<Image>();

        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.Instance.SFXPlay(_missionState._clips[0]);
            _sprayAnim.Play("SprayBody");

            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.35f);

            //동일한 게임 오브젝트가 없을 경우 리스트에 추가
            if (!_stainList.Contains(go))
            {
                _stainList.Add(go);
            }
             
            //Alpha 값이 0이 됐을 경우 비활성화 처리 및 미션 진행 카운트 감소
            if (image.color.a < 0)
            {
                _missionState.ObjectCount--; 
                go.SetActive(false);
                MissionClear();
            } 
            
        }
    }

    private void IncreaseTotalScore()
    {
        PlayerType type = _missionState.MyPlayerType;

        if (type.Equals(PlayerType.Goose))
        {
            GameManager.Instance.AddMissionScore();
        }
    }

    /// <summary>
    /// 미션 클리어 시 동작 기능
    /// </summary>
    private void MissionClear()
    {
        if (_missionState.ObjectCount < 1)
        {
            SoundManager.Instance.SFXPlay(_missionState._clips[1]);
            _missionController.MissionCoroutine(0.5f);
            IncreaseTotalScore();
        }
    } 
}
