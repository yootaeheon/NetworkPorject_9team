using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerVentUsable : MonoBehaviourPun
{
    private PlayerController _player;
    private Vent _vent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (photonView.IsMine == false) 
            return;
        if (_player.playerType == PlayerType.Goose)
            return;



        if(_enterTriggerRoutine == null)
        {
            _enterTriggerRoutine = StartCoroutine(EnterTriggerRoutine(collision));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (photonView.IsMine == false) 
            return;
        if (_player.playerType == PlayerType.Goose) 
            return;


        if (_enterTriggerRoutine != null)
        {
            StopCoroutine(_enterTriggerRoutine);
            _enterTriggerRoutine = null;
        }
    }

    Coroutine _enterTriggerRoutine;
    /// <summary>
    /// 트리거 진입 시 벤트 입력 대기
    /// </summary>
    IEnumerator EnterTriggerRoutine(Collider2D collision)
    {
        while (true)
        {
            // 벤트에서 왼쪽 쉬프트 클릭시
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vent vent = collision.GetComponent<Vent>();
                if (vent == null)
                    yield break;
                EnterVent(vent);
            }
            yield return null;  
        }
    }

    /// <summary>
    /// 벤트 입장
    /// </summary>
    private void EnterVent(Vent vent)
    {
        // 벤트 등록
        _vent = vent;
        // 벤트 변경 이벤트 등록
        _vent.OnChangeVentEvent += ChangeVent;

        // 카메라 설정 
        // TODO: 시네머신에 따라 코드 변경 가능성 존재
        Camera.main.transform.SetParent(vent.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        // 플레이어 위치 안보이는곳에 배치
        Vector2 tempPos = new Vector2(10000, 10000);
        transform.position = tempPos;

        // 벤트 입장
        vent.Enter(Vent.ActorType.Enter);

        // 입력 대기 코루틴 시작
        if (_enterVentRoutine == null)
            _enterVentRoutine = StartCoroutine(EnterVentRoutine());
    }

    /// <summary>
    /// 벤트 속에서 코루틴
    /// </summary>
    Coroutine _enterVentRoutine;
    IEnumerator EnterVentRoutine()
    {
        while (true)
        {
            yield return null;
            // 벤트 나오기
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ExitVent();
                _enterVentRoutine = null;
                yield break;
            }       
        }
    }

    /// <summary>
    /// 벤트 나오기
    /// </summary>
    private void ExitVent()
    {
        // 카메라 다시 플레이어 기준
        // TODO : 시네머신에 따라 코드 변경해야함
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        // 벤트 이벤트 구독 해제
        _vent.OnChangeVentEvent -= ChangeVent;

        // 플레이어 위치 벤트 위치로 이동
        transform.position = _vent.transform.position;
        // 벤트 퇴장
        _vent.Exit(Vent.ActorType.Enter);
    }

    /// <summary>
    /// 벤트 교체(이동)
    /// </summary>
    private void ChangeVent(Vent vent)
    {
        // 이전 벤트 구독 해제
        _vent.OnChangeVentEvent -= ChangeVent;

        // 이전 벤트 퇴장
        _vent.Exit(Vent.ActorType.Change);

        // 벤트 교체
        _vent = vent;
        
        // 현재 벤트 이벤트 구독
        vent.OnChangeVentEvent += ChangeVent;

        // 카메라 이동
        Camera.main.transform.SetParent(_vent.transform);
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);

        // 벤트 입장
        vent.Enter(Vent.ActorType.Change);
    }
}
