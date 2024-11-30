using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;


public class ReactorChargingMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;
     
    private Animator _chargeAnim;
    private Slider _energySlider;
    private Image _leverColor;

    private bool isPress;
    private int _reverseHash;
    private int _pressHash; 
    private float _elapsedTime;
    private float _notPressTime;
    private bool checkPress;
    private bool isClear;
    private int _flag = 1 << 0;

    private float _r;
    private float _g;
    private float _b;
     
    private void Awake() => Init();
    private void Start()
    {
        _energySlider = _missionController.GetMissionObj<Slider>("LeverSlider");
        _chargeAnim = _missionController.GetMissionObj<Animator>("Lever");
        _leverColor = _missionController.GetMissionObj<Image>("LeverColor");

        _reverseHash = Animator.StringToHash("Reverse");
        _pressHash = Animator.StringToHash("Press");

        _r = _leverColor.color.r;
        _g = _leverColor.color.g;
        _b = _leverColor.color.b;
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "원자로 중심부 충전하기";

        Debug.Log(_missionState.MyPlayerType);
    }

    private void OnEnable()
    {
        _elapsedTime = 0;
        _missionState.ObjectCount = 1;
        isPress = false; 
    }

    private void OnDisable()
    {
        isClear = false;
        _flag = 1 << 0;
        _energySlider.value = 0;
        _leverColor.color = new Color(_r, _g, _b);
    }


    private Coroutine _beepCo;
    private void Update()
    {
        if (GameManager.Instance.GlobalMissionState)
        {
            gameObject.SetActive(false);
        }

        _missionController.PlayerInput();
        LeverPress();
        ChargeEnergy();
         
        if (isPress)
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
        if (!checkPress)
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
            checkPress = true;
            _chargeAnim.SetFloat(_reverseHash, 1);
            _chargeAnim.SetBool(_pressHash, true);
             
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime > 1.5f)
                isPress = true;

            if (_energySlider.value >= 1f)
            {
                isClear = true;
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            _chargeAnim.SetFloat(_reverseHash, -1f);
            _chargeAnim.SetBool(_pressHash, false);
             
            _elapsedTime = 0;
            isPress = false;
            checkPress = false;
        }

        //클리어를 했고 flag가 1일 때 미션 클리어 호출
        if (isClear)
        {
            if(_flag == 1 << 0)
            {
                _flag = 1 << 1;
                _missionState.ObjectCount--;
                MissionClear();
            } 
        } 
    }
     
    /// <summary>
    /// 누르고 있는 상태에 따라 슬라이더 값 변경
    /// </summary>
    private void ChargeEnergy()
    {
        _energySlider.value += isPress ? Time.deltaTime * 0.1f : (-Time.deltaTime * 0.4f);
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

        if (isPress)
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



    /// <summary>
    /// 1초마다 사운드 재생
    /// </summary>
    /// <returns></returns>
    private IEnumerator BeepCoroutine()
    { 
        while (true)
        {
            if(_energySlider.value < 1f)
            {
                SoundManager.Instance.SFXPlay(_missionState._clips[0]); 
            }
            else
            {
                SoundManager.Instance.SFXPlay(_missionState._clips[1]);
            }
            yield return Util.GetDelay(1f);
        }
    }


}
