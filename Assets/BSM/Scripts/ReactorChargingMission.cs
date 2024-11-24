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

    private Animation _animation;
    private Animator _chargeAnim;
    private Slider _energySlider;
    private Image _leverColor;

    private bool IsPress;
    private int reverseHash; 
    private int pressHash; 
    private float _animLength;
    private float _elapsedTime;


    private void Awake() => Init();
    private void Start()
    {
        _energySlider = _missionController.GetMissionObj<Slider>("LeverSlider");
        _chargeAnim = _missionController.GetMissionObj<Animator>("Lever");
        _leverColor = _missionController.GetMissionObj<Image>("LeverColor");

        _animation = _missionController.GetMissionObj<Animation>("Lever");
        _animLength = _animation.clip.length; 
        
        reverseHash = Animator.StringToHash("Reverse");
        pressHash = Animator.StringToHash("Press");

    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "원자로 중심부 충전하기";


    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 1;
        IsPress = false;
        

    }



    private void Update()
    {
        _missionController.PlayerInput(); 
        LeverPress();
        ChargeEnergy();

    }
 

    private void LeverPress()
    {
        if (!_missionState.IsDetect) return;
 
        if (Input.GetMouseButton(0))
        {
            _chargeAnim.SetFloat(reverseHash, 1);
            _chargeAnim.SetBool(pressHash, true);
            //_chargeAnim.Play("LeverUp");

            _elapsedTime += Time.deltaTime;
            
            if (_elapsedTime > _animLength)
                IsPress = true;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            _chargeAnim.SetFloat(reverseHash, -1f);
            _chargeAnim.SetBool(pressHash, false);
            //_chargeAnim.Play("LeverUp");


            _elapsedTime = 0;
            IsPress = false;
        }

    }

    private void ChargeEnergy()
    {
        _energySlider.value += IsPress ? Time.deltaTime*0.1f : (-Time.deltaTime *0.4f);
        ChargeColorChange();
    }

    private void ChargeColorChange()
    { 
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
                if(colorR >= 0.5f)
                {
                    colorR += -(Time.deltaTime * 0.3f);
                } 
            } 
        }
        else
        {
            if(colorR < 1f)
            {
                colorR += Time.deltaTime * 0.3f;
            }
            else
            {
                if(colorG >= 0.12f)
                {
                    colorG += -(Time.deltaTime * (_energySlider.value *6.5f));
                } 
            }

        } 
        _leverColor.color = new Color(colorR, colorG, _leverColor.color.b);

    } 
 
    private void IncreaseTotalScore()
    {
        //Player의 타입을 받아 올 수 있으면 좋음
        PlayerType type = PlayerType.Duck;

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


}
