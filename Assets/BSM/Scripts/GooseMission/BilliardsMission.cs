using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BilliardsMission : MonoBehaviour
{

    private MissionController _missionController;
    private MissionState _missionState;
    private List<GameObject> _stainList = new List<GameObject>(4);

    private RectTransform _dishCloth;

    private void Awake() => Init(); 

    private void Start()
    {
        _dishCloth = _missionController.GetMissionObj<RectTransform>("DishCloth");

    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 4;
        IsCleaning = true;
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "당구공 청소하기";
    }

    private void OnDisable()
    {
        foreach (GameObject element in _stainList)
        {
            element.SetActive(true);
            element.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        }
    }

    private void Update()
    {
        if (GameManager.Instance.GlobalMissionState)
        {
            gameObject.SetActive(false);
        }

        _missionController.PlayerInput();
        DishClothUpdate();
        RemoveTrain();
    }

    private void DishClothUpdate()
    {
        if (_missionState.MousePos.x < 300f || _missionState.MousePos.x > 1550
            || _missionState.MousePos.y > 980f || _missionState.MousePos.y < 100f)
            return;
  
        _dishCloth.position = _missionState.MousePos; 

    }


    private bool IsCleaning;

    private void RemoveTrain()
    {
        if (!_missionState.IsDetect) return;

        GameObject go = _missionController._searchObj;

        if (!_stainList.Contains(go))
        {
            _stainList.Add(go);
        }


        if (!IsCleaning)
            return;

        IsCleaning = false; 

        SoundManager.Instance.SFXPlay(_missionState._clips[0]);

        Image image = go.GetComponent<Image>();
        image.color = new Color(1, 1, 1, image.color.a - 0.35f);

        if (image.color.a < 0)
        {
            _missionState.ObjectCount--;
            go.SetActive(false);
            MissionClear();
        }

        StartCoroutine(CleaningCoroutine());

    }

    private IEnumerator CleaningCoroutine()
    {
        yield return Util.GetDelay(0.5f);
        IsCleaning = true;
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
