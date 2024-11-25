using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;



//처음 컬러 값 빨강으로 시작 > 시간이 지나면 G 값 상승
//G 값 255 되면 R 값 60까지 감소 시작

public class ReactorChargingMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;


    private Animator _chargeAnim;
    private Slider _energySlider;
    private Image _leverColor;

    private bool IsPress;
    private int reverseHash;
    private int pressHash; 
    private float _elapsedTime;
    private float _notPressTime;
    private bool _checkPress;

    private float _r;
    private float _g;
    private float _b;


    private void Awake() => Init();
    private void Start()
    {
        _energySlider = _missionController.GetMissionObj<Slider>("LeverSlider");
        _chargeAnim = _missionController.GetMissionObj<Animator>("Lever");
        _leverColor = _missionController.GetMissionObj<Image>("LeverColor");

        reverseHash = Animator.StringToHash("Reverse");
        pressHash = Animator.StringToHash("Press");

        _r = _leverColor.color.r;
        _g = _leverColor.color.g;
        _b = _leverColor.color.b;
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "원자로 중심부 충전하기";


    }

    private void OnEnable()
    {
        _elapsedTime = 0;
        _missionState.ObjectCount = 1;
        IsPress = false;
    }

    private void OnDisable()
    {
        _energySlider.value = 0;
        _leverColor.color = new Color(_r, _g, _b); 
    }


    private Coroutine _beepCo;
    private void Update()
    {
        _missionController.PlayerInput();
        LeverPress();
        ChargeEnergy();



        if (IsPress)
        {
            if(_beepCo == null)
            {
                _beepCo = StartCoroutine(BeepCoroutine());
            }
           
        }
        else
        {
            if (_beepCo != null)
            {
                StopCoroutine(_beepCo);
                _beepCo = null;
            }
        } 
    }


    /// <summary>
    /// 레버 누르고 있는 상태에 따라 애니메이션 재생/되감기 진행
    /// </summary>
    private void LeverPress()
    {
        if (!_missionState.IsDetect) return;

        //레버를 누르고 있는 상태 확인
        //2초 이상 누르고 있지 않은 상태이면 애니메이션 Rebind
        if (!_checkPress)
        {
            _notPressTime += Time.deltaTime;

            if (_notPressTime > 2f && _energySlider.value == 0f)
            {
                _chargeAnim.Rebind();
                _notPressTime = 0;
            }
        }
        else
        {
            _notPressTime = 0;
        } 

        if (Input.GetMouseButton(0))
        {
            _checkPress = true;
            _chargeAnim.SetFloat(reverseHash, 1);
            _chargeAnim.SetBool(pressHash, true);
             
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > 1.5f)
                IsPress = true;

            if (_energySlider.value >= 1f)
            {
                Debug.Log("미션 클리어");
                _missionState.ObjectCount--;
                MissionClear();
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            _chargeAnim.SetFloat(reverseHash, -1f);
            _chargeAnim.SetBool(pressHash, false);
             
            _elapsedTime = 0;
            IsPress = false;
            _checkPress = false;
        } 
    }

    /// <summary>
    /// 누르고 있는 상태에 따라 슬라이더 값 변경
    /// </summary>
    private void ChargeEnergy()
    {
        _energySlider.value += IsPress ? Time.deltaTime * 0.1f : (-Time.deltaTime * 0.4f);
        ChargeColorChange();
    }

    /// <summary>
    /// 누르고 있는 시간 비례하여 슬라이더 컬러 값 변경
    /// </summary>
    private void ChargeColorChange()
    {
        //현재 컬러의 R,G 값
        float colorG = _leverColor.color.g;
        float colorR = _leverColor.color.r;

        if (IsPress)
        {
            if (colorG <= 1f)
            {
                colorG += Time.deltaTime * (_energySlider.value * 0.3f);
            }
            else
            {
                if (colorR >= 0.5f)
                {
                    colorR += -(Time.deltaTime * 0.3f);
                }
            }
        }
        else
        {
            if (colorR < 1f)
            {
                colorR += Time.deltaTime * 0.3f;
            }
            else
            {
                if (colorG >= 0.12f)
                {
                    colorG += -(Time.deltaTime * (_energySlider.value * 6.5f));
                }
            }

        }
        _leverColor.color = new Color(colorR, colorG, _leverColor.color.b);

    }

    private void IncreaseTotalScore()
    {
        //Player의 타입을 받아 올 수 있으면 좋음
        PlayerType type = PlayerType.Goose;

        if (type.Equals(PlayerType.Goose))
        {
            //전체 미션 점수 증가
            //미션 점수 동기화 필요 > 어디서 가져올건지
            GameManager.Instance.TEST();
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



    /// <summary>
    /// 1초마다 사운드 재생
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeepCoroutine()
    {

        while (true)
        {
            SoundManager.Instance.SFXPlay(_missionState._clips[0]);
            yield return Util.GetDelay(1f); 
        } 
    }


}
