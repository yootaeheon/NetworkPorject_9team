using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SabotageMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;

    private Image _arm;

    private TextMeshProUGUI _inputText;
    private TextMeshProUGUI _codeText;
    private int _randCode;

    public event EventHandler OnChangedPassword;


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
        OnChangedPassword += ComparePassword;
    }

    private void Start()
    {
        _arm = _missionController.GetMissionObj<Image>("Arm");
        _arm.color = MissionState.PlayerColor;

        _inputText = _missionController.GetMissionObj<TextMeshProUGUI>("InputText");

        _codeText = _missionController.GetMissionObj<TextMeshProUGUI>("Code");

        SetCodeText();

    }

    private void OnDisable()
    {
        _inputText.text = "";
        _inputText.color = Color.white;
        _inputText.alignment = TextAlignmentOptions.Left;
        _inputText.fontSize = 55;

        OnChangedPassword -= ComparePassword;
        SetCodeText();
    }


    private void SetCodeText()
    {
        _codeText.text = "";
        _randCode = UnityEngine.Random.Range(1000, 10000);
        _codeText.text = Convert.ToString(_randCode, 10);
    }

    public void ClickKeypad(string value)
    {

        _inputText.text += value;
        SoundManager.Instance.SFXPlay(_missionState._clips[0]);

        if (_inputText.text.Length > 3)
        {
            OnChangedPassword?.Invoke(this, EventArgs.Empty);
        }
    }


    /// <summary>
    /// 어떤 사보타지 미션인지 종류를 어떻게?
    /// 프리팹 마다 배치해두니까
    /// 여기서 Type을 설정해두고 클리어 했을 때 타입에 맞는 보상을 지급?
    /// Player한테 어떤 값을 넘겨줘야할지
    /// </summary>


    /// <summary>
    /// 비밀번호 비교 기능 및 Text 설정
    /// </summary>
    private void ComparePassword(object sender, EventArgs args)
    { 
        if (_codeText.text.Equals(_inputText.text))
        {
            //오리 Player 사보타지 능력 획득
            SoundManager.Instance.SFXPlay(_missionState._clips[1]);
            Debug.Log("사보타지 미션 성공");
            _inputText.text = "성공!";
            _inputText.color = Color.green;
            _inputText.alignment = TextAlignmentOptions.Center;
            _inputText.fontSize = 70;
        }
        else
        {
            Debug.Log("사보타지 미션 실패");
            _inputText.text = "실패";
            _inputText.color = Color.red;
            _inputText.alignment = TextAlignmentOptions.Center;
            _inputText.fontSize = 70;
        }

        _missionController.MissionCoroutine(1f);
    } 
}
