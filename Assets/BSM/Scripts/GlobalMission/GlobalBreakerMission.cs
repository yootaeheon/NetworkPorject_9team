using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalBreakerMission : MonoBehaviour
{
    private MissionState _missionState;
    private MissionController _missionController;



    //버튼을 클릭했을 때 애니메이션 재생
    //버튼 ON/OFF 사운드 재생
    //불빛 애니메이션 재생
    //클릭할 때 마다 GlobalButton이 가지고 있는 Count를 감소
    //Count가 0이 됐을 때
    //전원을 on/off 할 수 있게

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();
        _missionState.MissionName = "전등 고치기";
    }

    private void Update()
    {
        _missionController.PlayerInput();

        Interaction();
    }

    private void Interaction()
    {



    }


}
