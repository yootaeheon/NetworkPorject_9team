using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//처음 컬러 값 빨강으로 시작 > 시간이 지나면 G 값 상승
//G 값 255 되면 R 값 60까지 감소 시작

public class ReactorChargingMission : MonoBehaviour
{
    private MissionController _missionController;
    private MissionState _missionState;

    private Animator _chargeAnim;


    private void Awake() => Init();

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "원자로 중심부 충전하기";
    }

    private void OnEnable()
    {
        _missionState.ObjectCount = 5;
    }

    
    //Input 받아오고,
    //클릭하고 있는 상태
        //Animation True 전달
        //내려가는 애니메이션 재생
    //마우스 뗀 상태
        //False 전달
        //올라오는 애니메이션 재생

    //레버가 다 내려갔는지? > 누르고 있는 시간을 체크해서 검사    

        //레버 상태 = True 
        //게이지 상승

    //게이지 색 > Color.G 증가
        //Color.G > 1 ? 
            //Color.R 증가
        

    //Slider Value >= 1 ?
        //mission clear
        

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
