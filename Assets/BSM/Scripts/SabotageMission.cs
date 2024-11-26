using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SabotageMission : MonoBehaviour
{

    private MissionState _missionState;
    private MissionController _missionController;

    private Image _arm;

    private TextMeshProUGUI _codeText;
    private int _randCode;

    private void Awake()
    {
        Init();
    }
     
    private void Init()
    {
        _missionState = GetComponent<MissionState>();
        _missionController = GetComponent<MissionController>();
        _missionState.MissionName = "오리 미션";
    }

    private void OnEnable()
    {
        _randCode = Random.Range(1000, 10000);
        Debug.Log(_randCode);
    }

    private void Start()
    {
        _arm = _missionController.GetMissionObj<Image>("Arm");
        _arm.color = MissionState.PlayerColor;

        _codeText = _missionController.GetMissionObj<TextMeshProUGUI>("Code");
        SetCodeText();
    }

    private void SetCodeText()
    {
        while(_randCode > 0)
        {

            _codeText.text += (_randCode % 10).ToString() + " ";
            _randCode /= 10;

        }

    }




    //랜덤 코드 = random.range
    //앞자리 부터 짤라서 코드Text에 입력


    //버튼 클릭 리스너 = 연동 함수 등록
    //버튼 키패드 child Text 값을 입력?


    //InputText 연동 함수

}
