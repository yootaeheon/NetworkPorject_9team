using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLifeSupportMission : MonoBehaviour
{


    //우측 슬롯에 하나씩 추가될 때마다(List의 Count) 이용해서 다 채웠으면 (LeftSlot == RightSlot.Count)
    //Bool 변수 False > True
    //Bool 변수가 바꼈으면 Event 호출 event EventHandler
    //각 요소마다 칩 타입 비교 진행

    //왼쪽 슬롯 리스트
    //왼쪽 칩 리스트

    //오른쪽 슬롯 리스트

    //상, 하 대기 슬롯 리스트
    //오른쪽 슬롯에 넣을 칩 리스트


    //칩들이 어떤 타입인지 가지고 있고, 그 타입을 이용해서 오른쪽 슬롯이랑 리스트 비교해가면 될듯

    //OnEnable
    //왼쪽 슬롯에 있는 칩들 리스트에 집어넣고
    //순서 셔플
    //그리고 첫 번째부터 Position 배치

    //각 슬롯 1 ~ 9 번째 칸 마다 Position을 잡아줄 빈 게임 오브젝트 배치

    //오른쪽 슬롯
    //각 칩들 클릭 후 드래그하면 그 슬롯에 배치
    //배치가 다 끝났을 경우 왼쪽 슬롯과 0번째부터 8번째 까지 타입 비교

    //모든 요소의 타입이 같으면 미션 Clear
    //하나라도 다를 경우 Fail
    private MissionController _missionController;
    private MissionState _missionState;

    private Vector2 temp;

    [SerializeField] private List<RectTransform> _leftSlots = new List<RectTransform>(9);

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _missionController = GetComponent<MissionController>();
        _missionState = GetComponent<MissionState>();

        _missionState.MissionName = "생명 유지 장치 재설정하기";
    }
     
    private void OnEnable()
    {
        //리스트 순서 셔플
        LeftSlotShuffle();
        WaitSlotShuffle();
         
    }


    private void Update()
    {
        _missionController.PlayerInput();
        SelectChip();
    }

    /// <summary>
    /// 왼쪽 슬롯 셔플 기능
    /// </summary>
    private void LeftSlotShuffle()
    {
        for (int i = 0; i < _leftSlots.Count; i++)
        {
            int rand = Random.Range(0, 9);

            //현재 인덱스와 같지 않은 랜덤 인덱스 선택
            if (i == rand)
            {
                while (i != rand)
                {
                    rand = Random.Range(0, 9);
                }
            }

            //위치 교환
            temp = _leftSlots[i].anchoredPosition;
            _leftSlots[i].anchoredPosition = _leftSlots[rand].anchoredPosition;
            _leftSlots[rand].anchoredPosition = temp;

            //리스트 요소 교환
            RectTransform tempRect = _leftSlots[i];
            _leftSlots[i] = _leftSlots[rand];
            _leftSlots[rand] = tempRect; 
        }
    }

    /// <summary>
    /// 대기 슬롯 셔플 기능
    /// </summary>
    private void WaitSlotShuffle()
    {

    }

    /// <summary>
    /// 칩 선택 후 이동 기능
    /// </summary>
    private void SelectChip()
    {
        if (!_missionState.IsDetect) return;

    }


}
